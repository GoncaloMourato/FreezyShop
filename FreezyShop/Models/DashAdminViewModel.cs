using FreezyShop.Data.Entities;
using System.Collections.Generic;

namespace FreezyShop.Models
{
    public class DashAdminViewModel
    {
        public Product ProductMoreSold { get; set; }
        public Product ProductMoreAcessed { get; set; }
        public IEnumerable<UserWithRole> Users { get; set; }

        public int AllUsers { get; set; }

        public int AllProducts { get; set; }

        public int AllOrders { get; set; }

    }
}
