using FreezyShop.Data.Entities;
using SuperShop.Data;

namespace FreezyShop.Data
{
    public class SizeRepository : GenericRepository<Size>, ISizeRepository
    {
        public SizeRepository(DataContext context) : base(context)
        {

        }
    }
}
