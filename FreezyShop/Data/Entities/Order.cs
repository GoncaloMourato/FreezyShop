using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FreezyShop.Data.Entities
{
    public class Order : IEntity
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime OrderDate { get; set; }


        [Display(Name = "Delivery date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime DeliveryDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Lines => Items == null ? 0 : Items.Count();


        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity => Items == null ? 0 : Items.Sum(i => i.Quantity);



        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value => Items == null ? 0 : Items.Sum(i => i.Price) + ShippingTaxes - Discount;

        public decimal Discount { get; set; }
        public decimal ShippingTaxes { get; set; }
        
        public string UserId { get; set; }
        
        public User User { get; set; }

       public string FullName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string District { get; set; }
   
        public string ZipCode { get; set; }

        public string NameOnCard { get; set; }

        public string CardNumber { get; set; }

        public string ExpiryDate { get; set; }

       
        public int CVV { get; set; }

        public IEnumerable<OrderDetail> Items { get; set; }
    


        public string OrderStatus { get; set; }

        public string ShippingMethod { get; set; }
        public string DeleveryStatus { get; set; }

    }
}
