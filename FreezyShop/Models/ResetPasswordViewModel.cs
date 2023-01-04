using System.ComponentModel.DataAnnotations;

namespace FreezyShop.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserName { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must have at least 6 characters!")]
        public string Password { get; set; }



        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Password and Confirm Password must be equal!")]
        public string ConfirmPassword { get; set; }


        [Required]
        public string Token { get; set; }
    }
}
