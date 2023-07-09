namespace Identity.Domain.DTOs;

public class LoginUserRequestDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}