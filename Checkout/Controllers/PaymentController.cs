using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Checkout.Models;
using Checkout.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Checkout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger logger;

        private readonly ITransactionInformationService transactionService;

        private readonly IPaymentProcessor paymentProcessor;

        public PaymentController(ILogger logger, ITransactionInformationService transactionService, IPaymentProcessor paymentProcessor)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
            this.paymentProcessor = paymentProcessor ?? throw new ArgumentNullException(nameof(paymentProcessor));
        }

        // try this!  localhost:5000/api/payment/TransactionDetails?transactionId=sample
        [HttpGet("TransactionDetails")]
        [Authorize(Policy= "RequireRetrieveScope")]
        public async Task<IActionResult> GetTransactionDetails(string transactionId)
        {
            string storeId = GetStoreId(User);
            if (storeId == null)
                return BadRequest();

            TransactionDetailResponse res = await transactionService.ProcessTransactionDetail(transactionId, storeId);
            return Ok(res);
        }

        /*
         * Dont forget to run as Development environment, otherwise security claims will be validated, resulting in forbidden for both!
         try this!  localhost:5000/api/payment/RequestPayment

            in the body as application/json
        {
          "ammount": 1,
          "currency": "usd",
          "cardInfo": {
	          "cardNumber": "1234-1234-1234-1234",
	          "expirationDate": "0001-01-01T00:00:00",
	          "holderFirstName": "John",
	          "holderLastName": "Doe",
	          "ccv": 213
	        },
          "saveCredentials": true
        }
        */
        [HttpPost("RequestPayment")]
        [Authorize(Policy = "RequreProcessScope")]
        public async Task<IActionResult> RequestPayment(PaymentRequest paymentRequest)
        {
            string storeId = GetStoreId(User);
            if (storeId == null)
                return BadRequest();
            
            //TODO model validation!

            PaymentResponse res = await paymentProcessor.ProcessPaymentAsync(paymentRequest, storeId);
            return Ok(res);
        }

        private string GetStoreId(ClaimsPrincipal user)
        {
            if(user.HasClaim(c => c.Type == "client_id"))
            {
                return user.Claims.FirstOrDefault(c => c.Type == "client_id").Value;
            }
            return null;
        }
        public IActionResult Get()
        {
            return Ok(new List<string>() { "Hello world. This is my sample. Use RequestPayment and TransactionDetails" });
        }
    }
}