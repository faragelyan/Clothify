using System.Net.Http.Json;
using System.Text.Json;
using AutoMapper;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Payment;
using Clothify.Application.Interfaces;
using Clothify.Domain.Entities;
using Clothify.Domain.Enums;
using Clothify.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Clothify.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentGatewayService _paymentGatewayService;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentGatewayService paymentGatewayService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentGatewayService = paymentGatewayService;
        }

        public async Task<Result<Guid>> AddAsync(CreatePaymentDto dto)
        {
            var payment = _mapper.Map<Payment>(dto);
            payment.Status = PaymentStatus.Pending;

            var added = await _unitOfWork.Payments.AddAsync(payment);
            if (!added)
                return Result<Guid>.Fail("Failed to add payment");

            await _unitOfWork.CommitAsync();
            return Result<Guid>.Ok(payment.PaymentId);
        }

        public async Task<Result<bool>> UpdateAsync(UpdatePaymentDto dto)
        {
            var payment = await _unitOfWork.Payments.GetSingleEntityAsync(
                filter: p => p.PaymentId == dto.PaymentId
            );

            if (payment is null)
                return Result<bool>.Fail("Payment not found");

            _mapper.Map(dto, payment);

            var updated = _unitOfWork.Payments.Update(payment);
            if (!updated)
                return Result<bool>.Fail("Failed to update payment");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> RemoveAsync(Guid paymentId)
        {
            var payment = await _unitOfWork.Payments.GetSingleEntityAsync(
                filter: p => p.PaymentId == paymentId
            );

            if (payment is null)
                return Result<bool>.Fail("Payment not found");

            var deleted = _unitOfWork.Payments.Delete(payment);
            if (!deleted)
                return Result<bool>.Fail("Failed to delete payment");

            await _unitOfWork.CommitAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<IReadOnlyList<PaymentDto>>> GetAllAsync()
        {
            var payments = await _unitOfWork.Payments.GetAllEntitiesAsync(
                orderBy: q => q.OrderByDescending(p => p.PaymentDate),
                disableTracking: true
            );

            var result = _mapper.Map<IReadOnlyList<PaymentDto>>(payments);
            return Result<IReadOnlyList<PaymentDto>>.Ok(result);
        }

        public async Task<Result<PaymentDto>> GetAsync(Guid paymentId)
        {
            var payment = await _unitOfWork.Payments.GetSingleEntityAsync(
                filter: p => p.PaymentId == paymentId,
                disableTracking: true
            );

            if (payment is null)
                return Result<PaymentDto>.Fail("Payment not found");

            var dto = _mapper.Map<PaymentDto>(payment);
            return Result<PaymentDto>.Ok(dto);
        }

        public async Task<Result<string>> PayWithWalletAsync(PayWithWalletDto dto)
        {
            var order = await _unitOfWork.Orders.GetSingleEntityAsync(filter: o => o.OrderId == dto.OrderId);
            if (order == null) return Result<string>.Fail("Order not found");

            // 1. Record the intent to pay locally first to obtain the strict natively database-bound Guid
            var payment = new Payment
            {
                Amount = order.TotalAmount,
                Currency = "EGP",
                OrderId = order.OrderId,
                PaymentMethod = PaymentMethod.Wallet,
                Status = PaymentStatus.Pending
            };
            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.CommitAsync();

            // 2. We use the resulting distinct PaymentId as the pure Paymob merchant_order_id
            // This grants completely perfect 1:1 webhook mapping isolation.
            var uniqueMerchantOrderId = payment.PaymentId.ToString();

            // Initiate payment through the decoupled gateway service
            var gatewayResult = await _paymentGatewayService.InitiateWalletPaymentAsync(order.TotalAmount, uniqueMerchantOrderId, dto.PhoneNumber);
            if (!gatewayResult.Success)
            {
                 // Instantly correct the local DB status if outward HTTP failed
                 payment.Status = PaymentStatus.Failed;
                 _unitOfWork.Payments.Update(payment);
                 await _unitOfWork.CommitAsync();

                 return Result<string>.Fail(gatewayResult.Error);
            }

            return Result<string>.Ok(gatewayResult.Data);
        }

        public async Task<Result<bool>> ProcessCallbackAsync(PaymobCallbackDto callback)
        {
            var rawMerchantOrderId = callback.obj?.order?.merchant_order_id;
            if (string.IsNullOrEmpty(rawMerchantOrderId))
            {
                return Result<bool>.Fail("Invalid callback data");
            }

            // The callback delivers our PaymentId natively since we strictly transmitted it above
            if (!Guid.TryParse(rawMerchantOrderId, out Guid paymentId))
            {
                return Result<bool>.Fail("Invalid merchant order ID");
            }

            // Precisely fetch the exact row without relying on OrderId chronologically
            var payment = await _unitOfWork.Payments.GetSingleEntityAsync(
                filter: p => p.PaymentId == paymentId,
                disableTracking: false
            );

            if (payment == null) 
            {
                return Result<bool>.Fail($"Payment record not strictly synced yet for PaymentId {paymentId}");
            }

            // Idempotency / Replay protection exactly evaluates existing states organically:
            if (payment.Status == PaymentStatus.Completed)
            {
                return Result<bool>.Ok(true); // Ignore duplicate successful webhook gracefully
            }
            if (payment.Status == PaymentStatus.Failed && !callback.obj.success)
            {
                return Result<bool>.Ok(true); // Avoid repeating fail actions from Paymob retries
            }

            // Amount Validation
            var expectedAmountCents = (int)(payment.Amount * 100);
            if (callback.obj.amount_cents != expectedAmountCents)
            {
                payment.Status = PaymentStatus.Failed;
                _unitOfWork.Payments.Update(payment);
                await _unitOfWork.CommitAsync();
                return Result<bool>.Fail("Payment amount mismatch");
            }

            if (callback.obj.success)
            {
                payment.Status = PaymentStatus.Completed;

                // Sync Order Status
                var order = await _unitOfWork.Orders.GetSingleEntityAsync(o => o.OrderId == payment.OrderId);
                if (order != null)
                {
                    order.Status = OrderStatus.Processing;
                    _unitOfWork.Orders.Update(order);
                }
            }
            else
            {
                payment.Status = PaymentStatus.Failed;
            }

            _unitOfWork.Payments.Update(payment);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Ok(true);
        }
    }
}
