using Chat.Domain.Data;
using Chat.Domain.Models;

namespace Chat.Application.Services;

public class GroupService : IGroupService
{
    private readonly ChatDbContext _context;

    public GroupService(ChatDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<Group> GetAll()
    {
        return _context.Groups.AsQueryable();
    }
}