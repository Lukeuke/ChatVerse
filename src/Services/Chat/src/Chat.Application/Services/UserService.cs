using Chat.Domain.DTOs;
using Chat.Domain.Models;

namespace Chat.Application.Services;

public class UserService : IUserService
{
    private List<User> _users = new()
    {
        new User
        {
            GroupId = Guid.Parse("94D4D7EA-1883-428D-B051-98E702467210"),
            Name = "Tom",
            Id = Guid.NewGuid()
        }
    };
    
    public IQueryable<User> GetAll()
    {
        return _users.AsQueryable();
    }

    public bool Create(CreateUserDto request)
    {
        _users.Add(new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            GroupId = request.GroupId
        });

        return true;
    }
}