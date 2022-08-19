using Meetup.Core.DTOs;
using Meetup.Core.Entities;

namespace Meetup.Core.Interfaces.Repositories;

public interface IEventRepository
{
    Task<int> AddEventAsync(string name, string description, string city, DateTime startEvent, ICollection<TagDto> tagsDto, string ownerId);
    Task<Event?> IsEventExistAsync(string eventId);
    Task<bool> IsOwnerEvent(string ownerId);
    Task<int> UpdateEventAsync(Event @event, string? name, string? description, string? city, DateTime? startEvent, ICollection<TagDto>? tagsDto);
    Task<int> DeleteEventAsync(Event @event);
}