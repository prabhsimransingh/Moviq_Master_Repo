
namespace Moviq.Api
{
    using Moviq.Helpers;
    using Moviq.Interfaces.Services;
    using Nancy;
    using RestSharp;
    using System.Configuration;

    public class PaymentModule : NancyModule
    {
        public PaymentModule(IModuleHelpers helper)
        {
            this.Get["/api/payment", true] = async (args, cancellationToken) =>
            {
                var token = this.Request.Query.q;
                var totalcost = this.Request.Query.totalcost;
                var email=this.Request.Query.email;
                const string baseUrl = "https://api.stripe.com/";
                const string endPoint = "v1/charges";
                var apiKey = ConfigurationManager.AppSettings["apikey"]; ;

                var client = new RestClient(baseUrl) { Authenticator = new HttpBasicAuthenticator(apiKey, "") };
                var request = new RestRequest(endPoint, Method.POST);

                request.AddParameter("card", token);
                request.AddParameter("amount", totalcost);
                request.AddParameter("currency", "usd");
           

                var response = client.Execute(request);

                return helper.ToJson(response);


            };
        }
    }
}