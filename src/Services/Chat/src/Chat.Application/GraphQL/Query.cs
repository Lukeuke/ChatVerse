using Chat.Application.Services;
using Chat.Application.Types;
using Chat.Domain.Data;
using Chat.Domain.Models;

namespace Chat.Application.GraphQL;

public class Query
{
    private readonly IMessageService _messageService;
    private readonly IGroupService _groupService;

    public Query(IMessageService messageService, IGroupService groupService)
    {
        _messageService = messageService;
        _groupService = groupService;
    }

    [UsePaging(SchemaType = typeof(GroupType))]
    [UseFiltering]
    public IQueryable<Group> Groups => _groupService.GetAll();
    
    [UsePaging(SchemaType = typeof(MessageType))]
    [UseFiltering]
    public IQueryable<Message> Messages => _messageService.GetAll();
}