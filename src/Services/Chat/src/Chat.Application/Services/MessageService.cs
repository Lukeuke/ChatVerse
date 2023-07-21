﻿using Chat.Application.GraphQL;
using Chat.Domain.Data;
using Chat.Domain.DTOs;
using Chat.Domain.Helpers.Authorization;
using Chat.Domain.Models;
using HotChocolate.Subscriptions;

namespace Chat.Application.Services;

public class MessageService : IMessageService
{
    private readonly IHttpClientFactory _clientFactory;

    public MessageService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    
    public async Task<bool> Create(
        ChatDbContext context, 
        CreateMessageDto request, 
        [Service] ITopicEventSender eventSender,
        string token)
    {
        if (!await IsUserInGroup(request.GroupId, token))
        {
            return false;
        }
        
        var message = new Message
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            SenderId = JwtHelper.ParseTokenIntoUserId(token).ToString(),
            GroupId = request.GroupId
        };

        context.Messages.Add(message);
        await context.SaveChangesAsync();

        await eventSender.SendAsync(nameof(Subscription.MessageCreated), message);
        return true;
    }

    private async Task<bool> IsUserInGroup(Guid groupId, string token)
    {
        var client = _clientFactory.CreateClient("group");
        client.DefaultRequestHeaders.Add("Authorization", token);

        Console.WriteLine(client.BaseAddress);
        
        var result = await client.GetAsync($"group/{groupId}/has_member");

        if (result.IsSuccessStatusCode)
        {
            return bool.Parse(await result.Content.ReadAsStringAsync());
        }

        return false;
    }
}