using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Core.DTOs;
using Auth.Core.Interfaces;
using Auth.Models.Entities;
using Auth.Models.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Auth.Core.Services;

public class TokenService(IConfiguration configuration, UserManager<AppUser> userManager) : ITokenService
{


    public async Task<ResponseDto<TokenCreateResponseDto>> Create(TokenCreateRequestDto request)
    {

        var hasUser = await userManager.FindByNameAsync(request.UserName);

        if (hasUser is null)
        {
            return ResponseDto<TokenCreateResponseDto>.Fail("Username or password is wrong");
        }

        var checkPassword = await userManager.CheckPasswordAsync(hasUser!, request.Password);

        if (checkPassword == false)
        {
            return ResponseDto<TokenCreateResponseDto>.Fail("Username or password is wrong");
        }

        var claimList = new List<Claim>();

        var userIdAsClaim = new Claim(ClaimTypes.NameIdentifier, hasUser.Id.ToString());
        var userNameAsClaim = new Claim(ClaimTypes.Name, hasUser.UserName!);
        var idAsClaim = new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());

        claimList.Add(userIdAsClaim);
        claimList.Add(userNameAsClaim);
        claimList.Add(idAsClaim);

        foreach (var role in await userManager.GetRolesAsync(hasUser))
        {
            claimList.Add(new Claim(ClaimTypes.Role, role));
        }


        var signatureKey = configuration.GetSection("TokenOptions")["Key"]!;
        var tokenExpireAsHour = configuration.GetSection("TokenOptions")["TokenExpire"]!;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKey));

        var signingCredentials =
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(Convert.ToDouble(tokenExpireAsHour)),
            signingCredentials: signingCredentials,
            claims: claimList
        );

        var responseDto = new TokenCreateResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };

        return ResponseDto<TokenCreateResponseDto>.Success(responseDto);

    }
}