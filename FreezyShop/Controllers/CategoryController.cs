using FreezyShop.Data;
using FreezyShop.Data.Entities;
using FreezyShop.Helpers;
using FreezyShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FreezyShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(IUserHelper userHelper, ICategoryRepository categoryRepository)
        {
            _userHelper = userHelper;
            _categoryRepository = categoryRepository;
        }
        [Authorize(Roles = "Employee")]
        public IActionResult Index()
        {
            CategoryViewModel model = new CategoryViewModel
            {
                Categories = _categoryRepository.GetAll().OrderByDescending(P => P.Id).ToList(),
            };
            return View(model);
        }
      
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create(Category category)
        {
            //if (ModelState.IsValid)
            //{
                category.CreatedOn = System.DateTime.Today;
                await _categoryRepository.CreateAsync(category);
                return RedirectToAction(nameof(Index));
            //}
          
        }

      
        [Authorize(Roles = "Employee")]
       
        public async Task<IActionResult> Delete(int id)
        {
            var disciplina = await _categoryRepository.GetByIdAsync(id);
            await _categoryRepository.DeleteAsync(disciplina);

            return RedirectToAction(nameof(Index));
        }

    }
}
