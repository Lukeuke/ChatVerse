using Chat.Application.Services;
using Chat.Domain.DTOs;
using HotChocolate.Subscriptions;

namespace Chat.Application.GraphQL;

public class Mutation
{
    private readonly IMessageService _messageService;

    public Mutation(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task<bool> CreateMessage(CreateMessageDto request, [Service] ITopicEventSender eventSender)
    {
        return _messageService.Create(request, eventSender);
    }
}