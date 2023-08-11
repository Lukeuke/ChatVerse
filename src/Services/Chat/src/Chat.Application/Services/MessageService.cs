using System.Text.Json;
using Chat.Application.GraphQL;
using Chat.Domain.Data;
using Chat.Domain.DTOs;
using Chat.Domain.Helpers.Authorization;
using Chat.Domain.Models;
using HotChocolate.Subscriptions;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

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

        var user = await GetUser(token);
        
        var message = new Message
        {
            Id = Guid.NewGuid(),
            Content = request.Content,
            SenderId = JwtHelper.ParseTokenIntoUserIdHeader(token).ToString(),
            GroupId = request.GroupId,
            SenderFullName = user.Username,
            TimeStampOffset = DateTimeOffset.Now.ToUnixTimeSeconds()
        };

        context.Messages.Add(message);
        await context.SaveChangesAsync();

        await eventSender.SendAsync(nameof(Subscription.MessageCreated), message);
        return true;
    }

    public async Task<bool> ReadStatus(ChatDbContext context, ReadStatusDto request, ITopicEventSender eventSender, string token)
    {
        try
        {
            if (!await IsUserInGroup(request.GroupId, token))
            {
                return false;
            }
        
            var user = await GetUser(token);

            await context.ReadStatus.AddAsync(new ReadStatus
            {
                MessageId = request.MessageId,
                GroupId = request.GroupId,
                Message = context.Messages.First(x => x.Id == request.MessageId),
                SenderId = user.Id,
                SenderUsername = user.Username
            });

            await context.SaveChangesAsync();

            var readStatusEnumerable = context.ReadStatus.Where(x => x.GroupId == request.GroupId && x.MessageId == request.MessageId);

            await eventSender.SendAsync(nameof(Subscription.MessageReadStatus), readStatusEnumerable.ToList());
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<User> GetUser(string token)
    {
        var client = _clientFactory.CreateClient("identity");
        client.DefaultRequestHeaders.Add("Authorization", token);
        
        var result = await client.GetAsync("auth/user");
        
        var json = await result.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<User>(json)!;
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