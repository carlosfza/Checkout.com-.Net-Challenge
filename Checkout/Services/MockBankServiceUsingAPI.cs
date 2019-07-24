using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Models;

namespace Checkout.Services
{
    /// <summary>
    /// Second mock bank service
    /// Denotes how a real request may be, but it is not implemented, instead only with high level instructions
    /// </summary>
    public class MockBankServiceUsingAPI : IBankService
    {
        public bool IsCardManaged(string cardNumber)
        {
            //This method would need to look at the number and compute whether it belongs to this provider
            //No API would be called, because as far as I know, we can know the bank from the card number
            //Otherwise perform a REST call to the bank API
            return true;
        }

        public async Task<BankResponse> RequestPaymentAsync(CardInfo cardInfo, long ammount, string currencyCode)
        {
            //This method would be made to know excactly what API each bank service provides.
            //Because different banks may provide different api, many bank services were modeled.
            //Due to time constraints, I did not develop a bank API to mock.
            //Therefore the bellow code is for illustration purposes only, and will not work if attempted


            return null;
        }

    }
}
