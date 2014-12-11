using Moviq.Interfaces;
using Moviq.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Products
{
   public class CartItemServices : ICartItemServices
    {
        public CartItemServices(ICartRepository cartRepo) 
        {
            this.cartRepo = cartRepo;
        }

        ICartRepository cartRepo;

       public ICartItem AddCartItem(ICartItem item)
        {
            return cartRepo.Set(item);
        }
        public ICartItem UpdateCartItem(ICartItem item)
        {
           return cartRepo.Remove(item);
       }

       public ICartItem EmptyCart(string userId)
        {
            return cartRepo.EmptyCart(userId);
        }

       public string GetCartItems()
       {
           if (AmbientContext.CurrentClaimsPrinciple != null)
           {
               ICustomClaimsIdentity currentUser = AmbientContext.CurrentClaimsPrinciple.ClaimsIdentity;
               string userId = currentUser.GetAttribute(AmbientContext.UserPrincipalGuidAttributeKey).ToString();
               var result = cartRepo.GetByUserId(userId);
               if (result != null)
                   return result.Uid;
               else
                   return null;
           }
           return null;
       }
    }
}
