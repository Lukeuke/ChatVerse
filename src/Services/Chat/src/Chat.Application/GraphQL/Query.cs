using Chat.Application.Types;
using Chat.Domain.Data;
using Chat.Domain.Helpers.Authorization;
using Chat.Domain.Models;
using HotChocolate.Authorization;

namespace Chat.Application.GraphQL;

public class Query
{
    [Authorize]
    [UsePaging(SchemaType = typeof(MessageType))]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Message> Messages([Service] ChatDbContext context)
    {
        return context.Messages;
    }
    
    [Authorize]
    [UsePaging(SchemaType = typeof(MessageType))]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Message> MessagesByGroupId([Service] ChatDbContext context, Guid groupId)
    {
        return context.Messages.Where(x => x.GroupId == groupId);
    }
}