using Chat.Domain.Models;

namespace Chat.Application.Services;

public class GroupService : IGroupService
{
    private List<Group> _groups = new()
    {
        new Group
        {
            Id = Guid.Parse("94D4D7EA-1883-428D-B051-98E702467210"),
            Name = "Test"
        }
    };
    
    public IQueryable<Group> GetAll()
    {
        return _groups.AsQueryable();
    }
}