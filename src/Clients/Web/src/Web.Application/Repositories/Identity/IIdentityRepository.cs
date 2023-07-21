using Web.Domain.DTOs;

namespace Web.Application.Repositories.Identity;

public interface IIdentityRepository
{ 
    Task<(bool, object)> RegisterUser(RegisterUserDto userDto);
    Task<(bool, object)> LoginUser(SignInRequestDto userDto);
}