namespace Chat.Domain.DTOs;

public class ReadStatusDto
{
    public Guid MessageId { get; set; }
    public Guid GroupId { get; set; }
}