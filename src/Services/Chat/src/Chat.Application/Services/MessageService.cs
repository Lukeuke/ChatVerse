using Chat.Application.GraphQL;
using Chat.Domain.Data;
using Chat.Domain.DTOs;
using Chat.Domain.Models;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;

namespace Chat.Application.Services;

public class MessageService : IMessageService
{
    private readonly ChatDbContext _context;

    public MessageService(ChatDbContext context)
    {
        _context = context;
    }
    
    public IQueryable<Message> GetAll()
    {
        return _context.Messages.AsQueryable();
    }

    public async Task<bool> Create(ChatDbContext context, CreateMessageDto request, [Service] ITopicEventSender eventSender, Guid? groupId)
    {
        if (groupId.HasValue && !IsUserInGroup(request.SenderId, groupId.Value))
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
        await context.SaveChangesAsync();

        await eventSender.SendAsync(nameof(Subscription.MessageCreated), message);
        return true;
    }

    private bool IsUserInGroup(string requestSenderId, Guid groupId)
    {
        // TODO: Check if user is in group from Group Service through RabbitMQ
        return true;
    }
}