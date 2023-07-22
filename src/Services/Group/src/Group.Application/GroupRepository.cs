using System.IdentityModel.Tokens.Jwt;
using Group.Domain.Data;
using Group.Domain.DTOs;
using Group.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Group.Application;

public class GroupRepository : IGroupRepository
{
    private readonly GroupDbContext _context;

    public GroupRepository(GroupDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Domain.Models.Group> GetAll() => _context.Groups.AsEnumerable();
    public IEnumerable<Domain.Models.Group> GetAllForUser(string authorization)
    {
        return _context.Groups.Where(x => x.Members.Any(x => x.Id == ParseTokenIntoUserId(authorization)));
    }

    public IEnumerable<Member> GetMembers(Guid groupId)
    {
        return _context.Groups.Include(x => x.Members).First(x => x.Id == groupId).Members;
    }

    public bool CheckMember(string authorization, Guid groupId)
    {
        return _context.Groups.Include(x => x.Members)
            .First(x => x.Id == groupId)
            .Members
            .Any(x => x.Id == ParseTokenIntoUserId(authorization));
    }

    public async Task<CreateGroupResponseDto> Create(string authorization, CreateGroupRequestDto request)
    {
        var groupId = Guid.NewGuid();
        var ownerId = ParseTokenIntoUserId(authorization);
        var group = new Domain.Models.Group
        {
            Id = groupId,
            Name = request.Name,
            OwnerId = ownerId,
        };
        
        _context.Groups.Add(group);

        var member = _context.Members.FirstOrDefault(x => x.Id == ownerId);
        
        if (member is null)
        {
            member = new Member
            {
                Id = ownerId,
            };
            _context.Members.Add(member);
        }
        
        member.Groups.Add(group);

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

        var member = _context.Members.FirstOrDefault(x => x.Id == userId);

        if (member is null)
        {
            _context.Members.Add(new Member
            {
                Id = userId
            });
            
            await _context.SaveChangesAsync();
            member = _context.Members.First(x => x.Id == userId);
        }
        
        member!.Groups.Add(group);
        
        group.Members.Add(member);

        await _context.SaveChangesAsync();

        return (true, new { Message = "User was added to the group." });
    }

    public async Task<(bool, object)> DeleteUserFromGroup(string authorization, Guid userId, Guid groupId)
    {
        var ownerId = ParseTokenIntoUserId(authorization);
        
        var group = _context.Groups.FirstOrDefault(x => x.Id == groupId);

        if (group == null || group.OwnerId != ownerId) return (false, new {Message = "Only group owners can add users."});

        var member = _context.Members.Include(x => x.Groups).FirstOrDefault(x => x.Id == userId);

        if (member is null)
        {
            return (false, new { Message = "Member is not in the group." });
        }

        member.Groups.Remove(group);

        await _context.SaveChangesAsync();

        return (true, new { Messsage = "User was deleted from the group" });
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