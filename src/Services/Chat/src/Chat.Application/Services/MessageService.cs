using Chat.Application.GraphQL;
using Chat.Domain.DTOs;
using Chat.Domain.Models;
using HotChocolate.Subscriptions;

namespace Chat.Application.Services;

public class MessageService : IMessageService
{
    private List<Message> _messages = new()
    {
        new Message
        {
            GroupId = Guid.Parse("94D4D7EA-1883-428D-B051-98E702467210"),
            Content = "Tom",
            Id = Guid.NewGuid()
        }
    };
    
    public IQueryable<Message> GetAll()
    {
        return _messages.AsQueryable();
    }

    public bool Create(CreateMessageDto request, [Service] ITopicEventSender eventSender)
    {
        var message = new Message
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            SenderId = request.SenderId,
            GroupId = request.GroupId
        };
        
        _messages.Add(message);

        eventSender.SendAsync(nameof(Subscription.MessageCreated), message);
        
        return true;
    }
}