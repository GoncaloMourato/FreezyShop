using System.ComponentModel.DataAnnotations;

namespace FreezyShop.Data.Entities
{
    public class ProductSizes : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Product")]
        [Required(ErrorMessage = "{0} is required")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        public string Size { get; set; }
        
    }
}
