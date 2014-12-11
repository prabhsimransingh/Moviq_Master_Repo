namespace Moviq.Api
{
    using Cart;
    using Moviq.Domain.Products;
    using Moviq.Helpers;
    using Moviq.Interfaces;
    using Moviq.Interfaces.Services;
    using Nancy;

    public class CartModule : NancyModule
    {
        public CartModule(IProductDomain products, IModuleHelpers helper, ICartItemServices cartItemService)
        {

            this.Get["/api/cart/{uid}"] = args =>
            {
                return helper.ToJson(products.Repo.Get(args.uid));
            };

            this.Get["/api/getcart/{uid}"] = args =>
            {
                string cartItems = cartItemService.GetCartItems();
                return cartItems;
            };
            this.Get["/api/removecart/{uid}"] = args =>
            {
                
                if (AmbientContext.CurrentClaimsPrinciple != null)
                {
                    var product = products.Repo.Get(args.uid);
                    var cart =
                    new Cart.ShoppingCart(
                        new Cart.AmountCalculator(),
                        new PaymentHandler(), new ActiveState(), cartItemService);
                    cart.Remove(product);
                    string cartItems = cartItemService.GetCartItems();
                    return cartItems;                    
                }
                return null;
            };
            this.Get["/api/emptycart/{uid}"] = args =>
           {
               if (AmbientContext.CurrentClaimsPrinciple != null)
               {
                   var product = products.Repo.Get(args.uid);
                   var cart =
                   new Cart.ShoppingCart(
                       new Cart.AmountCalculator(),
                       new PaymentHandler(), new ActiveState(), cartItemService);
                   cart.EmptyCart();
                   string cartItems = cartItemService.GetCartItems();
                   return cartItems;
               }
               return null;
           };
        }
    }
}