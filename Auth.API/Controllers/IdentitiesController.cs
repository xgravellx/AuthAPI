using Auth.Core.DTOs;
using Auth.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentitiesController(IIdentityService identityService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateRequestDto request)
        {
            var response = await identityService.CreateUser(request);

            if (response.AnyError)
            {
                return BadRequest(response);
            }

            return Created("", response);
        }
    }
}
