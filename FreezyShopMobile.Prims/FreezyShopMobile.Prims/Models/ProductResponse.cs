using System;
using System.Collections.Generic;
using System.Text;

namespace FreezyShopMobile.Prims.Models
{
    public class ProductResponse
    {

        public int Id { get; set; }
        public UserResponse User { get; set; }

        public string Name { get; set; }
     
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Size { get; set; }

        public Guid ImageUrl1 { get; set; }

        public Guid ImageUrl2 { get; set; }
        public int Accessed { get; set; }
        public string ImageFullPath1 { get; set; }
        public string ImageFullPath2 { get; set; }
    }
}
