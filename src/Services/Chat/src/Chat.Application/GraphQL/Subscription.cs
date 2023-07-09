using Chat.Domain.Models;

namespace Chat.Application.GraphQL;

public class Subscription
{
    [Subscribe]
    public Message MessageCreated([EventMessage] Message message) => message;
}