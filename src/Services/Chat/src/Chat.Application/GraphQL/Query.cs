using Chat.Application.Services;
using Chat.Application.Types;
using Chat.Domain.Data;
using Chat.Domain.Models;

namespace Chat.Application.GraphQL;

public class Query
{
    private readonly IMessageService _messageService;
    private readonly IGroupService _groupService;
    private readonly ChatDbContext _context;

    public Query(IMessageService messageService, IGroupService groupService, ChatDbContext context)
    {
        _messageService = messageService;
        _groupService = groupService;
        _context = context;
    }

    [UsePaging(SchemaType = typeof(GroupType))]
    [UseFiltering]
    public IQueryable<Group> Groups => _groupService.GetAll(_context);
    
    [UsePaging(SchemaType = typeof(MessageType))]
    [UseFiltering]
    public IQueryable<Message> Messages => _messageService.GetAll(_context);
}