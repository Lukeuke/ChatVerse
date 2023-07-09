using Chat.Application.Services;
using Chat.Domain.Models;
using HotChocolate.Resolvers;

namespace Chat.Application.Resolvers;

public class MessageResolver
{
    private readonly IMessageService _messageService;

    public MessageResolver(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public IEnumerable<Message> GetMessages(Group group, IResolverContext context)
    {
        return _messageService.GetAll().Where(x => x.GroupId == group.Id);
    }
}