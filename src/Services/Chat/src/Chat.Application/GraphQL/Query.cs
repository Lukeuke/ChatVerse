using Chat.Application.Services;
using Chat.Application.Types;
using Chat.Domain.Models;

namespace Chat.Application.GraphQL;

public class Query
{
    private readonly IUserService _userService;
    private readonly IGroupService _groupService;

    public Query(IUserService userService, IGroupService groupService)
    {
        _userService = userService;
        _groupService = groupService;
    }

    [UsePaging(SchemaType = typeof(GroupType))]
    [UseFiltering]
    public IQueryable<Group> Groups => _groupService.GetAll();
    
    [UsePaging(SchemaType = typeof(UserType))]
    [UseFiltering]
    public IQueryable<User> Users => _userService.GetAll();
}