using Chat.Domain.Data;
using Chat.Domain.Models;

namespace Chat.Application.Services;

public interface IGroupService
{
    IQueryable<Group> GetAll();
}