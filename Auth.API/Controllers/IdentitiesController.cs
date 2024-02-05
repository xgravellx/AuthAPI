using Auth.Core.DTOs;
using Auth.Core.Interfaces;
using Auth.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentitiesController(IIdentityService identityService, ITokenService tokenService) : ControllerBase
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

        [HttpPost("create-token")]
        public async Task<IActionResult> CreateToken(TokenCreateRequestDto request)
        {
            var response = await tokenService.Create(request);
            if (response.AnyError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("assign-to-user")]
        public async Task<IActionResult> AssignToUser(RoleCreateRequestDto request)
        {
            var response = await identityService.CreateRole(request);
            if (response.AnyError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

    }
}
