using Chat.Domain.Models;
using HotChocolate.Authorization;

namespace Chat.Application.GraphQL;

public class Subscription
{
    //[Authorize]
    [Subscribe]
    public Message? MessageCreated([EventMessage] Message message, string? groupId) 
    {
        // If a groupId is provided, check if the message belongs to the specified group.
        // If groupId is null, it means no filtering is applied, and all messages will be received.
        if (groupId == null || message.GroupId == Guid.Parse(groupId))
        {
            return message;
        }

        return null; // Return null if the message doesn't match the groupId filter.
    }
}