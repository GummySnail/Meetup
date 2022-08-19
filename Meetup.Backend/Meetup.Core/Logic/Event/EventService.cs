using Meetup.Core.DTOs;
using Meetup.Core.Exceptions;
using Meetup.Core.Interfaces.Repositories;
using Meetup.Core.Logic.Event.Exceptions;
using Meetup.Core.Logic.Event.Response;

namespace Meetup.Core.Logic.Event;

public class EventService
{
    private readonly IEventRepository _eventRepository;

    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task AddEventAsync(string name, string description, string city, DateTime startEvent, ICollection<TagDto> tagsDto, string ownerId)
    {
        if (await _eventRepository.AddEventAsync(name, description, city, startEvent, tagsDto, ownerId) == 0)
        {
            throw new SaveChangesToDbException("Can't save event");
        }
    }

    public async Task EditEventAsync(string? name, string? description, string? city, DateTime? startEvent,
        ICollection<TagDto>? tagsDto, string ownerId, string eventId)
    {
        if (!await _eventRepository.IsEventExistAsync(eventId))
        {
            throw new NotFoundException($"Event with id '{eventId}' not found");
        }

        if (!await _eventRepository.IsOwnerEventAsync(ownerId))
        {
            throw new EventOwnerException("Only owner can edit his event");
        }

        var @event = await _eventRepository.GetEventByIdAsync(eventId);
        
        if (await _eventRepository.UpdateEventAsync(@event, name, description, city, startEvent, tagsDto) == 0)
        {
            throw new SaveChangesToDbException("Can't update event");
        }
    }

    public async Task DeleteEventAsync(string ownerId, string eventId)
    {
        if (!await _eventRepository.IsEventExistAsync(eventId))
        {
            throw new NotFoundException($"Event with id '{eventId}' not found");
        }

        if (!await _eventRepository.IsOwnerEventAsync(ownerId))
        {
            throw new EventOwnerException("Only owner can delete his event");
        }

        var @event = await _eventRepository.GetEventByIdAsync(eventId);
        
        if (await _eventRepository.DeleteEventAsync(@event) == 0)
        {
            throw new SaveChangesToDbException("Can't delete event");
        }
    }

    public async Task<EventResponse> GetEventByIdAsync(string eventId)
    {
        if (!await _eventRepository.IsEventExistAsync(eventId))
        {
            throw new NotFoundException($"Event with id '{eventId}' not found");
        }

        var @event = await _eventRepository.GetEventByIdAsync(eventId);
        
        return _eventRepository.MappingToResponseEventModel(@event);
    }

    public async Task<ICollection<EventResponse>> GetEventsAsync(EventParams eventParams)
    {
        var events = await _eventRepository.GetEventsAsync(eventParams);

        return _eventRepository.MappingToResponseListEventModel(events);
    }

    public async Task SignUpForEventAsync(string userId, string eventId)
    {
        if (!await _eventRepository.IsEventExistAsync(eventId))
        {
            throw new NotFoundException($"Event with id '{eventId}' not found");
        }

        if (await _eventRepository.IsOwnerEventAsync(userId))
        {
            throw new EventOwnerException("Owners can't sign up to they events");
        }

        if (await _eventRepository.SignUpForEventAsync(userId, eventId) == 0)
        {
            throw new SaveChangesToDbException("Can't sign up for event");
        }
    }
}