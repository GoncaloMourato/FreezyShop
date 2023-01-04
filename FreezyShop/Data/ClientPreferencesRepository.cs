using FreezyShop.Data.Entities;
using SuperShop.Data;

namespace FreezyShop.Data
{
    public class ClientPreferencesRepository : GenericRepository<ClientPreference>, IClientPreferencesRepository
    {
        public ClientPreferencesRepository(DataContext context) : base(context)
        {

        }
    }
}
