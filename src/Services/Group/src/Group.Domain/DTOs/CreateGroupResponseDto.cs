namespace Group.Domain.DTOs;

public class CreateGroupResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid Owner { get; set; }
}