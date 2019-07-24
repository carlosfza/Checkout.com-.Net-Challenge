using Checkout.Models;
using Checkout.Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Services
{
    public class TransactionInformationService : ITransactionInformationService
    {
        private ITransactionRepository transactionRepository;

        private ILogger logger;

        public TransactionInformationService(ITransactionRepository transactionRepository, ILogger logger)
        {
            this.transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TransactionDetailResponse> ProcessTransactionDetail(string transactionIdentifier, string storeId)
        {
            CardInfo persistedCard = await transactionRepository.ObtainCardInfoAsync(transactionIdentifier, storeId);

            if(persistedCard != null)
            {
                CardInfo maskedCardInfo = new CardInfo()
                {
                    HolderFirstName = persistedCard.HolderFirstName,
                    HolderLastName = persistedCard.HolderLastName,
                    CardNumber = persistedCard.CardNumber.Substring(0,4) + "xxx-xxx-xxx",
                    Ccv = -1,
                    ExpirationDate = persistedCard.ExpirationDate
                };

                logger.Information($"Succesfully provided transaction detail of transaction: '{transactionIdentifier}' to store: '{storeId}'");
                return new TransactionDetailResponse()
                {
                    IsSuccessful = true,
                    CardInfo = maskedCardInfo,
                };
            }
            else
            {
                logger.Error($"Store: '{storeId}' requested transaction: '{transactionIdentifier}' unsuccesfully");
                return new TransactionDetailResponse()
                {
                    IsSuccessful = false,
                    CardInfo = null
                };
            }
        }
        
    }
}
