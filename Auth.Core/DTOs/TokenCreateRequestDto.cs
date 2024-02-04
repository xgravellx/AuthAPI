namespace Auth.Core.DTOs;

public class TokenCreateRequestDto
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}