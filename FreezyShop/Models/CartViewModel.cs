using FreezyShop.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreezyShop.Models
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal shipptaxes {get;set;}

        public decimal PricewithTaxes { get { return TotalPrice +  shipptaxes; } }

        public string Shipping { get; set; }

       
        public decimal Discount { get { return PricewithTaxes * Percentagem / 100; } }
        
        public decimal FinalPrice { get { return PricewithTaxes - Discount; } }
        public string Code { get; set; }
        public int Percentagem { get; set; }
        
    }
}
