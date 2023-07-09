using Chat.Application.Resolvers;
using Chat.Domain.Models;

namespace Chat.Application.Types;

public class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        descriptor.Field(x => x.Id).Type<IdType>();
        descriptor.Field(x => x.Name).Type<StringType>();
        descriptor.Field<GroupResolver>(x => x.GetGroup(default, default));
    }
}