using Chat.Domain.DTOs;
using Chat.Domain.Models;
using HotChocolate.Subscriptions;

namespace Chat.Application.Services;

public interface IMessageService
{
    IQueryable<Message> GetAll();
    bool Create(CreateMessageDto request, [Service] ITopicEventSender eventSender);
}