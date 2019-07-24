using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Models
{
    public class PaymentResponse
    {
        /// <summary>
        /// Denotes whether the transaction was successful
        /// </summary>
        public bool IsSuccesful { get; set; }

        /// <summary>
        /// Additional information regarding errors. May originate from gateway or bank
        /// </summary>
        public string ErrorInformation { get; set; }

        /// <summary>
        /// Unique transaction identifier
        /// </summary>
        public string TransactionIdentifier { get; set; }
    }
}
