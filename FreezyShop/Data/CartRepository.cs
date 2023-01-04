using FreezyShop.Data.Entities;
using SuperShop.Data;

namespace FreezyShop.Data
{
    public class CartRepository : GenericRepository<CartItem>, ICartRepository
    {
        public CartRepository(DataContext context) : base(context)
        {

        }
    }
}
