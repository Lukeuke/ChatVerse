using Chat.Application.Services;
using Chat.Domain.Data;
using Chat.Domain.DTOs;
using Chat.Domain.Models;
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
        var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"][0];
        
        return await messageService.Create(context, request, eventSender, token!);
    }

    [Authorize]
    public async Task<bool> ReadStatus([Service] ChatDbContext context,
        ReadStatusDto readStatusDto,
        [Service] IMessageService messageService,
        [Service] ITopicEventSender eventSender, 
        [Service] IHttpContextAccessor httpContextAccessor)
    {
        try
        {
            var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"][0];

            return await messageService.ReadStatus(context, readStatusDto, eventSender, token!);
        }
        catch
        {
            return false;
        }
    }
}