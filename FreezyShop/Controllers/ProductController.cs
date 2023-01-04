using FreezyShop.Data;
using FreezyShop.Data.Entities;
using FreezyShop.Helpers;
using FreezyShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;

namespace FreezyShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly IFlashMessage _flashMessage;

        public ProductController(IUserHelper userHelper, IBlobHelper blobHelper, IProductRepository productRepository, 
            ICategoryRepository categoryRepository, IProductCategoryRepository productCategoryRepository,IProductSizeRepository productSizeRepository ,IFlashMessage flashMessage)
        {
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productCategoryRepository = productCategoryRepository;
            _productSizeRepository = productSizeRepository;
            _flashMessage = flashMessage;
        }

       
        [Authorize(Roles="Employee")]
        public IActionResult Index()
        {
            var model = Enumerable.Empty<ProductViewModel>();



            var categories = _productRepository.GetAll();
            if (categories.Any())
            {
                model = (_productRepository.ProductsToProductViewModel(categories)).OrderByDescending(p => p.Id);
            }
            else
            {
                _flashMessage.Warning("Products Not Found");
                 
            }





            return View(model);


        }


        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                {
                    return RedirectToAction("Index", "Products");
                }

                var product = await _productRepository.GetByIdAsync(id.Value);
                if (product == null)
                {
                   return RedirectToAction("Index", "Products");
                }

                return View(product);
        }


        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {
            var model = new ProductViewModel
            {

              Categories = _categoryRepository.GetListCategories()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId1 = Guid.Empty;
                Guid imageId2 = Guid.Empty;
                if (model.ImageFile1 != null && model.ImageFile1.Length > 0 && model.ImageFile2 != null && model.ImageFile2.Length > 0)
                {

                    imageId1 = await _blobHelper.UploadBlobAsync(model.ImageFile1, "products");
                    imageId2 = await _blobHelper.UploadBlobAsync(model.ImageFile2, "products");
                }
                var product = new Product
                { 
                    ImageUrl1 = imageId1,
                    ImageUrl2 = imageId2,
                    Name = model.Name,
                    Price = model.Price,
                    Quantity = model.Quantity,
                    Description = model.Description,
                    Color = model.Color,
                    Gender=model.Gender,
                    User = model.User
                };
                model.Categories = _categoryRepository.GetListCategories();
                List<SelectListItem> selectListItems = model.Categories.Where(p => model.CategoryIds.Contains(int.Parse(p.Value))).ToList();
                
                product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _productRepository.CreateAsync(product);

                foreach (var selectListItem in selectListItems)
                {
                    selectListItem.Selected = true;
                    ViewBag.message += "\\n" + selectListItem.Text;


                    if (selectListItem.Selected == true)
                    {
                        var productcategory= new ProductCategory
                        {
                      
                            CategoryId = int.Parse(selectListItem.Value),
                            ProductId = product.Id
                        };
                        try
                        {
                            await _productCategoryRepository.CreateAsync(productcategory);
                           
                        }
                        catch
                        {
                            _flashMessage.Warning("Ups! Something went wrong!");
                        }
                        


                    }

                    
                }
                foreach (var item in model.Sizes)
                {
                    var productsizes = new ProductSizes
                    {
                        ProductId = product.Id,
                        Size = item
                    };
                    try
                    {
                        await _productSizeRepository.CreateAsync(productsizes);
                        _flashMessage.Info("Product created!");
                    }
                    catch
                    {
                        _flashMessage.Warning("Ups! Something went wrong!");
                    }
                }
                return RedirectToAction("Index");
            }
            model.Categories = _categoryRepository.GetListCategories();
            return View(model);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Products");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }


            var model = new ProductViewModel
            {
                Id = product.Id,
                
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Color= product.Color,
                Gender= product.Gender,
                Sizes = await _productSizeRepository.GetAll().Where(p=>p.ProductId == product.Id).Select(p=>p.Size).ToArrayAsync(),
                User = product.User,
                ImageUrl1 = product.ImageUrl1,
                ImageUrl2 = product.ImageUrl2,
                Categories = _categoryRepository.GetListCategories(),
                Imageurl1 = product.ImageFullPath1,
                Imageurl2 = product.ImageFullPath2,
                
                CategoryIds = await _productCategoryRepository.GetAll().Where(p=>p.ProductId == product.Id).Select(p=>p.CategoryId).ToArrayAsync()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId1 = Guid.Empty;
                    Guid imageId2 = Guid.Empty;
                    if (model.ImageFile1 != null && model.ImageFile1.Length > 0 && model.ImageFile2 != null && model.ImageFile2.Length > 0)
                    {

                        imageId1 = await _blobHelper.UploadBlobAsync(model.ImageFile1, "products");
                        imageId2 = await _blobHelper.UploadBlobAsync(model.ImageFile2, "products");
                    }
                    else if(model.ImageFile1 != null && model.ImageFile1.Length > 0 && model.ImageFile2 == null )
                    {
                        imageId1 = await _blobHelper.UploadBlobAsync(model.ImageFile1, "products");
                        imageId2 = model.ImageUrl2;
                    }
                    else if (model.ImageFile2 != null && model.ImageFile2.Length > 0 && model.ImageFile1 == null )
                    {
                        imageId1 = model.ImageUrl1; 
                        imageId2 = await _blobHelper.UploadBlobAsync(model.ImageFile2, "products");
                    }
                    else
                    {
                        imageId1 = model.ImageUrl1;
                        imageId2 = model.ImageUrl2;
                    }

                    var product = new Product
                    {
                        Id = model.Id,
                        ImageUrl1 = imageId1,
                        ImageUrl2 = imageId2,
                        Description = model.Description,
                        Color = model.Color,
                        Gender=model.Gender,
                        Name = model.Name,
                        Price = model.Price,
                        Quantity = model.Quantity,
                        User = model.User


                    };



                    //TODO: Modificar para o user que tiver logado
                    product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _productRepository.UpdateAsync(product);
                    model.Categories = _categoryRepository.GetListCategories();
                    List<SelectListItem> selectListItems = model.Categories.Where(p => model.CategoryIds.Contains(int.Parse(p.Value))).ToList();
                    var categoriesofproduct = await _productCategoryRepository.GetAll().Where(p => p.ProductId == product.Id).ToListAsync();
                    foreach (var item in categoriesofproduct)
                    {
                        await _productCategoryRepository.DeleteAsync(item);
                    }

                    foreach (var selectListItem in selectListItems)
                    {
                        selectListItem.Selected = true;
                        ViewBag.message += "\\n" + selectListItem.Text;


                        if (selectListItem.Selected == true)
                        {



                            var productcategory = new ProductCategory
                            {

                                CategoryId = int.Parse(selectListItem.Value),
                                ProductId = product.Id
                            };
                            try
                            {
                             
                                await _productCategoryRepository.CreateAsync(productcategory);
                            }
                            catch
                            {
                                _flashMessage.Warning("Ups! Something went wrong!");
                            }
                     


                        }


                    }
                    var sizeofproduct = await _productSizeRepository.GetAll().Where(p => p.ProductId == product.Id).ToListAsync();
                    foreach(var item in sizeofproduct)
                    {
                       await _productSizeRepository.DeleteAsync(item);
                    }
                    foreach (var item in model.Sizes)
                    {
                        
                        var productsizes = new ProductSizes
                        {
                            ProductId = product.Id,
                            Size = item
                        };
                        try
                        {
                            await _productSizeRepository.CreateAsync(productsizes);
                            _flashMessage.Info("Product Updated!");
                        }
                        catch
                        {
                            _flashMessage.Warning("Ups! Something went wrong!");
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            try
            {
                //throw new Exception("Excepção de Teste");
                await _productRepository.DeleteAsync(product);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {

                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{product.Name} provavelmente está a ser usado!!";
                    ViewBag.ErrorMessage = $"{product.Name} não pode ser apagado visto haverem encomendas que o usam.</br></br>" +
                        $"Experimente primeiro apagar todas as encomendas que o estão a usar," +
                        $"e torne novamente a apagá-lo";
                }

                return View("Error");
            }
          
        }
        [Authorize(Roles = "Employee")]
        public IActionResult StockProducts()
        {
            var model = Enumerable.Empty<ProductViewModel>();



            var categories = _productRepository.GetAll().OrderByDescending(p=>p.Id);
            if (categories.Any())
            {
                model = (_productRepository.ProductsToProductViewModel(categories)).OrderByDescending(p => p.Id);
            }
            else
            {
                ViewBag.message = "Products Not Found";
            }
            return View(model);
        }
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AddStock(int id)
        {
            
            Product product = await _productRepository.GetByIdAsync(id);
            try
            {
                product.Quantity += 1;
                await _productRepository.UpdateAsync(product);
            }
            catch
            {
                //flashnotif
            }
              
            

            return RedirectToAction("StockProducts");
        }
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DecreaseStock(int id)
        {

            Product product = await _productRepository.GetByIdAsync(id);
            if (product.Quantity == 0)
            {
                //flashnotif - > The quantitu of a product cant be -0
            }
            try
            {
                product.Quantity -= 1;
                await _productRepository.UpdateAsync(product);
            }
            catch
            {
                //flashnotif
            }



            return RedirectToAction("StockProducts");
        }

        public async Task<IActionResult> DeleteStock(int id)
        {

            Product product = await _productRepository.GetByIdAsync(id);
           
                product.Quantity = 0;
                await _productRepository.UpdateAsync(product);
          


            return RedirectToAction("StockProducts");
        }
    }
}
