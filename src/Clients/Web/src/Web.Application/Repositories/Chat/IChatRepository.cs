using Web.Domain.DTOs;
using Web.Domain.Models.Chat;

namespace Web.Application.Repositories.Chat;

public interface IChatRepository
{
    Task<IEnumerable<ChatNameIdModel>> GetChats(string jwt);
}