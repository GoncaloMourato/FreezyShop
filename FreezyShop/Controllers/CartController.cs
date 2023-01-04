using FreezyShop.Data;
using FreezyShop.Data.Entities;
using FreezyShop.Helpers;
using FreezyShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreezyShop.Controllers
{
    public class CartController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPromoCodeRepository _promoCodeRepository;

        public CartController(IUserHelper  userHelper, ICartRepository cartRepository, IProductRepository productRepository, IPromoCodeRepository promoCodeRepository)
        {
            _userHelper = userHelper;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _promoCodeRepository = promoCodeRepository;
        }
        public async Task<IActionResult> Index(string shipping, int percentage)
        {
            var user =await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            List<CartItem> cartItems = new List<CartItem>();
            cartItems = _cartRepository.GetAll().Include(p=>p.Product).Where(p => p.UserId == user.Id).ToList();

            CartViewModel model= new CartViewModel
            {
                CartItems = cartItems,
                TotalPrice = cartItems.Sum(x => x.Quantity * x.Price)
            };
            if (shipping != null)
            {
                model.Shipping = shipping;
                if (shipping == "CttExpress") {
                    model.shipptaxes = 4.89M;
                }
                else
                {
                    model.shipptaxes = 2.75M;
                }
            }

            if(percentage != 0)
            {
                model.Percentagem = percentage;
            }
            return View(model);
        }
        public async Task<IActionResult> AddCupom(string shipping,string Code)
        {
            
            var cupomaccept = await _promoCodeRepository.GetAll().Where(p => p.Code == Code).FirstOrDefaultAsync();
            if(cupomaccept != null)
            {
                ViewBag.Message = "Coupom added!";
                return RedirectToAction("Index", new { shipping = shipping, percentage = cupomaccept.Percentagem });
               
            }
            else {
                ViewBag.Message = "That coupom doesn't exist";
                return RedirectToAction("Index", new { shipping = shipping }); 
            
            }
            
          

           
          
        }
        public async Task<IActionResult> AddToCart(Product products)
        {
           
            if(this.User.Identity.Name == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            Product product = await _productRepository.GetByIdAsync(products.Id);
            List<CartItem> cartItems = new List<CartItem>();
            cartItems = _cartRepository.GetAll().Where(p => p.UserId == user.Id).ToList();

            CartItem cartItem = cartItems.Where(x => x.ProductId == products.Id).FirstOrDefault();

            if(cartItem == null)
            {
                var newCartItem = new  CartItem
                {
                    ProductId = product.Id,
                    ImageUrl1 = product.ImageFullPath1,
                    Price = product.Price,
                    Size = products.Size,
                    Quantity = 1,
                    UserId = user.Id,
                    User = user
                };
                await _cartRepository.CreateAsync(newCartItem);
                cartItems.Add(newCartItem);
            }
            else
            {
                if(cartItem.Quantity == product.Quantity)
                {
                    ViewBag.Message1 = "No stock";
                }
                else
                {
                    cartItem.Quantity += 1;
                    cartItem.Size = products.Size;
                    await _cartRepository.UpdateAsync(cartItem);
                }
               
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Decrease(int id)
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            
            List<CartItem> cartItems =  _cartRepository.GetAll().Where(p => p.UserId == user.Id).ToList();

            CartItem cartItem = cartItems.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity >1)
            {
                --cartItem.Quantity;
                await _cartRepository.UpdateAsync(cartItem);
                
            }
            else
            {
                cartItems.RemoveAll(p => p.ProductId == id);
                await _cartRepository.DeleteAsync(cartItem);
            }

       

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            List<CartItem> cartItems = _cartRepository.GetAll().Where(p => p.UserId == user.Id).ToList();
            CartItem cartItem = cartItems.Where(x => x.ProductId == id).FirstOrDefault();

            cartItems.RemoveAll(p => p.ProductId == id);
                await _cartRepository.DeleteAsync(cartItem);
            



            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Clear(int id)
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            List<CartItem> cartItems = _cartRepository.GetAll().Where(p => p.UserId == user.Id).ToList();

            foreach(var item in cartItems)
            {
                await _cartRepository.DeleteAsync(item);
            }


            return RedirectToAction("Index");
        }
    }
}
