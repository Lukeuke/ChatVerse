using Chat.Application.Services;
using Chat.Domain.Data;
using Chat.Domain.Models;
using HotChocolate.Resolvers;

namespace Chat.Application.Resolvers;

public class GroupResolver
{
    private readonly IGroupService _groupService;

    public GroupResolver(IGroupService groupService)
    {
        _groupService = groupService;
    }

    public Group GetGroup(Message message, IResolverContext context)
    {
        return _groupService.GetAll().FirstOrDefault(x => x.Id == message.GroupId)!;
    }
}