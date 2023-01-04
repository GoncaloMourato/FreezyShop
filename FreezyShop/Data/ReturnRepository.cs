using FreezyShop.Data.Entities;
using SuperShop.Data;

namespace FreezyShop.Data
{
    public class ReturnRepository : GenericRepository<Return>, IReturnRepository
    {
        public ReturnRepository(DataContext context) : base(context)
        {

        }
    }
}
