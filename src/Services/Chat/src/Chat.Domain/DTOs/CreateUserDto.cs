namespace Chat.Domain.DTOs;

public class CreateUserDto
{
    public string Name { get; set; } = null!;
    public Guid GroupId { get; set; }
}