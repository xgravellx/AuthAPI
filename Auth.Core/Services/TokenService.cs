using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Auth.Core.DTOs;
using Auth.Models.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Core.Services;

public class TokenService(IConfiguration configuration)
{


    public async Task<ResponseDto<TokenCreateResponseDto>> Create(TokenCreateRequestDto request)
    {
        var signatureKey = configuration.GetSection("TokenOptions")["Key"]!;
        var tokenExpireAsHour = configuration.GetSection("TokenOptions")["TokenExpire"]!;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKey));

        var signingCredentials =
            new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddHours(Convert.ToDouble(tokenExpireAsHour)),
            signingCredentials: signingCredentials
        );

    }
}