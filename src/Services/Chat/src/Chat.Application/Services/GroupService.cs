using Chat.Domain.Data;
using Chat.Domain.Models;

namespace Chat.Application.Services;

public class GroupService : IGroupService
{
    public IQueryable<Group> GetAll(ChatDbContext context)
    {
        return context.Groups.AsQueryable();
    }
}