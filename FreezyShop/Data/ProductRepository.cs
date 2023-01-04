using FreezyShop.Data.Entities;
using FreezyShop.Models;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreezyShop.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext _context;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductRepository(DataContext context, ICategoryRepository categoryRepository , IProductCategoryRepository productCategoryRepository) : base(context)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsWithFilters(ShopViewModel model)
        {
            List<Product> list = new List<Product>();
            if (model.Category != null)
            {
                if ( model.Color != null )
                {
                    var test = await _context.ProductsCategories.Include(p => p.Product).Include(p => p.Category).Where(p => p.Category.Name == model.Category && p.Product.Color == model.Color).Select(p => p.Product).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }

                }
                if(model.Gender != null){
                    var test = await _context.ProductsCategories.Include(p => p.Product).Include(p => p.Category).Where(p => p.Category.Name == model.Category && p.Product.Gender == model.Gender).Select(p => p.Product).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }

                }

                if (model.Price != 0)
                {

                    var test = await _context.ProductsCategories.Include(p=>p.Product).Where(p => p.Category.Name == model.Category && p.Product.Price <= model.Price).Select(p => p.Product).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }
                }

                //if(model.Size != null)
                //{
                //    var size = await _context.ProductSizes.Include(p => p.Product).Where(p => p.Size == model.Size).Select(p => p.Product).ToListAsync();
                //    var categoria = await _context.ProductsCategories.Include(p => p.Product).Include(P => P.Category).Where(p => p.Category.Name == model.Category).Select(p => p.Product).ToListAsync();
                //    foreach (var itemsize in size)
                //    {
                //        foreach (var itemcategoria in categoria)
                //        {
                           
                //        }
                //    }
                //}
                if (model.Color == null && model.Gender == null && model.Price == 0)
                {
                    var categoria = await _context.ProductsCategories.Include(p => p.Product).Include(P => P.Category).Where(p => p.Category.Name == model.Category).Select(p => p.Product).ToListAsync();
                    foreach (var item1 in categoria)
                    {
                        list.Add(item1);
                    }
                }
            }
            else if (model.Size != null)
            {
                if (model.Color != null)
                {
                    var test = await _context.ProductSizes.Include(p => p.Product).Where(p => p.Size == model.Size && p.Product.Color == model.Color).Select(p => p.Product).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }

                }
                if (model.Gender != null)
                {
                    var test = await _context.ProductSizes.Include(p => p.Product).Where(p => p.Size == model.Size && p.Product.Gender == model.Gender).Select(p => p.Product).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }

                }

                if (model.Price != 1)
                {

                    var test = await _context.ProductSizes.Include(p => p.Product).Where(p => p.Size == model.Size && p.Product.Price <= model.Price).Select(p => p.Product).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }
                }

                if (model.Color == null && model.Gender == null && model.Price == 1)
                {
                    var test = await _context.ProductSizes.Include(p => p.Product).Where(p => p.Size == model.Size).Select(p => p.Product).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }
                }
            }else if (model.Color != null)
            {
                if (model.Category != null)
                {
                    var test = await _context.ProductsCategories.Include(p => p.Product).Include(p => p.Category).Where(p => p.Category.Name == model.Category && p.Product.Color == model.Color).Select(p => p.Product).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }

                }
                if (model.Gender != null)
                {
                    var test = await _context.Products.Where(p => p.Color == model.Color && p.Gender == model.Gender).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }

                }

                if (model.Price != 1)
                {

                    var test = await _context.Products.Where(p => p.Color == model.Color && p.Price <= model.Price).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }
                }

                if (model.Category == null && model.Gender == null && model.Price == 1)
                {
                    var test = await _context.Products.Where(p => p.Color == model.Color).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }
                }


            
            }else if (model.Gender != null)
            {
                if (model.Category != null)
                {
                    var test = await _context.ProductsCategories.Include(p => p.Product).Include(p => p.Category).Where(p => p.Category.Name == model.Category && p.Product.Gender == model.Gender).Select(p => p.Product).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }

                }
                if (model.Color != null)
                {
                    var test = await _context.Products.Where(p => p.Color == model.Color && p.Gender == model.Gender).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }

                }

                if (model.Price != 1)
                {

                    var test = await _context.Products.Where(p => p.Gender == model.Gender && p.Price <= model.Price).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }
                }

                if (model.Category == null && model.Color == null && model.Price == 1)
                {
                    var test = await _context.Products.Where(p => p.Gender == model.Gender).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }
                }

               
            }else if (model.Userprefselected != null)
            {


                foreach (var item in model.UserPreferences)
                {
                    var test = await _context.ProductsCategories.Include(p => p.Product).Include(p => p.Category).Where(p => p.Category.Name == item).Select(p => p.Product).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }

                }
                return list;
            }else if (model.Price != 0)
            {
                if (model.Category != null)
                {
                    var test = await _context.ProductsCategories.Include(p => p.Product).Include(p => p.Category).Where(p => p.Category.Name == model.Category && p.Product.Price <= model.Price).Select(p => p.Product).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }

                }
                if (model.Color != null)
                {
                    var test = await _context.Products.Where(p => p.Color == model.Color && p.Price <= model.Price).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }

                }

                if (model.Gender != null)
                {

                    var test = await _context.Products.Where(p => p.Gender == model.Gender && p.Price <= model.Price).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }
                }

                if (model.Category == null && model.Color == null && model.Gender == null)
                {
                    var test = await _context.Products.Where(p => p.Price <= model.Price).ToListAsync();
                    foreach (var item1 in test)
                    {
                        list.Add(item1);
                    }
                }
            
            }else if (model.Category == null && model.Size == null && model.Color == null && model.Gender == null && model.Userprefselected == null && model.Price == 0)
            {
                var test = await _context.Products.ToListAsync();
                foreach (var item1 in test)
                {
                    list.Add(item1);
                }
            }

            return list;

            //List<Product> list = new List<Product>();
            //if (model.Category != null)
            //{

            //    if(model.Color != null)
            //    {
            //        var test = await _context.ProductsCategories.Include(p => p.Product).Include(p => p.Category).Where(p => p.Category.Name == model.Category && p.Product.Color == model.Color).Select(p => p.Product).ToListAsync();
            //        foreach (var item1 in test)
            //        {
            //            list.Add(item1);
            //        }
            //    }
            //    if (model.Gender!= null)
            //    {
            //        var test = await _context.ProductsCategories.Include(p => p.Product).Include(p => p.Category).Where(p => p.Category.Name == model.Category && p.Product.Gender == model.Gender).Select(p => p.Product).ToListAsync();
            //        foreach (var item1 in test)
            //        {
            //            list.Add(item1);
            //        }
            //    }
            //    if (model.Price != 1)
            //    {
            //        var test = await _context.ProductsCategories.Include(p => p.Product).Include(p => p.Category).Where(p => p.Category.Name == model.Category && p.Product.Price == model.Price).Select(p => p.Product).ToListAsync();
            //        foreach (var item1 in test)
            //        {
            //            list.Add(item1);
            //        }
            //    }

            //if (model.Size != null)
            //{

            //}

        }
           

        public IQueryable GetAllWithUsers()
        {
            return _context.Products.Include(p => p.User);
        }

        public IQueryable<ProductViewModel> ProductsToProductViewModel(IQueryable<Product> products)
        {


            return products.Select(product => new ProductViewModel
            {
                Id = product.Id,

                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                User = product.User,
                Gender = product.Gender,
                Color = product.Color,
                Categories = _categoryRepository.GetListCategories(),
                Imageurl1 = product.ImageFullPath1,
                Imageurl2 = product.ImageFullPath2,
                CategoriesOfTheProduct = _context.ProductsCategories.Include(p => p.Category).Where(p=> p.ProductId == product.Id).Select
                (p => new Category
                {
                    Id = p.CategoryId,
                    CreatedOn = p.Category.CreatedOn,
                    Name = p.Category.Name
                }).ToList(),
                SizesOfTheProduct = _context.ProductSizes.Where(p=>p.ProductId == product.Id).ToList()
            }); 
        }

    }
}
