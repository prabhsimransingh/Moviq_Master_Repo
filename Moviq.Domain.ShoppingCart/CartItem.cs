using Moviq.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Products
{
   public class CartItem : ICartItem, IHelpCategorizeNoSqlData
    {
        public CartItem() 
        {
            this._type = "cartItem";
        }

        public Guid Guid { get;set; }
        public string Uid { get; set; }
       public int SrNo { get; set; }
       public string CartId { get; set; }
       public string _type { get; set; }
       public string Title { get; set; }
       public decimal Price { get; set; }
       public bool AddedFlag { get; set; }
    }
}
