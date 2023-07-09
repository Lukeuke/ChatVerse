using Chat.Application.Resolvers;
using Chat.Domain.Models;

namespace Chat.Application.Types;

public class GroupType : ObjectType<Group>
{
    protected override void Configure(IObjectTypeDescriptor<Group> descriptor)
    {
        descriptor.Field(x => x.Id).Type<IdType>();
        descriptor.Field(x => x.Name).Type<StringType>();

       descriptor.Field<MessageResolver>(x => x.GetMessages(default, default));
    }
}