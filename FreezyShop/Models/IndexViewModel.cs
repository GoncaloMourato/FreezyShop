using FreezyShop.Data.Entities;
using System.Collections.Generic;

namespace FreezyShop.Models
{
    public class IndexViewModel
    {
        public IList<Product> Products { get; set; }
        public IList <ProductCategory> Categories { get; set; }

 

    }

}
