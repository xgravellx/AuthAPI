using Auth.Core.DTOs;
using Auth.Core.Interfaces;
using Auth.Models.Entities;
using Auth.Models.Shared;
using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Services;

public class IdentityService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : IIdentityService
{
    public UserManager<AppUser> UserManager { get; set; } = userManager;
    public RoleManager<AppRole> RoleManager { get; set; } = roleManager;

    public async Task<ResponseDto<Guid?>> CreateUser(UserCreateRequestDto request)
    {
        var appUser = new AppUser
        {
            UserName = request.UserName,
            Email = request.Email,
        };

        // burada diğer methods kullanılabilir; deleteasync, updateas, changeEmail, changePhone
        var result = await userManager.CreateAsync(appUser, request.Password);

        if (!result.Succeeded)
        {
            var errorList = result.Errors.Select(x => x.Description).ToList();

            return ResponseDto<Guid?>.Fail(errorList);
        }

        return ResponseDto<Guid?>.Success(appUser.Id);

    }
}