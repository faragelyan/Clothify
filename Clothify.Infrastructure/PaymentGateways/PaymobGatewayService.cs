using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Clothify.Application.DTOs;
using Clothify.Application.DTOs.Payment;
using Clothify.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Clothify.Infrastructure.PaymentGateways
{
    public class PaymobGatewayService : IPaymentGatewayService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PaymobGatewayService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<Result<string>> InitiateWalletPaymentAsync(decimal totalAmount, string merchantOrderId, string phoneNumber)
        {
            var apiKey = _configuration["Paymob:ApiKey"];
            if (string.IsNullOrEmpty(apiKey)) return Result<string>.Fail("Paymob configuration is missing");

            try
            {
                // 1. Authentication
                var authResponse = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/auth/tokens", new { api_key = apiKey });
                if (!authResponse.IsSuccessStatusCode)
                {
                    var errorBody = await authResponse.Content.ReadAsStringAsync();
                    return Result<string>.Fail($"Paymob Auth failed: {authResponse.StatusCode} - {errorBody}");
                }
                var authResult = await authResponse.Content.ReadFromJsonAsync<JsonElement>();
                var authToken = authResult.GetProperty("token").GetString();

                // 2. Order Registration
                int amountCents = (int)(totalAmount * 100);
                var orderReq = new
                {
                    auth_token = authToken,
                    delivery_needed = false,
                    amount_cents = amountCents,
                    currency = "EGP",
                    merchant_order_id = merchantOrderId
                };

                var orderResponse = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/ecommerce/orders", orderReq);
                if (!orderResponse.IsSuccessStatusCode)
                {
                    var errorBody = await orderResponse.Content.ReadAsStringAsync();
                    return Result<string>.Fail($"Paymob Order Registration failed: {orderResponse.StatusCode} - {errorBody}");
                }
                var orderResult = await orderResponse.Content.ReadFromJsonAsync<JsonElement>();
                var paymobOrderId = orderResult.GetProperty("id").GetInt32();

                // 3. Payment Key Generation
                var paymentKeyReq = new
                {
                    auth_token = authToken,
                    amount_cents = amountCents,
                    expiration = 3600,
                    order_id = paymobOrderId,
                    billing_data = new
                    {
                        apartment = "NA",
                        email = "customer@clothify.com",
                        floor = "NA",
                        first_name = "Clothify",
                        street = "NA",
                        building = "NA",
                        phone_number = phoneNumber,
                        shipping_method = "NA",
                        postal_code = "NA",
                        city = "NA",
                        country = "EG",
                        last_name = "User",
                        state = "NA"
                    },
                    currency = "EGP",
                    integration_id = 5621142 // Wallet Integration ID
                };

                var keyResponse = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/acceptance/payment_keys", paymentKeyReq);
                if (!keyResponse.IsSuccessStatusCode)
                {
                    var errorBody = await keyResponse.Content.ReadAsStringAsync();
                    return Result<string>.Fail($"Paymob Payment Key Generation failed: {keyResponse.StatusCode} - {errorBody}");
                }
                var keyResult = await keyResponse.Content.ReadFromJsonAsync<JsonElement>();
                var paymentToken = keyResult.GetProperty("token").GetString();

                // 4. Pay with Wallet
                var payReq = new
                {
                    source = new { identifier = phoneNumber, subtype = "WALLET" },
                    payment_token = paymentToken
                };

                var payResponse = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/acceptance/payments/pay", payReq);
                if (!payResponse.IsSuccessStatusCode)
                {
                    var errorBody = await payResponse.Content.ReadAsStringAsync();
                    return Result<string>.Fail($"Paymob Wallet Initialization failed: {payResponse.StatusCode} - {errorBody}");
                }
                
                var payResult = await payResponse.Content.ReadFromJsonAsync<JsonElement>();
                var redirectUrl = payResult.GetProperty("iframe_redirection_url").GetString();

                return Result<string>.Ok(redirectUrl!);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Payment Gateway Error: {ex.Message}");
            }
        }

        public bool VerifyWebhookHmac(string providedHmac, PaymobCallbackObject payloadObj)
        {
            var hmacSecret = _configuration["Paymob:HmacSecret"];
            if (string.IsNullOrEmpty(hmacSecret)) return true; // Fail open if no secret configured for backward compat during dev

            try
            {
                var amount_cents = payloadObj.amount_cents.ToString();
                var created_at = payloadObj.created_at ?? ""; 
                var currency = payloadObj.currency ?? "";
                var error_occured = payloadObj.error_occured.ToString().ToLower();
                var has_parent_transaction = payloadObj.has_parent_transaction.ToString().ToLower();
                var objId = payloadObj.id.ToString();
                var integration_id = payloadObj.integration_id.ToString();
                var is_3d_secure = payloadObj.is_3d_secure.ToString().ToLower();
                var is_auth = payloadObj.is_auth.ToString().ToLower();
                var is_capture = payloadObj.is_capture.ToString().ToLower();
                var is_refunded = payloadObj.is_refunded.ToString().ToLower();
                var is_standalone_payment = payloadObj.is_standalone_payment.ToString().ToLower();
                var is_voided = payloadObj.is_voided.ToString().ToLower();
                var orderId = payloadObj.order?.id.ToString() ?? "";
                var owner = payloadObj.owner.ToString();
                var pending = payloadObj.pending.ToString().ToLower();
                var source_data_pan = payloadObj.source_data?.pan ?? "";
                var source_data_sub_type = payloadObj.source_data?.sub_type ?? "";
                var source_data_type = payloadObj.source_data?.type ?? "";
                var success = payloadObj.success.ToString().ToLower();

                var concatenatedString = amount_cents + created_at + currency + error_occured + has_parent_transaction +
                                         objId + integration_id + is_3d_secure + is_auth + is_capture + is_refunded +
                                         is_standalone_payment + is_voided + orderId + owner + pending +
                                         source_data_pan + source_data_sub_type + source_data_type + success;

                using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(hmacSecret));
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(concatenatedString));
                var calculatedHmac = BitConverter.ToString(hash).Replace("-", "").ToLower();

                return calculatedHmac == providedHmac.ToLower();
            }
            catch
            {
                return false;
            }
        }
    }
}
