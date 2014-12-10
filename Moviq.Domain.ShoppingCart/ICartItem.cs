using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Interfaces.Models
{
    public interface ICartItem
    {
        Guid Guid { get; set; }
        string CartId { get; set; }
        string Uid { get; set; }
        int SrNo { get; set; }
        string Title { get; set; }
        decimal Price { get; set; }
        bool AddedFlag { get; set; }
    }
}
