using System.ComponentModel.DataAnnotations;

namespace FreezyShop.Data.Entities
{
    public class ProductCategory :IEntity
    {
        public int Id { get; set; }

        [Display(Name="Product")]
        [Required(ErrorMessage = "{0} is required")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "{0} is required")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
