using FreezyShop.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuperShop.Data;
using System.Collections.Generic;

namespace FreezyShop.Data
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public List<SelectListItem> GetListCategories();
    }
}
