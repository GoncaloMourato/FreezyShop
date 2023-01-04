using FreezyShop.Data.Entities;
using SuperShop.Data;

namespace FreezyShop.Data
{
    public class PaymentMethodRepository : GenericRepository<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(DataContext context) : base(context)
        {

        }
    }
}
