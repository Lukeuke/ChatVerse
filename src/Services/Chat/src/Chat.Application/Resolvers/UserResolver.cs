using Chat.Application.Services;
using Chat.Domain.Models;
using HotChocolate.Resolvers;

namespace Chat.Application.Resolvers;

public class UserResolver
{
    private readonly IUserService _userService;

    public UserResolver(IUserService userService)
    {
        _userService = userService;
    }

    public IEnumerable<User> GetUsers(Group group, IResolverContext context)
    {
        return _userService.GetAll().Where(x => x.GroupId == group.Id);
    }
}