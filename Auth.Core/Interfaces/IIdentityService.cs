using Auth.Core.DTOs;
using Auth.Models.Shared;

namespace Auth.Core.Interfaces;

public interface IIdentityService
{
    Task<ResponseDto<Guid?>> CreateUser(UserCreateRequestDto request);
    Task<ResponseDto<string>> CreateRole(RoleCreateRequestDto request);
}