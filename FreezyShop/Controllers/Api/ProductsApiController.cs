using Microsoft.AspNetCore.Mvc;
using FreezyShop.Data;

namespace FreezyShop.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsApiController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_productRepository.GetAllWithUsers());
        }
    }
}
