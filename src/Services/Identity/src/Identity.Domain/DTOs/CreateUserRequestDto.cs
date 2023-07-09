namespace Identity.Domain.DTOs;

public class CreateUserRequestDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}