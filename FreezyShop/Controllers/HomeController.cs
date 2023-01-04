using FreezyShop.Data;
using FreezyShop.Data.Entities;
using FreezyShop.Helpers;
using FreezyShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;

namespace FreezyShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserHelper _userHelper;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly IClientFavouriteRepository _clientFavouriteRepository;
        private readonly IClientPreferencesRepository _clientPreferencesRepository;
        private readonly IFlashMessage _flashMessage;
        private readonly IMailHelper _mailHelper;

        public HomeController(ILogger<HomeController> logger, IUserHelper userHelper , IProductRepository productRepository,ICategoryRepository categoryRepository,IProductCategoryRepository productCategoryRepository, IProductSizeRepository productSizeRepository, IClientFavouriteRepository clientFavouriteRepository, IClientPreferencesRepository clientPreferencesRepository, IFlashMessage flashMessage, IMailHelper mailHelper)
        {
            _logger = logger;
            _userHelper = userHelper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _productCategoryRepository = productCategoryRepository;
            _productSizeRepository = productSizeRepository;
            _clientFavouriteRepository = clientFavouriteRepository;
            _clientPreferencesRepository = clientPreferencesRepository;
            _flashMessage = flashMessage;
            _mailHelper = mailHelper;
        }
        public async Task<IActionResult> Index()
        {
            //var categoriesnorepeat = _productCategoryRepository.GetAll().Include(p => p.Product).Include(p => p.Category).Select(p=>p.Category).ToList().OrderBy(p=>p.Name.Distinct());
           
            IndexViewModel model = new IndexViewModel
            {
                Products = _productRepository.GetAll().OrderByDescending(p => p.Id).ToList(),
                Categories = _productCategoryRepository.GetAll().Include(p=>p.Product).Include(p=>p.Category).OrderBy(p=>p.Id).ToList()
            };

            
           

            int i = 0;
           
            if (this.User.Identity.IsAuthenticated)
                 {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                foreach (var productfav in model.Products)
                {

                    var alreadyinfavourite = await _clientFavouriteRepository.GetAll().Where(p => p.ProductId == productfav.Id && p.UserId == user.Id).FirstOrDefaultAsync();
                    if (alreadyinfavourite != null)
                    {
                        productfav.InFavourite = true;
                    }
                    else
                    {
                        productfav.InFavourite = false;
                    }
                    i++;
                }
            }
       
            return View(model);
        }

        public async Task<IActionResult> Shop(ShopViewModel model, int page =1)
        {
           
            if (model != null)
            {
                model.PropertiesPerPage =8;
                model.CurrentPage = page;
                if (this.User.Identity.IsAuthenticated )
                {
                    var user =await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                    var userpreferences = _clientPreferencesRepository.GetAll().Where(p=>p.UserId == user.Id).Select(p=>p.Preferences).ToList();
                    if(userpreferences.Count() > 0)
                    {
                        model.Userhaspref =true;
                        
                    }
                    if(userpreferences.Count() > 0 && model.Userprefselected != null)
                    {
                        model.UserPreferences = userpreferences.ToArray();
                    }

                    _flashMessage.Danger("Hello");

                }
                    model.Categories = _categoryRepository.GetAll().ToList();
                    model.Products = await _productRepository.GetAllProductsWithFilters(model);
            }
            else
            {
                
            }
            if (this.User.Identity.IsAuthenticated)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                foreach (var productfav in model.Products)
                {

                    var alreadyinfavourite = await _clientFavouriteRepository.GetAll().Where(p => p.ProductId == productfav.Id && p.UserId == user.Id).FirstOrDefaultAsync();
                    if (alreadyinfavourite != null)
                    {
                        productfav.InFavourite = true;
                    }
                    else
                    {
                        productfav.InFavourite = false;
                    }

                }

            }
            _flashMessage.Danger("Hello");
            
            return View(model);
        }

        public async Task<IActionResult> Product(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            var categoria = await _productCategoryRepository.GetAll().Where(p => p.ProductId == product.Id).Select(p => p.CategoryId).FirstOrDefaultAsync();
            ProductShopViewModel model = new ProductShopViewModel
            {
                Product = product ,
                RelatedProducts = _productCategoryRepository.GetAll().Include(p=>p.Product).Where(p=>p.CategoryId == categoria && p.Product.Id != product.Id).ToList(),
                SizesOfProducts = await  _productSizeRepository.GetAll().Where(p=>p.ProductId ==product.Id).Select(p=>p.Size).ToArrayAsync()
        };
            if (this.User.Identity.IsAuthenticated)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                var alreadyinfavourite = await _clientFavouriteRepository.GetAll().Where(p => p.ProductId == product.Id && p.UserId == user.Id).FirstOrDefaultAsync();
                if (alreadyinfavourite !=null)
                {
                    model.Product.InFavourite = true;

                }
                

            }
            product.Accessed += 1;
            await _productRepository.UpdateAsync(product);
            return View(model );
        }

        public async Task<IActionResult> Favourites()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            FavouritesViewModel model = new FavouritesViewModel
            {
                Favourites = _clientFavouriteRepository.GetAll().Include(p => p.Product).Where(p => p.UserId == user.Id)
            };

            
            return View(model);
        }
        public async Task<IActionResult> AddToFavourites(int? id)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var alreadyinfavourite = await _clientFavouriteRepository.GetAll().Where(p => p.ProductId == id && p.UserId == user.Id).FirstOrDefaultAsync();
            var newproductfavourite = new ClientFavourite
            {
                ProductId = id.Value,
                UserId = user.Id
            };
            if(alreadyinfavourite == null)
            {
                try
                {
                    await _clientFavouriteRepository.CreateAsync(newproductfavourite);
                    

                }
                catch
                {

                }
            }
            else
            {
                //flassmessage
            }
       
   
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> RemoveToFavourites(int id)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var favourite = await _clientFavouriteRepository.GetAll().Where(p=>p.ProductId == id).FirstOrDefaultAsync();
            

            await _clientFavouriteRepository.DeleteAsync(favourite);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddToFavourites2(int id,string category)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var alreadyinfavourite = await _clientFavouriteRepository.GetAll().Where(p => p.ProductId == id && p.UserId == user.Id).FirstOrDefaultAsync();
            var newproductfavourite = new ClientFavourite
            {
                ProductId = id,
                UserId = user.Id
            };
            if (alreadyinfavourite == null)
            {
                try
                {
                    await _clientFavouriteRepository.CreateAsync(newproductfavourite);

                }
                catch
                {

                }
            }
            else
            {
                //flassmessage
            }


            return RedirectToAction("Shop", new {category =category});
        }
        public async Task<IActionResult> RemoveToFavourites2(int id, string category)
        {

            var favourite = await _clientFavouriteRepository.GetAll().Where(p => p.ProductId == id).FirstOrDefaultAsync();


            await _clientFavouriteRepository.DeleteAsync(favourite);
            return RedirectToAction("Shop", new {category= category});
        }
        public async Task<IActionResult> AddToFavourites3(int id, string category)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var alreadyinfavourite = await _clientFavouriteRepository.GetAll().Where(p => p.ProductId == id && p.UserId == user.Id).FirstOrDefaultAsync();
            var newproductfavourite = new ClientFavourite
            {
                ProductId = id,
                UserId = user.Id
            };
            if (alreadyinfavourite == null)
            {
                try
                {
                    await _clientFavouriteRepository.CreateAsync(newproductfavourite);

                }
                catch
                {

                }
            }
            else
            {
                //flassmessage
            }


            return RedirectToAction("Shop", new { category = category });
        }
        public async Task<IActionResult> RemoveToFavourites3(int id, string category)
        {

            var favourite = await _clientFavouriteRepository.GetAll().Where(p => p.ProductId == id).FirstOrDefaultAsync();


            await _clientFavouriteRepository.DeleteAsync(favourite);
            return RedirectToAction("Shop", new { category = category });
        }
        public async Task<IActionResult> RemoveToFavourites4(int id, string category)
        {

            var favourite = await _clientFavouriteRepository.GetAll().Where(p => p.ProductId == id).FirstOrDefaultAsync();


            await _clientFavouriteRepository.DeleteAsync(favourite);
            return RedirectToAction("Favourites");
        }
        public IActionResult About(AboutViewModel model)
        {
            if(model.Email!= null)
            {
                var emailbody = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office' lang='en'><head><meta name=x-apple-disable-message-reformatting><meta http-equiv=X-UA-Compatible><meta charset=utf-8><meta name=viewport content=target-densitydpi=device-dpi><meta content=true name=HandheldFriendly><meta content=width=device-width name=viewport><style type='text/css'>table {border-collapse: separate;table-layout: fixed;mso-table-lspace: 0pt;mso-table-rspace: 0pt}table td {border-collapse: collapse}.ExternalClass {width: 100%}.ExternalClass,.ExternalClass p,.ExternalClass span,.ExternalClass font,.ExternalClass td,.ExternalClass div {line-height: 100%}* {line-height: inherit;text-size-adjust: 100%;-ms-text-size-adjust: 100%;-moz-text-size-adjust: 100%;-o-text-size-adjust: 100%;-webkit-text-size-adjust: 100%;-webkit-font-smoothing: antialiased;-moz-osx-font-smoothing: grayscale}html {-webkit-text-size-adjust: none !important}img+div {display: none;display: none !important}img {Margin: 0;padding: 0;-ms-interpolation-mode: bicubic}h1, h2, h3, p, a {font-family: inherit;font-weight: inherit;font-size: inherit;line-height: 1;color: inherit;background: none;overflow-wrap: normal;white-space: normal;word-break: break-word}a {color: inherit;text-decoration: none}h1, h2, h3, p {min-width: 100%!important;width: 100%!important;max-width: 100%!important;display: inline-block!important;border: 0;padding: 0;margin: 0}a[x-apple-data-detectors] {color: inherit !important;text-decoration: none !important;font-size: inherit !important;font-family: inherit !important;font-weight: inherit !important;line-height: inherit !important}a[href^='mailto'],a[href^='tel'],a[href^='sms'] {color: inherit !important;text-decoration: none !important}@media only screen and (min-width: 481px) {.hd { display: none!important }}@media only screen and (max-width: 480px) {.hm { display: none!important }}[style*='Inter'] {font-family: 'Inter', BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif !important;} [style*='Fira Sans'] {font-family: 'Fira Sans', BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif !important;} [style*='Inter Tight'] {font-family: 'Inter Tight', BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif !important;}@media only screen and (min-width: 481px) {.t19,.t21{padding-top:60px!important;padding-bottom:60px!important}.t25{width:56px!important}.t32{mso-line-height-alt:30px!important;line-height:30px!important}.t41{line-height:34px!important;font-size:28px!important}.t42{mso-line-height-alt:30px!important;line-height:30px!important}.t51{line-height:28px!important;font-size:18px!important}.t52{mso-line-height-alt:30px!important;line-height:30px!important}.t61{line-height:28px!important;font-size:18px!important}.t62{mso-line-height-alt:35px!important;line-height:35px!important}.t68{padding:30px 40px!important}.t70{padding:30px 40px!important;width:734px!important}.t149{mso-line-height-alt:30px!important;line-height:30px!important}.t155{padding:48px 50px!important}.t157{padding:48px 50px!important;width:500px!important}.t161,.t166{padding-bottom:44px!important}}</style>" +
                $"<link href='https://fonts.googleapis.com/css2?family=Fira+Sans:wght@400;700&family=Inter+Tight:wght@700&family=Inter:wght@400&display=swap' rel='stylesheet' type='text/css'></head><body class=t0 style='min-width:100%;Margin:0px;padding:0px;background-color:#ffffff;'><div class=t1 style='background-color:#ffffff;'><table role=presentation width=100% cellpadding=0 cellspacing=0 border=0 align=center><tr><td class=t208 style='font-size:0;line-height:0;mso-line-height-rule:exactly;' valign=top align=center><table role=presentation width=100% cellpadding=0 cellspacing=0 border=0 align=center><tr><td><div class=t12 style='display:inline-table;width:100%;text-align:center;vertical-align:top;'><div class=t16 style='display:inline-table;text-align:initial;vertical-align:inherit;width:100%;max-width:642px;'><table role=presentation width=100% cellpadding=0 cellspacing=0 class=t18><tr><td class=t19 style='overflow:hidden;background-color:unset;padding:50px 20px 50px 20px;'><table role=presentation width=100% cellpadding=0 cellspacing=0><tr><td><table class=t24 role=presentation cellpadding=0 cellspacing=0 align=center><tr><td class=t25 style='background-color:unset;background-repeat:repeat;background-size:auto;background-position:center center;border:unset;overflow:hidden;width:30px;'><div style='font-size:0px;'><img class=t31 style='display:block;border:0;height:auto;width:100%;Margin:0;max-width:100%;' width=56 src=https://uploads.tabular.email/e/bbf947db-4fa5-4ca2-b078-a5e611b74344/3595853a-22f3-4960-9347-21ce4bbea68d.png /></div></td></tr></table></td></tr><tr><td><div class=t32 style='mso-line-height-rule:exactly;mso-line-height-alt:20px;line-height:20px;font-size:1px;display:block;'>&nbsp;</div></td></tr><tr><td><table class=t34 role=presentation cellpadding=0 cellspacing=0 align=center><tr><td class=t35 style='overflow:hidden;width:440px;'><h1 class=t41 style='font-family:BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif, Fira Sans;line-height:28px;font-weight:700;font-style:normal;font-size:22px;text-decoration:none;text-transform:none;direction:ltr;color:#000000;text-align:center;mso-line-height-rule:exactly;mso-text-raise:2px;'>Thank you for reaching out!</h1></td></tr></table></td></tr><tr><td><div class=t52 style='mso-line-height-rule:exactly;mso-line-height-alt:20px;line-height:20px;font-size:1px;display:block;'>&nbsp;</div></td></tr><tr><td><table class=t54 role=presentation cellpadding=0 cellspacing=0 align=center><tr><td class=t55 style='overflow:hidden;width:608px;'><p class=t61 style='font-family:BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif, Fira Sans;line-height:25px;font-weight:400;font-style:normal;font-size:16px;text-decoration:none;text-transform:none;direction:ltr;color:#121212;text-align:left;mso-line-height-rule:exactly;mso-text-raise:3px;'>We received your attempt to reaching us about:</p></td></tr></table></td></tr><tr><td><div class=t62 style='mso-line-height-rule:exactly;mso-line-height-alt:25px;line-height:25px;font-size:1px;display:block;'>&nbsp;</div></td></tr><tr><td><table class=t69 role=presentation cellpadding=0 cellspacing=0 align=center><tr><td class=t70 style='background-color:#FFFFFF;overflow:hidden;width:754px;padding:20px 30px 20px 30px;border-radius:8px 8px 8px 8px;'><table role=presentation width=100% cellpadding=0 cellspacing=0><tr><td><table class=t73 role=presentation cellpadding=0 cellspacing=0 align=center><tr><td class=t74 style='overflow:hidden;width:800px;'><div class=t80 style='display:inline-table;width:100%;text-align:left;vertical-align:top;'><div class=t84 style='display:inline-table;text-align:initial;vertical-align:inherit;width:100%;max-width:592px;'><table role=presentation width=100% cellpadding=0 cellspacing=0 class=t86><tr><td class=t87 style='overflow:hidden;padding:0 0 0 15px;'><table role=presentation width=100% cellpadding=0 cellspacing=0><tr><td><table class=t92 role=presentation cellpadding=0 cellspacing=0 align=center><tr><td class=t93 style='overflow:hidden;width:600px;'><p class=t99 style='font-family:BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif, Inter Tight;line-height:22px;font-weight:700;font-style:normal;font-size:18px;text-decoration:none;text-transform:none;direction:ltr;color:#333333;text-align:left;mso-line-height-rule:exactly;mso-text-raise:1px;'>Reason</p></td></tr></table></td></tr><tr><td><div class=t100 style='mso-line-height-rule:exactly;mso-line-height-alt:5px;line-height:5px;font-size:1px;display:block;'>&nbsp;</div></td></tr><tr><td><table class=t102 role=presentation cellpadding=0 cellspacing=0 align=center><tr><td class=t103 style='overflow:hidden;width:600px;'><p class=t109 style='font-family:BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif, Inter;line-height:22px;font-weight:400;font-style:normal;font-size:16px;text-decoration:none;text-transform:none;direction:ltr;color:#333333;text-align:left;mso-line-height-rule:exactly;mso-text-raise:2px;'>{model.Reason}</p></td></tr></table></td></tr></table></td></tr></table></div></div></td></tr></table></td></tr><tr><td><div class=t110 style='mso-line-height-rule:exactly;mso-line-height-alt:15px;line-height:15px;font-size:1px;display:block;'>&nbsp;</div></td></tr><tr><td><table class=t112 role=presentation cellpadding=0 cellspacing=0 align=center><tr><td class=t113 style='overflow:hidden;width:800px;'><div class=t119 style='display:inline-table;width:100%;text-align:left;vertical-align:top;'><div class=t123 style='display:inline-table;text-align:initial;vertical-align:inherit;width:100%;max-width:500px;'><table role=presentation width=100% cellpadding=0 cellspacing=0 class=t125><tr><td class=t126 style='overflow:hidden;padding:0 0 0 15px;'><table role=presentation width=100% cellpadding=0 cellspacing=0><tr><td><table class=t131 role=presentation cellpadding=0 cellspacing=0 align=center><tr><td class=t132 style='overflow:hidden;width:600px;'><p class=t138 style='font-family:BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif, Inter Tight;line-height:22px;font-weight:700;font-style:normal;font-size:18px;text-decoration:none;text-transform:none;direction:ltr;color:#333333;text-align:left;mso-line-height-rule:exactly;mso-text-raise:1px;'>Description</p></td></tr></table></td></tr><tr><td><div class=t139 style='mso-line-height-rule:exactly;mso-line-height-alt:5px;line-height:5px;font-size:1px;display:block;'>&nbsp;</div></td></tr><tr><td>" +
                $"<table class=t141 role=presentation cellpadding=0 cellspacing=0 align=center><tr><td class=t142 style='overflow:hidden;width:600px;'><p class=t148 style='font-family:BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif, Inter;line-height:22px;font-weight:400;font-style:normal;font-size:16px;text-decoration:none;text-transform:none;direction:ltr;color:#333333;text-align:left;mso-line-height-rule:exactly;mso-text-raise:2px;'>{model.Description}</p></td></tr></table></td></tr></table></td></tr></table></div></div></td></tr></table></td></tr></table></td></tr></table></td></tr><tr><td><div class=t63 style='mso-line-height-rule:exactly;mso-line-height-alt:5px;line-height:5px;font-size:1px;display:block;'>&nbsp;</div></td></tr><tr><td><div class=t42 style='mso-line-height-rule:exactly;mso-line-height-alt:20px;line-height:20px;font-size:1px;display:block;'>&nbsp;</div></td></tr><tr><td><table class=t44 role=presentation cellpadding=0 cellspacing=0 align=center><tr><td class=t45 style='overflow:hidden;width:570px;'><p class=t51 style='font-family:BlinkMacSystemFont,Segoe UI,Roboto,Helvetica Neue,Arial,sans-serif, Fira Sans;line-height:25px;font-weight:400;font-style:normal;font-size:16px;text-decoration:none;text-transform:none;direction:ltr;color:#121212;text-align:left;mso-line-height-rule:exactly;mso-text-raise:3px;'>We wil contact you as soon as possible on the reply of this email!</p></td></tr></table></td></tr><tr><td><div class=t149 style='mso-line-height-rule:exactly;mso-line-height-alt:20px;line-height:20px;font-size:1px;display:block;'>&nbsp;</div></td></tr><tr><td><table class=t156 role=presentation cellpadding=0 cellspacing=0 align=center><tr><td class=t157 style='background-color:#242424;overflow:hidden;width:540px;padding:40px 30px 40px 30px;'><table role=presentation width=100% cellpadding=0 cellspacing=0><tr><td><table class=t160 role=presentation cellpadding=0 cellspacing=0 align=center><tr><td class=t161 style='overflow:hidden;width:800px;padding:10px 0 36px 0;'><div class=t167 style='display:inline-table;width:100%;text-align:center;vertical-align:top;'><div class=t173 style='display:inline-table;text-align:initial;vertical-align:inherit;width:25%;max-width:44px;'><div class=t174 style='padding:0 10px 0 10px;'><table role=presentation width=100% cellpadding=0 cellspacing=0 class=t175><tr><td class=t176 style='overflow:hidden;'><div style='font-size:0px;'><img class=t177 style='display:block;border:0;height:auto;width:100%;Margin:0;max-width:100%;' width=24 src=https://uploads.tabular.email/e/2feb9749-6369-44a9-90e9-1c26bf36c1a5/90e14628-2d8f-4c64-af7a-410b0a53d60c.png /></div></td></tr></table></div></div><div class=t183 style='display:inline-table;text-align:initial;vertical-align:inherit;width:25%;max-width:44px;'><div class=t184 style='padding:0 10px 0 10px;'><table role=presentation width=100% cellpadding=0 cellspacing=0 class=t185><tr><td class=t186 style='overflow:hidden;'><div style='font-size:0px;'><img class=t187 style='display:block;border:0;height:auto;width:100%;Margin:0;max-width:100%;' width=24 src=https://uploads.tabular.email/e/b158fd0c-1d9a-41bb-885b-099af24afa59/bbde14ea-031f-4dfe-bb34-39af4949882b.png /></div></td></tr></table></div></div><div class=t193 style='display:inline-table;text-align:initial;vertical-align:inherit;width:25%;max-width:44px;'><div class=t194 style='padding:0 10px 0 10px;'><table role=presentation width=100% cellpadding=0 cellspacing=0 class=t195><tr><td class=t196 style='overflow:hidden;'><div style='font-size:0px;'><img class=t197 style='display:block;border:0;height:auto;width:100%;Margin:0;max-width:100%;' width=24 src=https://uploads.tabular.email/e/b158fd0c-1d9a-41bb-885b-099af24afa59/b6f1e7ce-8c7b-41ee-b453-746aaf5e9b57.png /></div></td></tr></table></div></div><div class=t203 style='display:inline-table;text-align:initial;vertical-align:inherit;width:25%;max-width:44px;'><div class=t204 style='padding:0 10px 0 10px;'><table role=presentation width=100% cellpadding=0 cellspacing=0 class=t205><tr><td class=t206 style='overflow:hidden;'><div style='font-size:0px;'><img class=t207 style='display:block;border:0;height:auto;width:100%;Margin:0;max-width:100%;' width=24 src=https://uploads.tabular.email/e/b158fd0c-1d9a-41bb-885b-099af24afa59/8e37593e-8033-4bc9-9fee-951849506678.png /></div></td></tr></table></div></div></div></td></tr></table></td></tr></table></td></tr></table></td></tr></table></td></tr></table></div></div></td></tr></table></td></tr></table></div></body></html>" ;

                Response response = _mailHelper.SendEmail(model.Email, "Contact FreezyStaff", emailbody);
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
