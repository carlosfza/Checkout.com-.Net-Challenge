using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Models
{
    public class TransactionDetailResponse
    {
        /// <summary>
        /// Whether the request for card detail is successful
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Masked card information
        /// </summary>
        public CardInfo CardInfo { get; set; }
    }
}
