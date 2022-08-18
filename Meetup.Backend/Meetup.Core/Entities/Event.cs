namespace Meetup.Core.Entities;

public class Event
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public DateTime StartEvent { get; set; }
    public string OwnerId { get; set; }
    
    public ICollection<Tag> Tags { get; set; }
    public ICollection<User> Users { get; set; }
}