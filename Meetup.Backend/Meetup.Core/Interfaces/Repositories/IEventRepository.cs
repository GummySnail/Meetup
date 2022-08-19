using Meetup.Core.DTOs;
using Meetup.Core.Entities;
using Meetup.Core.Logic;
using Meetup.Core.Logic.Event.Response;

namespace Meetup.Core.Interfaces.Repositories;

public interface IEventRepository
{
    Task<int> AddEventAsync(string name, string description, string city, DateTime startEvent, ICollection<TagDto> tagsDto, string ownerId);
    Task<bool> IsEventExistAsync(string eventId);
    Task<bool> IsOwnerEvent(string ownerId);
    Task<int> UpdateEventAsync(Event @event, string? name, string? description, string? city, DateTime? startEvent, ICollection<TagDto>? tagsDto);
    Task<int> DeleteEventAsync(Event @event);
    Task<Event> GetEventByIdAsync(string eventId);
    EventResponse MappingToResponseEventModel(Event @event);
    ICollection<EventResponse> MappingToResponseListEventModel(PagedList<Event> events);
    Task<PagedList<Event>> GetEventsAsync(EventParams eventParams);
}