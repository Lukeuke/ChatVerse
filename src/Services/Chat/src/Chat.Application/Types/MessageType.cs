using Chat.Application.Resolvers;
using Chat.Domain.Models;

namespace Chat.Application.Types;

public class MessageType : ObjectType<Message>
{
    protected override void Configure(IObjectTypeDescriptor<Message> descriptor)
    {
        descriptor.Field(x => x.Id).Type<IdType>();
        descriptor.Field(x => x.SenderId).Type<StringType>();
        descriptor.Field(x => x.Content).Type<StringType>();
        descriptor.Field(x => x.SenderFullName).Type<StringType>();
        
        //descriptor.Field<GroupResolver>(x => x.GetGroup(default, default));
    }
}