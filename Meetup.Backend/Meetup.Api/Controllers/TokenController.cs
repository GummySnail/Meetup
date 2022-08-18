using Meetup.Api.Extensions;
using Meetup.Api.Models.RefreshToken.Requests;
using Meetup.Core.Logic.RefreshToken;
using Meetup.Core.Logic.RefreshToken.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meetup.Api.Controllers;

public class TokenController : BaseApiController
{
    private readonly TokenService _tokenService;

    public TokenController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        return Ok(await _tokenService.RefreshTokenAsync(request.AccessToken, request.RefreshToken));
    }

    [Authorize]
    [HttpPost("revoke")]
    public async Task<ActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
    {
        await _tokenService.RevokeTokenAsync(User.GetId(), request.RefreshToken);
        return NoContent();
    }
}