﻿using Moviq.Interfaces.Models;
using Moviq.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.Products
{
   public interface ICartRepository : IRepository<ICartItem>
    {
      // List<ICartItem> GetByUserid(string userid);
       ICartItem GetByGuid(Guid guid);
       ICartItem GetByUserId(string userId);
       ICartItem Remove(ICartItem item);
       ICartItem EmptyCart(string userId);
    }
}