using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Models;

namespace Checkout.Services
{
    /// <summary>
    /// First mock bank service
    /// Mocks a bank, without needing a bank server
    /// </summary>
    public class MockBankService1 : IBankService
    {
        public bool IsCardManaged(string cardNumber)
        {
            return true;
        }

        public async Task<BankResponse> RequestPaymentAsync(CardInfo cardInfo, long ammount, string currencyCode)
        {
            if (cardInfo.CardNumber.Length < 5)
                return new BankResponse(null, false, "Card number invalid");

            return new BankResponse(GenerateRandomString(), true, "");
        }

        private static char CalculateNextRandom(Random random)
        {
            var chars = "abcdefghijklmnopqrstuvxyz";
            return chars[random.Next(0, chars.Length - 1)];
        }

        private static string GenerateRandomString()
        {
            Random random = new Random();
            char[] c = new char[40];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = CalculateNextRandom(random);
            }
            return new string(c);
        }
    }
}
