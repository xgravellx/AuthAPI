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

    public async Task<ResponseDto<string>> CreateRole(RoleCreateRequestDto request)
    {
        var appRole = new AppRole
            {
                Name = request.RoleName
            };


            var hasRole = await roleManager.RoleExistsAsync(appRole.Name);


            IdentityResult? roleCreateResult = null;
            if (!hasRole)
            {
                roleCreateResult = await roleManager.CreateAsync(appRole);
            }


            if (roleCreateResult is not null && !roleCreateResult.Succeeded)
            {
                var errorList = roleCreateResult.Errors.Select(x => x.Description).ToList();

                return ResponseDto<string>.Fail(errorList);
            }


            var hasUser = await userManager.FindByIdAsync(request.UserId);

            if (hasUser is null)
            {
                return ResponseDto<string>.Fail("kullanıcı bulunamadı.");
            }


            var roleAssignResult = await userManager.AddToRoleAsync(hasUser, appRole.Name);

            if (!roleAssignResult.Succeeded)
            {
                var errorList = roleAssignResult.Errors.Select(x => x.Description).ToList();

                return ResponseDto<string>.Fail(errorList);
            }

            return ResponseDto<string>.Success(string.Empty);
    }
}