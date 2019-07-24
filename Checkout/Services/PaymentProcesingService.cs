using Checkout.Models;
using Checkout.Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Services
{
    /// <summary>
    /// Payment processor
    /// </summary>
    public class PaymentProcessor : IPaymentProcessor
    {
        
        private Dictionary<string, IBankService> bankServices;

        private ITransactionRepository transactionRepository;

        private ILogger logger;

        /// <summary>
        /// Dependency injection constructor
        /// </summary>
        /// <param name="bankServices"></param>
        /// <param name="transactionRepository"></param>
        /// <param name="logger"></param>
        public PaymentProcessor(Dictionary<string, IBankService> bankServices, ITransactionRepository transactionRepository, ILogger logger)
        {
            this.bankServices = bankServices ?? throw new ArgumentNullException(nameof(bankServices));
            this.transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void AddBankService(IBankService bankService, string bankIdentifier) => bankServices?.Add(bankIdentifier, bankService);

        public async Task<PaymentResponse>  ProcessPaymentAsync(PaymentRequest paymentRequest, string storeId)
        {
            //find the correct bank to contact
            //contact the bank, providing request information
            BankResponse bankResponse = null;
            string bankServiceSelected = null;
            foreach (var bankService in bankServices)
            {
                if (bankService.Value.IsCardManaged(paymentRequest.CardInfo.CardNumber))
                {
                    bankServiceSelected = bankService.Key;
                    bankResponse = await bankService.Value.RequestPaymentAsync(paymentRequest.CardInfo, paymentRequest.Ammount, paymentRequest.Currency);
                    break;
                }
            }

            if (bankServiceSelected == null)
            {
                logger.Error($"No bank configured to process the current card");
                return new PaymentResponse()
                {
                    IsSuccesful = false,
                    TransactionIdentifier = null,
                    ErrorInformation = "Cannot accept payment selected provider."
                };
            }

            //process result, save card info if needed
            if(bankResponse != null && bankResponse.IsSuccess)
            {
                if (paymentRequest.SaveCredentials)
                {
                    bool persistResult = await transactionRepository.StorePaymentInfo(
                        bankResponse.TransactionIdentifier, 
                        paymentRequest.CardInfo, 
                        storeId);
                }
                return new PaymentResponse()
                {
                    IsSuccesful = true,
                    ErrorInformation = "",
                    TransactionIdentifier = bankResponse.TransactionIdentifier
                };
            }
            else
            {
                logger.Error($"Transaction unsuccessful. Bank: {bankServiceSelected}, Store: {storeId}, reason: {bankResponse?.OptionalError}");
                return new PaymentResponse() {
                    IsSuccesful = false,
                    TransactionIdentifier = null,
                    ErrorInformation = "The payment has not been accepted by the bank service."
                };
            }
        }
    }
}
