using Checkout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Repositories
{
    /// <summary>
    /// Local implementation of the repository
    /// Stores all information on memory
    /// Somewhat low fidelity mock
    /// </summary>
    public class MockTransactionRepository : ITransactionRepository
    {
        public MockTransactionRepository()
        {
            TranscationStoreCodes.Add("sample", "store11111111");
            PersistedCardInfo.Add("sample", new CardInfo()
            {
                ExpirationDate = DateTime.Now.AddYears(2),
                CardNumber = "1232-1234-1234-1234",
                HolderFirstName="John",
                HolderLastName="Doe",
                Ccv=123
            });
        }
        private Dictionary<string, CardInfo> PersistedCardInfo = new Dictionary<string, CardInfo>();

        private Dictionary<string, string> TranscationStoreCodes = new Dictionary<string, string>();

        public async Task<CardInfo> ObtainCardInfoAsync(string transactionId, string storeCode)
        {
            CardInfo persistedCard;
            string transactionStoreCode;
            if (PersistedCardInfo.TryGetValue(transactionId, out persistedCard) && TranscationStoreCodes.TryGetValue(transactionId, out transactionStoreCode))
            {
                //In a real database this would probably be a join. Here, we just check
                if (transactionStoreCode != storeCode)
                {
                    return null;
                }
                return persistedCard;
            }
            else
                return null;
        }

        public async Task<bool> StorePaymentInfo(string transactionId, CardInfo cardUsed, string storeId)
        {
            PersistedCardInfo.Add(transactionId, cardUsed);
            TranscationStoreCodes.Add(transactionId, storeId);
            return true;
        }
    }
}
