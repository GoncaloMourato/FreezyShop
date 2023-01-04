using FreezyShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreezyShop.Models
{
    public class ShopViewModel
    {
        public int PropertiesPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(Products.Count() / (double)PropertiesPerPage));
        }

        public IEnumerable<Product> PaginatedProperties()
        {
            int start = (CurrentPage - 1) * PropertiesPerPage;
            return Products.OrderBy(p => p.Id).Skip(start).Take(PropertiesPerPage);
        }



        public IEnumerable<Product> Products { get; set; }

        public List<Category> Categories { get; set; }
        public string Category { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string Gender { get; set; }
        public int Price { get; set; }
        public bool Userhaspref { get; set; }
        public string Userprefselected { get; set; }
        public string[] UserPreferences { get; set; }
        public int[] InFavourite { get; set; }

       
    }
}
