using System;
using System.Threading.Tasks;
using PharmaSphere.Services.Interfaces;

namespace PharmaSphere.Services.Implementations
{
    /// <summary>
    /// Mock implementation of a payment gateway (e.g. Stripe).
    /// </summary>
    public class StripePaymentGateway : IPaymentGateway
    {
        public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, PaymentInfo paymentInfo)
        {
            // Simulate processing
            await Task.Delay(1000);

            if (paymentInfo.CardNumber.StartsWith("4")) // Visa mock logic
            {
                return new PaymentResult
                {
                    IsSuccess = true,
                    TransactionId = "STRIPE_" + Guid.NewGuid().ToString("N")
                };
            }

            return new PaymentResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Thanh toán thất bại. Vui lòng kiểm tra thông tin thẻ."
                };
        }

        public async Task<bool> RefundPaymentAsync(string transactionId)
        {
            await Task.Delay(500);
            return true;
        }
    }
}
