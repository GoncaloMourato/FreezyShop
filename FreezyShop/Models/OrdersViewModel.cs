using FreezyShop.Data.Entities;
using System.Collections.Generic;

namespace FreezyShop.Models
{
    public class OrdersViewModel : Order
    {
        public List<Order> Orders { get; set; }
    }
}
