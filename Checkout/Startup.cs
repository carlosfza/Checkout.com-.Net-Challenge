using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Repositories;
using Checkout.Security;
using Checkout.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Checkout
{
    public class Startup
    {
        /// <summary>
        /// Hosting environment
        /// </summary>
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Creating the serilog logger
            //In this example I will use dependency injection, though a static logger may also be used
            //In the Web.config I will put only the basics
            //TODO: Add sinks to Serilog so that we can monitor this service remotelly
            Serilog.ILogger Logger = new LoggerConfiguration()
              .ReadFrom.AppSettings()
              .CreateLogger();

            services.AddSingleton<Serilog.ILogger>(Logger);
            ConfigureDiServices(services);

            //Adding development authentication options
            if (_env.EnvironmentName == "Development")
            {
                services.AddAuthentication(o =>
                {
                    o.DefaultScheme = "TestAuthorizationScheme";
                }).AddMockAuthentication("TestAuthorizationScheme", "Development authentication profile with all needed claims!", o => { });
            }
            //Production authentication options
            else
            {
                services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    //TODO: add logic to configure production Identity Provider on production
                    options.Authority = "https://localhost:12345";
                    options.RequireHttpsMetadata = true;

                    options.ApiName = "checkout_gateway";
                });
            }
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequreProcessScope", policy => policy.RequireClaim("scope", new List<string>() { "checkout.gateway.process"}));
                options.AddPolicy("RequireRetrieveScope", policy => policy.RequireClaim("scope", new List<string>() { "checkout.gateway.retrieve" }));
            });
        }

        private void ConfigureDiServices(IServiceCollection services)
        {
            var mockBank1 = new MockBankService1();
            Dictionary<string, IBankService> dictBankServices = new Dictionary<string, IBankService>();
            dictBankServices.Add("mockBank1Id", mockBank1);
            services.AddSingleton<Dictionary<string, IBankService>>(dictBankServices);
            services.AddSingleton<ITransactionRepository>(new MockTransactionRepository());
            services.AddSingleton<IPaymentProcessor, PaymentProcessor>();
            services.AddSingleton<ITransactionInformationService, TransactionInformationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            
            //We are adding OpenID authentication here. Using IdentityServer4 protected resouce library
            //If the server is in development mode, we use a mock user profile
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
