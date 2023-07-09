using Chat.Application.Services;
using Chat.Domain.Data;
using Chat.Domain.DTOs;
using HotChocolate.Subscriptions;

namespace Chat.Application.GraphQL;

public class Mutation
{
    private readonly IMessageService _messageService;
    private readonly ChatDbContext _context;

    public Mutation(IMessageService messageService, ChatDbContext context)
    {
        _messageService = messageService;
        _context = context;
    }

    public async Task<bool> CreateMessage(CreateMessageDto request, [Service] ITopicEventSender eventSender)
    {
        return _messageService.Create(request, eventSender, _context);
    }
}