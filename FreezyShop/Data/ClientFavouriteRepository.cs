using FreezyShop.Data.Entities;
using SuperShop.Data;

namespace FreezyShop.Data
{
    public class ClientFavouriteRepository : GenericRepository<ClientFavourite>, IClientFavouriteRepository
    {
        public ClientFavouriteRepository(DataContext context) : base(context)
        {

        }
    }
}
