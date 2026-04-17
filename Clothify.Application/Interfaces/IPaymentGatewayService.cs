using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Payment;

namespace Clothify.Application.Interfaces
{
    public interface IPaymentGatewayService
    {
        Task<Result<string>> InitiateWalletPaymentAsync(decimal totalAmount, string merchantOrderId, string phoneNumber);
        bool VerifyWebhookHmac(string providedHmac, PaymobCallbackObject payloadObj);
    }
}
