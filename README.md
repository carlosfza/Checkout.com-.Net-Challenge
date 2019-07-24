# Checkout.com-.Net-Challenge
Checkout.com challenge solution

## Solution description. 

In order to solve this challenge, I ecided to focus on developing a solution that segregates successfully various software elements that this application requires, which are the controller, the various models, services to handle each request, and a repository to save card information. 

  In the startup class, the implementation of each interface is set in the IOC container so that in general only the model classes are instantiated throughout the processing of the requests. While logging is configured, and in some occasions used, the configuration depends on the web.config file and no sinks have been attached for development.
  
  The solution design is mostly based on a set of interfaces defined. Afterwards, some of these interfaces were developed fully, while others were mocked. Interfaces mocked include a service that is required to understand the API of one specific bank, and the card information and transaction repository. The reason for this is because the implementation for each one of these services will vary depending on architectural decisions that are not completely specified, and thus open for discussion.
  
  Regarding further steps, if this was not an exercise project, I would discuss with the development team solutions for the persistence of the data, evaluating local and distributed data persistence. Additionally, further care should be taken in the IBankService development, in order to make sure that the interface can encompass the requirements for several banking services.
  
  Due to having to focus on my PhD thesis, I cannot dedicate anymore time to this project. The current solution represents about somewhat under a full day of work. Because of this, no time was available for unit and integration testing. The code is commented and I hope it is easy to navigate and understand. In developing this exercise the materials consulted were several Microsoft documentation pages, library documentation pages, as well as a fair amount of web searches. I would be available for a phone call where questions are asked regarding technical or architectural decisions.


## How to run: 

  Clone the solution in Visual Studio
  
  Run the Solution the Checkout project Visual Studio
  
  The TransactionDetails API can be used in a browser, meanwhile RequestPayment is better tested in another application, such as Postman
  
## Notes: 
  Tested internally with Kerberos hosting outside of IIS.
  
  Run in with the environment set as "Development". Running as production enabled https redirection, OpenID Connect Client authentication bearer token validation. In Development mode a mock identity is injected, allowing access to the methods.
  
  Using the default configurations. Http runs in port 5000, https in 5001. Better to use http for testing.
  
## APIs:
  
  ### api/payment/TranactionDetails?transactionId=<transactionID>
	
  HTTP GET
  
  localhost:5000/api/payment/TransactionDetails?transactionId=sample
  
  Retrieves the redacted card information from a previous transaction of the store (store ID is obtained from the authentication claims).
  
  Requires a previous transaction to have used the SaveCredentials. As otherwise the card info is not saved.
  
  A sample transaction was added to the mock repository, so use the "?transactionId=sample" to access it
  
  
  ### localhost:5000/api/payment/RequestPayment
  
  HTTP POST
 
  Requests the gateway to process a payment.
  Requires a PaymentRequest model encoded as application/json in the body.
  
  In my solution, I assumed that there may be several banks. We can find out the correct bank by querying the IBankService instances.
  
  ### json PaymentRequest sample:
  
  {
          "ammount": 1,
          "currency": "usd",
          "cardInfo": {
	          "cardNumber": "1234-1234-1234-1234",
	          "expirationDate": "2020-01-01T00:00:00",
	          "holderFirstName": "John",
	          "holderLastName": "Doe",
	          "ccv": 213
	        },
          "saveCredentials": true
   }
  
  ### Sample result:
  
  {
    "isSuccesful": true,
    "errorInformation": "",
    "transactionIdentifier": "ojitsiffotipopuhhunxgdjqstgglftnvlakpjlk"
  }
