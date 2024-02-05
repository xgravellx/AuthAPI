using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("all products");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post()
        {
            return Ok("save products");
        }

        [Authorize(Roles = "Manager")]
        [HttpPut]
        public IActionResult Update()
        {
            return Ok("update products");
        }
    }
}
