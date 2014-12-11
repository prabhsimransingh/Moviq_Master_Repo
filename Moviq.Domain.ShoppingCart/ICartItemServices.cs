
using Moviq.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Products
{
    public interface ICartItemServices
    {
        ICartItem AddCartItem(ICartItem item);
        ICartItem UpdateCartItem(ICartItem item);
        string GetCartItems();
        ICartItem EmptyCart(string userId);

    }
}
