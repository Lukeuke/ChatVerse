using Group.Domain.DTOs;
using Group.Domain.Models;

namespace Group.Application;

public interface IGroupRepository
{
    IEnumerable<Domain.Models.Group> GetAll();
    IEnumerable<Member> GetMembers(Guid groupId);
    bool CheckMember(string authorization, Guid groupId);
    Task<CreateGroupResponseDto> Create(string authorization, CreateGroupRequestDto request);
    Task<(bool, object)> AddUserToGroup(string authorization, Guid userId, Guid groupId);
}