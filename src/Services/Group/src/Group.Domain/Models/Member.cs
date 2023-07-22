using System.ComponentModel.DataAnnotations;

namespace Group.Domain.Models;

public class Member
{
    [Key]
    public Guid Id { get; set; }
    
    public List<Group> Groups { get; set; } = new();
}