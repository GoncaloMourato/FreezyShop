using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreezyShop.Data.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public User User { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Name { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }

        public string Size { get; set; }

        public string Gender { get; set; }
        public string Color { get; set; }
        [NotMapped]
        public bool InFavourite { get; set; }
        [Display(Name = "Image Front")]
        public Guid ImageUrl1 { get; set; }
        [Display(Name = "Image Back")]
        public Guid ImageUrl2 { get; set; }

        public int Accessed { get; set; }
        
        public int Ordered { get; set; }

      
        public string ImageFullPath1 => ImageUrl1 == Guid.Empty ? $"https://freezyshopstorage2.blob.core.windows.net/noimage/noroupas.png"
:       $"https://freezyshopstorage2.blob.core.windows.net/products/{ImageUrl1}";
        public string ImageFullPath2 => ImageUrl2 == Guid.Empty ? $"https://freezyshopstorage2.blob.core.windows.net/noimage/noroupas.png"
:       $"https://freezyshopstorage2.blob.core.windows.net/products/{ImageUrl2}";

        

    }
}
