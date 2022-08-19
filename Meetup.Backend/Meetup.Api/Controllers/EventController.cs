using Meetup.Api.Extensions;
using Meetup.Api.Models.Event.Requests;
using Meetup.Core.DTOs;
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
    
    /*[HttpGet]
    public async Task<ActionResult<EventListResponse>> GetEvents([FromQuery] )*/
}