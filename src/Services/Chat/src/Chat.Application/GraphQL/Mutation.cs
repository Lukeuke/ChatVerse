using Chat.Application.Services;
using Chat.Domain.DTOs;

namespace Chat.Application.GraphQL;

public class Mutation
{
    private readonly IUserService _userService;

    public Mutation(IUserService userService)
    {
        _userService = userService;
    }

    public bool CreateUser(CreateUserDto request)
    {
        return _userService.Create(request);
    }
}