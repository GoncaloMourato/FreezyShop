using FreezyShop.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuperShop.Data;
using System.Collections.Generic;
using System.Linq;

namespace FreezyShop.Data
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly DataContext _context;
        public  CategoryRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public List<SelectListItem> GetListCategories()
        {
            var list = _context.Categories.ToList();
            List<SelectListItem> lista = new List<SelectListItem>();
            foreach (var item in list)
            {
                lista.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()

                });
            }
            return lista;
        }
    }
}
