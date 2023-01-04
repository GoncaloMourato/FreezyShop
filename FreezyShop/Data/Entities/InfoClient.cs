using System;
using System.ComponentModel.DataAnnotations;

namespace FreezyShop.Data.Entities
{
    public class InfoClient : IEntity
    {
        public int Id { get; set; }
        
        public string UserId { get; set; }
        public User User { get; set; }

        public Size Size { get; set; }

        public string FullName { get; set; }
        public string GenderClient { get; set; }
        

        public int PhoneNumber { get; set; }

        [Display(Name = "Profile Image")]
        public Guid ImageUrl { get; set; }

        public string ImageFullPath1 => ImageUrl == Guid.Empty ? $"https://freezyshopstorage2.blob.core.windows.net/noimage/noroupas.png"
:       $"https://freezyshopstorage2.blob.core.windows.net/users/{ImageUrl}";

    }
}
