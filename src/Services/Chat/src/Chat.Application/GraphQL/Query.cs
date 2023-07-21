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
    [UseSorting]
    public IQueryable<Group> Groups([Service] ChatDbContext context) => context.Groups;

    [Authorize]
    [UsePaging(SchemaType = typeof(MessageType))]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Message> Messages([Service] ChatDbContext context) => context.Messages;
}