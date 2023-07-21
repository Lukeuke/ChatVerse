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

    public async Task<(bool, object)> AddUserToGroup(string authorization, Guid userId, Guid groupId)
    {
        var ownerId = ParseTokenIntoUserId(authorization);

        var group = _context.Groups.FirstOrDefault(x => x.Id == groupId);

        if (group == null || group.OwnerId != ownerId) return (false, new {Message = "Only group owners can add users."});
        
        _context.Members.Add(new Member
        {
            GroupId = groupId,
            MemberId = userId
        });

        await _context.SaveChangesAsync();

        return (true, new { Message = "User was added to the group." });
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