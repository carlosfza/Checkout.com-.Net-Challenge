using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Models
{
    public class PaymentRequest
    {
        public PaymentRequest()
        {

        }

        /// <summary>
        /// Ammount of money to be paid. Notation is in cents.
        /// </summary>
        public long Ammount { get; set; }

        /// <summary>
        /// Currency code
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Information of card to be used for payment
        /// </summary>
        public CardInfo CardInfo { get; set; }

        /// <summary>
        /// Whether the card credentials should be saved
        /// </summary>
        public bool SaveCredentials { get; set; }
    }
}
