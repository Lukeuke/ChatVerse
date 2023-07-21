namespace Chat.Domain.DTOs;

public class CreateMessageDto
{
    public string Content { get; set; } = null!;
    public Guid GroupId { get; set; }
}