using Chat.Domain.Data;
using Chat.Domain.DTOs;
using HotChocolate.Subscriptions;

namespace Chat.Application.Services;

public interface IMessageService
{
    Task<bool> Create(ChatDbContext context,CreateMessageDto request, [Service] ITopicEventSender eventSender, Guid? groupId);
}