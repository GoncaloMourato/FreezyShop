using System.ComponentModel.DataAnnotations;

namespace FreezyShop.Data.Entities
{
    public class OrderDetail : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string UserId { get; set; }
        public User User { get; set; }


        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }


        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        public int Quantity { get; set; }


        public decimal Value => Price * (decimal)Quantity;

        public string Size { get; set; }
    }
}
