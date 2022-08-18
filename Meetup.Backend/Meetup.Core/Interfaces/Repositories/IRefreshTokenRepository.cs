using Meetup.Core.Entities;

namespace Meetup.Core.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task AddRefreshTokenAsync(string userId, string refreshToken, DateTime expiryTime);
    Task<RefreshToken> GetRefreshTokenAsync(string userId, string refreshToken);
    Task UpdateRefreshTokenAsync(string userId, string oldRefreshToken, string newRefreshToken);
    Task DeleteRefreshTokenAsync(string userId, string refreshToken);
    void DeleteAllRefreshTokensAsync(string userId);
    Task SaveChangesAsync();
}