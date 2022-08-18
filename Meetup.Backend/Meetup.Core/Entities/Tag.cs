namespace Meetup.Core.Entities;

public class Tag
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string EventId { get; set; }
    public Event Event { get; set; }
}