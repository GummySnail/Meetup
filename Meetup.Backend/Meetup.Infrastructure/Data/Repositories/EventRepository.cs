using AutoMapper;
using Meetup.Core.DTOs;
using Meetup.Core.Entities;
using Meetup.Core.Interfaces.Repositories;

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
}