using Identity.Application.Helpers;
using Identity.Domain.Data;
using Identity.Domain.DTOs;
using Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _context;

    public UserRepository(IdentityDbContext context)
    {
        _context = context;
    }
    
    public async Task<(bool, object)> Add(CreateUserRequestDto request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            PasswordHash = request.Password,
            CreatedAt = DateTimeOffset.Now.ToUnixTimeSeconds().ToString()
        };
        
        user.ProvideSaltAndHash();
        
        if (Get(request.Username) is not null) return (false, new { Message = "User with this username already exists." });
        
        await _context.Users.AddAsync(user);

        await _context.SaveChangesAsync();

        return (true, user);
    }

    public User? Get(Guid id)
    {
        return _context.Users.FirstOrDefault(x => x.Id == id);
    }

    private User? Get(string username)
    {
        return _context.Users.FirstOrDefault(x => x.Username == username);
    }
    
    public Task<List<User>> GetAll()
    {
        return _context.Users.ToListAsync();
    }

    public bool Delete()
    {
        throw new NotImplementedException();
    }
}