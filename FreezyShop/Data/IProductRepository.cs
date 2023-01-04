using FreezyShop.Data.Entities;
using FreezyShop.Models;
using SuperShop.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreezyShop.Data
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IQueryable<ProductViewModel> ProductsToProductViewModel(IQueryable<Product> products);

        public  Task<IEnumerable<Product>> GetAllProductsWithFilters(ShopViewModel model);
        //users? api 
        public IQueryable GetAllWithUsers();
    }
}
