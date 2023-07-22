namespace Group.Domain.Models;

public class GroupMember
{
    public Guid MemberId { get; set; }
    public Guid GroupId { get; set; }
    
    public Member Member { get; set; } = null!;
    public Group Group { get; set; } = null!;
}