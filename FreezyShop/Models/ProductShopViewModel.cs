using FreezyShop.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FreezyShop.Models
{
    public class ProductShopViewModel : Product
    {
        public Product Product { get; set; }
        public List<ProductCategory> RelatedProducts { get; set; }

        public string[] SizesOfProducts { get; set; }
    }
}
