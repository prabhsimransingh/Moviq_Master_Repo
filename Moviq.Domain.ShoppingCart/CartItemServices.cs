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
           var result = cartRepo.GetByGuid(item.Guid);
           result.AddedFlag = true;
           return cartRepo.Set(item);
       }

       public bool EmptyCart(string userId)
        {
           var result = cartRepo.GetByUserId(userId);
           foreach(CartItem record in result)
           {
            record.AddedFlag = true;
            cartRepo.Set(record);
           }
            return true;
        }
    }
}
