using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.Models
{
    /// <summary>
    /// Model class that emcompasses all information of a payment card info
    /// </summary>
    public class CardInfo
    {
        public CardInfo()
        {

        }
        /// <summary>
        /// Card number, formatted as a string
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Expiration date
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// First name of holder
        /// </summary>
        public string HolderFirstName { get; set; }

        /// <summary>
        /// Last Name of holder
        /// </summary>
        public string HolderLastName { get; set; }

        /// <summary>
        /// Card security code
        /// </summary>
        public int Ccv { get; set; }
    }
}
