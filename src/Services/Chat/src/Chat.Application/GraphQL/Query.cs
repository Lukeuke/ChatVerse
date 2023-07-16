using Chat.Application.Types;
using Chat.Domain.Data;
using Chat.Domain.Models;

namespace Chat.Application.GraphQL;

public class Query
{
    [UsePaging(SchemaType = typeof(GroupType))]
    [UseFiltering]
    public IQueryable<Group> Groups([Service] ChatDbContext context) => context.Groups;

    [UsePaging(SchemaType = typeof(MessageType))]
    [UseFiltering]
    public IQueryable<Message> Messages([Service] ChatDbContext context) => context.Messages;
}