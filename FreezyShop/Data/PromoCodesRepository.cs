using FreezyShop.Data.Entities;
using SuperShop.Data;

namespace FreezyShop.Data
{
    public class PromoCodesRepository : GenericRepository<PromoCode>, IPromoCodeRepository
    {
        public PromoCodesRepository(DataContext context) : base(context)
        {

        }
    }
}
