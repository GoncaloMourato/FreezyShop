using FreezyShop.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreezyShop.Models
{
    public class ProductViewModel : Product
    {

        public string Imageurl1 { get; set; }
        public string Imageurl2 { get; set; }
        [Display(Name = "Image Front")]
        public IFormFile ImageFile1 { get; set; }
        [Display(Name = "Image Back")]
        public IFormFile ImageFile2 { get; set; }

        public int CategoryId { get; set; }
        public int[] CategoryIds { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        public IEnumerable<Category> CategoriesOfTheProduct { get; set; }

        public string[] Sizes { get; set; }

        public IEnumerable<ProductSizes> SizesOfTheProduct { get; set; }
        public int NewStock { get; set; }
    }
}
