using Meetup.Core.DTOs;

namespace Meetup.Api.Models.Event.Requests;

public record CreateEventRequest(string Name, string Description, string City, DateTime StartEvent, ICollection<TagDto> Tags);