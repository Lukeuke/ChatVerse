namespace Chat.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public Guid GroupId { get; set; }
    public string Name { get; set; } = null!;
}