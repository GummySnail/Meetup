using AutoMapper;
using Meetup.Core.DTOs;
using Meetup.Core.Entities;
using Meetup.Core.Interfaces.Repositories;
using Meetup.Core.Logic;
using Meetup.Core.Logic.Event.Response;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.Data.Repositories;

public class EventRepository : IEventRepository
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    
    public EventRepository(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<int> AddEventAsync(string name, string description, string city, DateTime startEvent, ICollection<TagDto> tagsDto, string ownerId)
    {
        Event @event = new Event
        {
            Name = name,
            Description = description,
            City = city,
            StartEvent = startEvent,
            OwnerId = ownerId
        };

        await _context.Events.AddAsync(@event);
        await _context.SaveChangesAsync();
        
        var tags = new List<Tag>();

        foreach (var tagDto in tagsDto)
        {
            var tag = new Tag{EventId = @event.Id};
            tags.Add(_mapper.Map(tagDto, tag));
        }

        await _context.Tags.AddRangeAsync(tags);
        return await _context.SaveChangesAsync();
    }

    public async Task<bool> IsEventExistAsync(string eventId)
    {
       return await _context.Events.AnyAsync(ev => ev.Id == eventId);
    }

    public async Task<bool> IsOwnerEventAsync(string ownerId)
    {
        return await _context.Events.AnyAsync(ev => ev.OwnerId == ownerId);
    }


    public async Task<int> UpdateEventAsync(Event @event, string? name, string? description, string? city, DateTime? startEvent, ICollection<TagDto>? tagsDto)
    {
        var tags = new List<Tag>();

        if (tagsDto != null)
        {
            foreach (var tagDto in tagsDto)
            {
                var tag = new Tag { EventId = @event.Id };
                tags.Add(_mapper.Map(tagDto, tag));
            }
        }

        @event.Name = name;
        @event.Description = description;
        @event.City = city;
        @event.StartEvent = startEvent.Value;
        @event.Tags = tags;
        
        _context.Events.Update(@event);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteEventAsync(Event @event)
    { 
        _context.Events.RemoveRange(@event);
        return await _context.SaveChangesAsync();
    }

    public async Task<Event> GetEventByIdAsync(string eventId)
    {
        return await _context.Events.Include(ev => ev.Tags).SingleOrDefaultAsync(ev => ev.Id == eventId);
    }

    public EventResponse MappingToResponseEventModel(Event @event)
    {
        return _mapper.Map<Event, EventResponse>(@event);
    }

    public ICollection<EventResponse> MappingToResponseListEventModel(PagedList<Event> events)
    {
        List<EventResponse> eventsList = new List<EventResponse>();

        foreach (var @event in events)
        {
            eventsList.Add(_mapper.Map<Event, EventResponse>(@event));
        }

        return eventsList;
    }

    public async Task<PagedList<Event>> GetEventsAsync(EventParams eventParams)
    {
        var query = _context.Events
            .Where(ev => ev.City.ToLower().Contains(eventParams.City.ToLower()))
            .Where(ev => ev.Name.ToLower().Contains(eventParams.Name.ToLower()));

        query = eventParams.OrderByDateTime switch
        {
            "Upcoming" => query.OrderBy(q => q.StartEvent).Include(ev => ev.Tags),
            _ => query.OrderByDescending(q => q.StartEvent).Include(ev => ev.Tags)
        };

        return await PagedList<Event>
            .CreateAsync(query, eventParams.PageNumber, eventParams.PageSize);
    }

    public async Task<int> SignUpForEventAsync(string userId, string eventId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Include(u => u.Events)
            .FirstOrDefaultAsync();

        var @event = await _context.Events.FindAsync(eventId);
        
        user.Events.Add(@event);
        
        return await _context.SaveChangesAsync();
    }
}