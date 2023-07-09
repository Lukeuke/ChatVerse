using Chat.Domain.Data;
using Chat.Domain.DTOs;
using Chat.Domain.Models;
using HotChocolate.Subscriptions;

namespace Chat.Application.Services;

public interface IMessageService
{
    IQueryable<Message> GetAll([Service] ChatDbContext context);
    bool Create(CreateMessageDto request, [Service] ITopicEventSender eventSender, [Service] ChatDbContext context);
}