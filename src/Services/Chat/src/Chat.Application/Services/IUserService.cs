using Chat.Domain.DTOs;
using Chat.Domain.Models;

namespace Chat.Application.Services;

public interface IUserService
{
    IQueryable<User> GetAll();
    bool Create(CreateUserDto request);
}