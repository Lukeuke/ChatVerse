using Chat.Application.Services;
using Chat.Domain.Data;
using Chat.Domain.DTOs;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Application.GraphQL;

public class Mutation
{
    [Authorize]
    public async Task<bool> CreateMessage([Service] ChatDbContext context,
        [Service] IMessageService messageService,
        CreateMessageDto request, 
        [Service] ITopicEventSender eventSender, 
        [Service] IHttpContextAccessor httpContextAccessor)
    {
        var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"][0];
        
        return await messageService.Create(context, request, eventSender, token!);
    }
}