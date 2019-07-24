using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Checkout.Security
{
    /// <summary>
    /// Mock authentication class
    /// </summary>
    public class MockAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string Scheme = "TestAuthentication";

        /// <summary>
        /// Claims to be obtained by the mock 'user'
        /// </summary>
        public virtual ClaimsIdentity Identity { get; } = new ClaimsIdentity(new Claim[]
           {
                new Claim("scope", "checkout.gateway.process"),
                new Claim("scope", "checkout.gateway.retrieve"),
                new Claim("client_id", "store11111111"),
           }, "test");

        public MockAuthenticationOptions()
        {

        }

        /// <summary>
        /// Mock validation.
        /// </summary>
        public override void Validate()
        {
            base.Validate();
        }
    }

    /// <summary>
    /// Authentication handler 
    /// </summary>
    internal class MockAuthenticationHandler : AuthenticationHandler<MockAuthenticationOptions>
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticationTicket = new AuthenticationTicket(
                                             new ClaimsPrincipal(Options.Identity),
                                             new AuthenticationProperties(),
                                             this.Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }

        public MockAuthenticationHandler(IOptionsMonitor<MockAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            // Nothing to do!
        }
    }

    /// <summary>
    /// Extension class for adding the mock authentication
    /// </summary>
    public static class MockAuthenticationExtensions
    {
        public static AuthenticationBuilder AddMockAuthentication(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<MockAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<MockAuthenticationOptions, MockAuthenticationHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
