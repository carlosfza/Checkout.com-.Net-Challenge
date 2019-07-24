using System.Threading.Tasks;
using Checkout.Models;

namespace Checkout.Repositories
{
    /// <summary>
    /// Interface for persisting the data in the gateway
    /// In simple scenarious can be implemented in a database
    /// In more complex scenarious might request the data from remote services
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Obtains information on a card that is persisted
        /// </summary>
        /// <param name="transactionId">Identifier of the payment transaction</param>
        /// <param name="storeCode">Store identifier</param>
        /// <returns>Masked information, is not masked</returns>
        Task<CardInfo> ObtainCardInfoAsync(string transactionId, string storeCode);
        
        /// <summary>
        /// Persist a card information
        /// </summary>
        /// <param name="transactionId">Transaction in which the information was used</param>
        /// <param name="cardUsed">Card information object</param>
        /// <param name="storeId">Store that performed the transaction</param>
        /// <returns>Whether the operation was successful</returns>
        Task<bool> StorePaymentInfo(string transactionId, CardInfo cardUsed, string storeId);
    }
}