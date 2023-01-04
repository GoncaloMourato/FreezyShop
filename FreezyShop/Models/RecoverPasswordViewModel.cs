using System.ComponentModel.DataAnnotations;

namespace FreezyShop.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
