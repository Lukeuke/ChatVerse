using System.IdentityModel.Tokens.Jwt;
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

    public bool CheckMember(string authorization, Guid groupId)
    {
        var token = authorization.Split(" ")[1];
        
        var jsonToken = new JwtSecurityToken(token);
        
        var (key, value) = jsonToken.Payload.First(x => x.Key == "id");

        var userId = Guid.Parse(value.ToString()!);

        return _context.Members.Where(x => x.MemberId == userId).Select(x => x.GroupId).Contains(groupId);
    }
}