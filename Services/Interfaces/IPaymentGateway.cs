using System;
using System.Threading.Tasks;

namespace PharmaSphere.Services.Interfaces
{
    public interface IPaymentGateway
    {
        Task<PaymentResult> ProcessPaymentAsync(decimal amount, PaymentInfo paymentInfo);
        Task<bool> RefundPaymentAsync(string transactionId);
    }

    public class PaymentInfo
    {
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public string CardHolderName { get; set; }
    }

    public class PaymentResult
    {
        public bool IsSuccess { get; set; }
        public string TransactionId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
