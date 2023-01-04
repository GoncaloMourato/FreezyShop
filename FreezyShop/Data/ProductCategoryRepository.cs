using FreezyShop.Data.Entities;
using SuperShop.Data;

namespace FreezyShop.Data
{
    public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(DataContext context) : base(context)
        {

        }
    }
}
