using System.ComponentModel.DataAnnotations;

namespace Group.Domain.Models;

public class Member
{
    [Key]
    public int Id { get; set; }
    
    public Guid MemberId { get; set; }
    public Guid GroupId { get; set; }
}