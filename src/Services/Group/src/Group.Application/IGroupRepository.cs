using Group.Domain.Models;

namespace Group.Application;

public interface IGroupRepository
{
    IEnumerable<Member> GetMembers(Guid groupId);
}