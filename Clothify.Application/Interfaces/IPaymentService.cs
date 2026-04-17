using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Payment;

namespace Clothify.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<Result<Guid>> AddAsync(CreatePaymentDto dto);
        Task<Result<bool>> UpdateAsync(UpdatePaymentDto dto);
        Task<Result<bool>> RemoveAsync(Guid paymentId);
        Task<Result<IReadOnlyList<PaymentDto>>> GetAllAsync();
        Task<Result<PaymentDto>> GetAsync(Guid paymentId);

        Task<Result<string>> PayWithWalletAsync(PayWithWalletDto dto);
        Task<Result<bool>> ProcessCallbackAsync(PaymobCallbackDto callback);
    }
}
