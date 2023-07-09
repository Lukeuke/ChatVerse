using Identity.Domain.DTOs;
using Identity.Domain.Models;

namespace Identity.Application.Repositories;

public interface IUserRepository
{
    Task<(bool, object)> Add(CreateUserRequestDto request);
    User? Get(Guid id);
    Task<List<User>> GetAll();
    bool Delete();
}