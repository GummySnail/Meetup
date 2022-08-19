using Meetup.Core.DTOs;

namespace Meetup.Core.Interfaces.Repositories;

public interface IEventRepository
{
    Task<int> AddEventAsync(string name, string description, string city, DateTime startEvent, ICollection<TagDto> tagsDto, string ownerId);
}