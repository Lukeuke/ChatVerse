using System.IdentityModel.Tokens.Jwt;
using Group.Domain.Data;
using Group.Domain.DTOs;
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
        return _context.Members.Where(x => x.MemberId == ParseTokenIntoUserId(authorization)).Select(x => x.GroupId).Contains(groupId);
    }

    public async Task<CreateGroupResponseDto> Create(string authorization, CreateGroupRequestDto request)
    {
        var groupId = Guid.NewGuid();
        var ownerId = ParseTokenIntoUserId(authorization);
        
        _context.Groups.Add(new Domain.Models.Group
        {
            Id = groupId,
            Name = request.Name,
            OwnerId = ownerId
        });

        _context.Members.Add(new Member
        {
            GroupId = groupId,
            MemberId = ownerId
        });

        await _context.SaveChangesAsync();

        return new CreateGroupResponseDto
        {
            Id = groupId,
            Name = request.Name,
            Owner = ownerId
        };
    }

    private static Guid ParseTokenIntoUserId(string authorization)
    {
        var token = authorization.Split(" ")[1];
        
        var jsonToken = new JwtSecurityToken(token);
        
        var (key, value) = jsonToken.Payload.First(x => x.Key == "id");

        var userId = Guid.Parse(value.ToString()!);

        return userId;
    }
}