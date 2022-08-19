using Meetup.Api.Extensions;
using Meetup.Api.Models;
using Meetup.Api.Models.Event.Requests;
using Meetup.Core.Logic;
using Meetup.Core.Logic.Event;
using Meetup.Core.Logic.Event.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meetup.Api.Controllers;

public class EventController : BaseApiController
{
    private readonly EventService _eventService;

    public EventController(EventService eventService)
    {
        _eventService = eventService;
    }

    [Authorize]
    [HttpPost("create-event")]
    public async Task<ActionResult> CreateEvent([FromBody] CreateEventRequest request)
    {
        await _eventService.AddEventAsync(request.Name, request.Description, request.City, request.StartEvent, request.Tags, User.GetId());
        return NoContent();
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateEvent([FromBody] UpdateEventRequest request, string id)
    {
        await _eventService.EditEventAsync(request?.Name, request?.Description, request?.City, request?.StartEvent,
            request?.Tags, User.GetId(), id);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEvent(string id)
    {
        await _eventService.DeleteEventAsync(User.GetId(), id);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventResponse>> GetEvent(string id)
    {
        return Ok(await _eventService.GetEventByIdAsync(id));
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<EventResponse>>> GetEvents([FromQuery] EventParams eventParams)
    {
        return Ok(await _eventService.GetEventsAsync(eventParams));
    }

    [Authorize]
    [HttpPost("{id}")]
    public async Task<ActionResult> SignUpForEvent(string id)
    {
        await _eventService.SignUpForEventAsync(User.GetId(), id);
        return NoContent();
    }
}