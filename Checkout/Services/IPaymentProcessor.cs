using System.Threading.Tasks;
using Checkout.Models;

namespace Checkout.Services
{
    /// <summary>
    /// Interface defining the payment processor service
    /// </summary>
    public interface IPaymentProcessor
    {
        /// <summary>
        /// Add a <see cref="IBankService"/>
        /// </summary>
        /// <param name="bankService">Bank service instance</param>
        /// <param name="bankIdentifier">Identifier of the bank</param>
        void AddBankService(IBankService bankService, string bankIdentifier);

        /// <summary>
        /// Performs the task of requesting payment from a bank, using the card information
        /// </summary>
        /// <param name="paymentRequest">Payment request</param>
        /// <param name="storeId">Identifier of the store that requested the payment</param>
        /// <returns>Payment response instance containing the operation result information</returns>
        Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest paymentRequest, string storeId);
    }
}