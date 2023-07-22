using System.ComponentModel.DataAnnotations;

namespace Group.Domain.Models;

public class Group
{
    [Key]
    public Guid Id { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    public Guid OwnerId { get; set; }

    public List<Member> Members { get; set; } = new();
}