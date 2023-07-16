using Chat.Domain.Data;
using Chat.Domain.DTOs;
using Chat.Domain.Models;
using HotChocolate.Subscriptions;

namespace Chat.Application.Services;

public interface IMessageService
{
    IQueryable<Message> GetAll();
    Task<bool> Create(ChatDbContext context,CreateMessageDto request, [Service] ITopicEventSender eventSender, Guid? groupId);
}