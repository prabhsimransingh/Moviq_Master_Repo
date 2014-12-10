using Moviq.Interfaces.Factories;
using Moviq.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Products
{
   public class CartFactory : IFactory<ICartItem>
    {
        public ICartItem GetInstance()
        {
            return new CartItem() as ICartItem;
        }
    }
}
