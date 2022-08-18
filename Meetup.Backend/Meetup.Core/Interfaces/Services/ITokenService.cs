using System.Security.Claims;

namespace Meetup.Core.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromAccessToken(string accessToken);
}