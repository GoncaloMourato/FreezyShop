using FreezyShop.Data.Entities;
using System.Collections.Generic;

namespace FreezyShop.Models
{
    public class CategoryViewModel : Category
    {
        public List<Category> Categories{ get; set; }
    }
}
