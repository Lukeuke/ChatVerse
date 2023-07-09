using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Models;

public class Group
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}