using Chat.Application.Services;
using Chat.Domain.Data;
using Chat.Domain.DTOs;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;

namespace Chat.Application.GraphQL;

public class Mutation
{
    [Authorize]
    public async Task<bool> CreateMessage([Service] ChatDbContext context,
        [Service] IMessageService messageService,
        CreateMessageDto request, 
        [Service] ITopicEventSender eventSender, 
        Guid? groupId)
    {
        return await messageService.Create(context, request, eventSender, groupId);
    }
}