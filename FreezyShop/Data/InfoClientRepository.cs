using FreezyShop.Data.Entities;
using SuperShop.Data;

namespace FreezyShop.Data
{
    public class InfoClientRepository : GenericRepository<InfoClient>, IInfoClientRepository
    {
        public InfoClientRepository(DataContext context) : base(context)
        {

        }
    }
}
