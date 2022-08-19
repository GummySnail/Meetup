using AutoMapper;
using Meetup.Core.DTOs;
using Meetup.Core.Entities;
using Meetup.Core.Interfaces.Repositories;
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

    public async Task<Event?> IsEventExistAsync(string eventId)
    {
       return await _context.Events.SingleOrDefaultAsync(ev => ev.Id == eventId);
    }

    public async Task<bool> IsOwnerEvent(string ownerId)
    {
        return await _context.Events.AnyAsync(ev => ev.OwnerId == ownerId);
    }


    public async Task<int> UpdateEventAsync(Event @event, string? name, string? description, string? city, DateTime? startEvent, ICollection<TagDto>? tagsDto)
    {
        var tags = new List<Tag>();

        if (tagsDto != null)
            foreach (var tagDto in tagsDto)
            {
                var tag = new Tag { EventId = @event.Id };
                tags.Add(_mapper.Map(tagDto, tag));
            }

        @event.Name = name;
        @event.Description = description;
        @event.City = city;
        @event.StartEvent = startEvent.Value;
        @event.Tags = tags;

        _context.Tags.RemoveRange(_context.Tags.Where(t => t.EventId == @event.Id));
        _context.Events.UpdateRange(@event);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteEventAsync(Event @event)
    { 
        _context.Events.RemoveRange(@event);
        return await _context.SaveChangesAsync();
    }
}