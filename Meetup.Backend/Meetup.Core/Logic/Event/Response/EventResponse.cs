using Meetup.Core.DTOs;

namespace Meetup.Core.Logic.Event.Response;

public record EventResponse(string Name, string Description, string City, DateTime StartEvent, string OwnerId, ICollection<TagDto> Tags);