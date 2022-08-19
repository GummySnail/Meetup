using Meetup.Core.DTOs;
using Meetup.Core.Entities;
using Meetup.Core.Exceptions;
using Meetup.Core.Interfaces.Repositories;
using Meetup.Core.Logic.Event.Exceptions;

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
        if (startEvent <= DateTime.UtcNow)
        {
            throw new StartEventException("Event start date error");
        }

        if (await _eventRepository.AddEventAsync(name, description, city, startEvent, tagsDto, ownerId) == 0)
        {
            throw new SaveChangesToDbException("Can't save event");
        }
    }

    public async Task EditEventAsync(string? name, string? description, string? city, DateTime? startEvent,
        ICollection<TagDto>? tagsDto, string ownerId, string eventId)
    {
        var @event = await _eventRepository.IsEventExistAsync(eventId);
        
        if (@event is null)
        {
            throw new NotFoundException($"Event with id '{eventId}' not found");
        }

        if (!await _eventRepository.IsOwnerEvent(ownerId))
        {
            throw new EventOwnerException("Only owner can edit his event");
        }

        if (await _eventRepository.UpdateEventAsync(@event, name, description, city, startEvent, tagsDto) == 0)
        {
            throw new SaveChangesToDbException("Can't update event");
        }
    }

    public async Task DeleteEventAsync(string ownerId, string eventId)
    {
        var @event = await _eventRepository.IsEventExistAsync(eventId);
        
        if (@event is null)
        {
            throw new NotFoundException($"Event with id '{eventId}' not found");
        }

        if (!await _eventRepository.IsOwnerEvent(ownerId))
        {
            throw new EventOwnerException("Only owner can delete his event");
        }

        if (await _eventRepository.DeleteEventAsync(@event) == 0)
        {
            
        }
    }
}