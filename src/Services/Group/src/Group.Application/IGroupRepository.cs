using Group.Domain.DTOs;
using Group.Domain.Models;

namespace Group.Application;

public interface IGroupRepository
{
    IEnumerable<Member> GetMembers(Guid groupId);
    bool CheckMember(string authorization, Guid groupId);
    Task<CreateGroupResponseDto> Create(string authorization, CreateGroupRequestDto request);
}