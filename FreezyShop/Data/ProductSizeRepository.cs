using FreezyShop.Data.Entities;
using SuperShop.Data;

namespace FreezyShop.Data
{
    public class ProductSizeRepository : GenericRepository<ProductSizes>, IProductSizeRepository
    {
        public ProductSizeRepository(DataContext context) : base(context)
        {

        }
    }
}
