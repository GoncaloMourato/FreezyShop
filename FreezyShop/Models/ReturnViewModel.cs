using FreezyShop.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FreezyShop.Models
{
    public class ReturnViewModel : Return
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        public string AdicionalInfEmail { get; set; }

        public Product Product { get; set; }
    }
}
