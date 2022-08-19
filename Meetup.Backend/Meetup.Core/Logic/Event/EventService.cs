using Meetup.Core.DTOs;
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
}