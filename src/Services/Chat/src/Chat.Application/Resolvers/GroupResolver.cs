using Chat.Application.Services;
using Chat.Domain.Data;
using Chat.Domain.Models;
using HotChocolate.Resolvers;

namespace Chat.Application.Resolvers;

public class GroupResolver
{
    private readonly IGroupService _groupService;
    private readonly ChatDbContext _context;

    public GroupResolver(IGroupService groupService, ChatDbContext context)
    {
        _groupService = groupService;
        _context = context;
    }

    public Group GetGroup(Message message, IResolverContext context)
    {
        return _groupService.GetAll(_context).FirstOrDefault(x => x.Id == message.GroupId)!;
    }
}