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

    public bool Create(CreateMessageDto request, [Service] ITopicEventSender eventSender, ChatDbContext context)
    {
        var message = new Message
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            SenderId = request.SenderId,
            GroupId = request.GroupId
        };
        
        context.Messages.Add(message);
        
        //TODO: check if SenderId is in the GroupId and then send the event
        eventSender.SendAsync(nameof(Subscription.MessageCreated), message);

        context.SaveChanges();
        return true;
    }
}