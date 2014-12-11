namespace Moviq.Api
{
    using Cart;
    using Moviq.Domain.Products;
    using Moviq.Helpers;
    using Moviq.Interfaces;
    using Moviq.Interfaces.Services;
    using Nancy;

    public class SearchModule : NancyModule
    {
        public SearchModule(IProductDomain products, IModuleHelpers helper, ICartItemServices cartItemServices) 
        {
            this.Get["/api/search", true] = async (args, cancellationToken) =>
            {
                var searchTerm = this.Request.Query.q;
                var result = await products.Repo.Find(searchTerm);
                return helper.ToJson(result);
            };
            this.Get["/api/addToCart", true] = async (args, cancellationToken) =>
            {

                var searchTerm = this.Request.Query.q;
                var product = products.Repo.Get(searchTerm);
                if (AmbientContext.CurrentClaimsPrinciple != null)
                {
                    var cart =
                    new Cart.ShoppingCart(
                        new Cart.AmountCalculator(),
                        new PaymentHandler(), new EmptyState(), cartItemServices);
                    
                    cart.Add(product);
                }
                return null;
            };   
        }
    }
}