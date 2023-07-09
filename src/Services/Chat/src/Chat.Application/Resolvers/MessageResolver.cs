using Chat.Application.Services;
using Chat.Domain.Data;
using Chat.Domain.Models;
using HotChocolate.Resolvers;

namespace Chat.Application.Resolvers;

public class MessageResolver
{
    private readonly IMessageService _messageService;
    private readonly ChatDbContext _context;

    public MessageResolver(IMessageService messageService, ChatDbContext context)
    {
        _messageService = messageService;
        _context = context;
    }

    public IEnumerable<Message> GetMessages(Group group, IResolverContext context)
    {
        return _messageService.GetAll(_context).Where(x => x.GroupId == group.Id);
    }
}