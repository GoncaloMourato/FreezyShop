using System.ComponentModel.DataAnnotations;

namespace FreezyShop.Data.Entities
{
    public class ShippingInfo : IEntity
    {

        [Required]
        public int Id { get; set; }

        public string District { get; set; }

        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string RecepientName { get; set; } //automatic user profile name

        public int PhoneNumber { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

    }
}
