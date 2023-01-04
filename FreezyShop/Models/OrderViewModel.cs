using FreezyShop.Data.Entities;
using System.Collections.Generic;

namespace FreezyShop.Models
{
    public class OrderViewModel : Order
    {
        

        
        public IEnumerable<CartItem> CartItems { get; set; }


        public decimal TotalOrder { get; set; }
    }
}
