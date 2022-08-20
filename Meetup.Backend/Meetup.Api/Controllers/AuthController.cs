using Meetup.Api.Models.Auth.Requests;
using Meetup.Infrastructure.Identity.Response;
using Meetup.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Mvc;

namespace Meetup.Api.Controllers;

public class AuthController : BaseApiController
{
    private readonly IdentityService _identityService;

    public AuthController(IdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoggedResponse>> Register([FromBody] RegisterRequest request)
    {
        return Ok(await _identityService.RegisterAsync(request.Email, request.UserName, request.Password, request.ConfirmPassword));
    }

    [HttpPost("login-email")]
    public async Task<ActionResult> LoginByEmail([FromBody] LoginEmailRequest request)
    {
        return Ok(await _identityService.LoginByEmailAsync(request.Email, request.Password));
    }

    [HttpPost("login-username")]
    public async Task<ActionResult> LoginByUserName([FromBody] LoginUserNameRequest request)
    {
        return Ok(await _identityService.LoginByUserNameAsync(request.UserName, request.Password));
    }
}