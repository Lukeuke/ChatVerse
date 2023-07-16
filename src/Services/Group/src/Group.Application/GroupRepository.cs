using Group.Domain.Data;
using Group.Domain.Models;

namespace Group.Application;

public class GroupRepository : IGroupRepository
{
    private readonly GroupDbContext _context;

    public GroupRepository(GroupDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Member> GetMembers(Guid groupId)
    {
        return _context.Members.Where(x => x.GroupId == groupId);
    }
}