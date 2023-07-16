using Chat.Application.GraphQL;
using Chat.Domain.Data;
using Chat.Domain.DTOs;
using Chat.Domain.Models;
using HotChocolate.Subscriptions;

namespace Chat.Application.Services;

public class MessageService : IMessageService
{
    public IQueryable<Message> GetAll(ChatDbContext context)
    {
        return context.Messages.AsQueryable();
    }

    public bool Create(CreateMessageDto request, [Service] ITopicEventSender eventSender, ChatDbContext context, Guid? groupId)
    {
        if (groupId.HasValue && !IsUserInGroup(request.SenderId, groupId.Value, context))
        {
            return false;
        }
        
        var message = new Message
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            SenderId = request.SenderId,
            GroupId = request.GroupId
        };
        
        context.Messages.Add(message);
        
        eventSender.SendAsync(nameof(Subscription.MessageCreated), message);

        context.SaveChanges();
        return true;
    }

    private bool IsUserInGroup(string requestSenderId, Guid groupId, ChatDbContext context)
    {
        // TODO: Check if user is in group from Group Service through RabbitMQ
        return true;
    }
}