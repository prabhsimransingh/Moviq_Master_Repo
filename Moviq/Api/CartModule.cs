namespace Moviq.Api
{
    using Moviq.Helpers;
    using Moviq.Interfaces.Services;
    using Nancy;

    public class CartModule : NancyModule
    {
        public CartModule(IProductDomain products, IModuleHelpers helper)
        {

            this.Get["/api/cart/{uid}"] = args =>
            {
                return helper.ToJson(products.Repo.Get(args.uid));
            };
        }
    }
}