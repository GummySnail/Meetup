using System.Security.Claims;
using Meetup.Core.Interfaces.Repositories;
using Meetup.Core.Interfaces.Services;
using Meetup.Core.Logic.RefreshToken.Exceptions;
using Meetup.Core.Logic.RefreshToken.Response;

namespace Meetup.Core.Logic.RefreshToken;

public class TokenService
{
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;

    public TokenService(ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var principal = _tokenService.GetPrincipalFromAccessToken(accessToken);
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var user = await _userRepository.GetUserByIdAsync(userId);
        var userRefreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(userId, refreshToken);

        if (user is null || userRefreshToken is null || userRefreshToken.ExpiryTime <= DateTime.UtcNow)
        {
            throw new RefreshTokenException("Unable to refresh token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        await _refreshTokenRepository.UpdateRefreshTokenAsync(userId, refreshToken, newRefreshToken);
        await _refreshTokenRepository.SaveChangesAsync();

        return new RefreshTokenResponse(newAccessToken, newRefreshToken);
    }

    public async Task RevokeTokenAsync(string userId, string refreshToken)
    {
        await _refreshTokenRepository.DeleteRefreshTokenAsync(userId, refreshToken);
        await _refreshTokenRepository.SaveChangesAsync();
    }

    public async Task RevokeAllTokensAsync(string userId)
    {
        _refreshTokenRepository.DeleteAllRefreshTokensAsync(userId);
        await _refreshTokenRepository.SaveChangesAsync();
    }
}