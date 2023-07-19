using Chat.Application.Types;
using Chat.Domain.Data;
using Chat.Domain.Models;
using HotChocolate.Authorization;

namespace Chat.Application.GraphQL;

public class Query
{
    [Authorize]
    [UsePaging(SchemaType = typeof(GroupType))]
    [UseFiltering]
    public IQueryable<Group> Groups([Service] ChatDbContext context) => context.Groups;

    [Authorize]
    [UsePaging(SchemaType = typeof(MessageType))]
    [UseFiltering]
    public IQueryable<Message> Messages([Service] ChatDbContext context) => context.Messages;
}