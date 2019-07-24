using Checkout.Models;
using System.Threading.Tasks;

namespace Checkout.Services
{
    /// <summary>
    /// Bank service interface
    /// Each bank may require different workflows for processing payments
    /// Each Bank service knows how to request the payment processing for one specific bank
    /// </summary>
    public interface IBankService
    {
        /// <summary>
        /// Request a payment
        /// </summary>
        /// <param name="cardInfo">User credit card information</param>
        /// <param name="ammount">Ammount in cents</param>
        /// <param name="currencyCode">Currency for payment</param>
        /// <returns><see cref="BankResponse"/> instance containing the transaction and whether it was successful</returns>
        Task<BankResponse> RequestPaymentAsync(CardInfo cardInfo, long ammount, string currencyCode);

        /// <summary>
        /// Verifies if this bank service can process payment of a card, denoted by its number
        /// </summary>
        /// <param name="cardNumber">Card number in string format</param>
        /// <returns>Whether this is the correct service to process the card</returns>
        bool IsCardManaged(string cardNumber);
    }

    /// <summary>
    /// Response object for bank transaction processing
    /// May be refactored later to become generic and be the standard reponse of multiple bank operations
    /// </summary>
    public class BankResponse
    {
        public BankResponse(string transactionIdentifier, bool isSuccess, string optionalError)
        {
            TransactionIdentifier = transactionIdentifier;
            IsSuccess = isSuccess;
            OptionalError = optionalError;
        }

        /// <summary>
        /// Operation identifier
        /// </summary>
        public string TransactionIdentifier { get; private set; }

        /// <summary>
        /// Whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Any error reported by the bank. Might contain private information, handle with care.
        /// </summary>
        public string OptionalError { get; private set; }
    }
}