using FreezyShop.Data;
using FreezyShop.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FreezyShop.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : Controller
    {
        private DataContext _dataContext;
        public UserApiController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_dataContext.Users.ToList());
        }
    }
}
