using Meetup.Core.DTOs;

namespace Meetup.Api.Models.Event.Requests;

public record UpdateEventRequest(string Name, string Description, string City, DateTime StartEvent, ICollection<TagDto>? Tags);