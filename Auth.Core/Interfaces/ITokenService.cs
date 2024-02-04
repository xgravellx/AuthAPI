using Auth.Core.DTOs;
using Auth.Models.Shared;

namespace Auth.Core.Interfaces;

public interface ITokenService
{
    Task<ResponseDto<TokenCreateResponseDto>> Create(TokenCreateRequestDto request);
}