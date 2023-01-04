using System;
using System.ComponentModel.DataAnnotations;

namespace FreezyShop.Data.Entities
{
    public class Return : IEntity
    {
        public int Id { get; set; }
        public string Reason { get; set; }

        public string Description{ get; set; }

        [Display(Name = "Image Of Damage")]
        public Guid ImageUrl { get; set; }

        public string ImageFullPath1 => ImageUrl == Guid.Empty ? $"https://freezyshopstorage2.blob.core.windows.net/noimage/noroupas.png"
:       $"https://freezyshopstorage2.blob.core.windows.net/returns/{ImageUrl}";

        public string Status { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int OrderDetailId { get; set; }
        public OrderDetail OrderDetail { get; set; }


        
    }
}
