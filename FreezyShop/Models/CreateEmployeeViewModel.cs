using System.ComponentModel.DataAnnotations;

namespace FreezyShop.Models
{
    public class CreateEmployeeViewModel
    {
        [Required, StringLength(35)]
        public string FirstName { get; set; }
        [Required, StringLength(30)]
        public string LastName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public string NickName { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
