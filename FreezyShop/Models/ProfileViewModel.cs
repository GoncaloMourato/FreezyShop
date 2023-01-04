using FreezyShop.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreezyShop.Models
{
    public class ProfileViewModel : InfoClient
    {
        public string Imageurl1 { get; set; }
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        // shipping info 
        public string District { get; set; }
        
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string RecepientName { get; set; }

        //payment method
        public string NameOnCard { get; set; }

        public string CardNumber { get; set; }

        public string ExperyDate { get; set; }

        public int CVV { get; set; }

        public string[] Preferences { get; set; }
        public string[] ClientPreferences { get; set; }

    }
}
