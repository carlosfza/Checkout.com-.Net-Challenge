using System.Threading.Tasks;
using Checkout.Models;

namespace Checkout.Services
{
    /// <summary>
    /// Interface defining a service that obains transaction information
    /// </summary>
    public interface ITransactionInformationService
    {
        /// <summary>
        /// Onbtains a redacted card information from the configured repository
        /// </summary>
        /// <param name="transactionIdentifier">Identifier of the transaction</param>
        /// <param name="storeId">Identifier of the store</param>
        /// <returns>Object response containing redacted card info if found</returns>
        Task<TransactionDetailResponse> ProcessTransactionDetail(string transactionIdentifier, string storeId);
    }
}