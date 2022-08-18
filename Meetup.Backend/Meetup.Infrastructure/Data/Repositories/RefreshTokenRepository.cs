using AutoMapper;
using Meetup.Core.Entities;
using Meetup.Core.Interfaces.Repositories;
using Meetup.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.Data.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public RefreshTokenRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task AddRefreshTokenAsync(string userId, string refreshToken, DateTime expiryTime)
    {
        await _context.RefreshTokens.AddAsync(new ApplicationUserRefreshTokens
        {
            UserId = userId,
            RefreshToken = refreshToken,
            ExpiryTime = expiryTime
        });
    }

    public async Task<RefreshToken> GetRefreshTokenAsync(string userId, string refreshToken)
    {
        return _mapper.Map<RefreshToken>(
            await _context.RefreshTokens.SingleOrDefaultAsync(x =>
                x.UserId == userId && x.RefreshToken == refreshToken));
    }

    public async Task UpdateRefreshTokenAsync(string userId, string oldRefreshToken, string newRefreshToken)
    {
        var refreshToken =
            await _context.RefreshTokens.SingleOrDefaultAsync(x =>
                x.UserId == userId && x.RefreshToken == oldRefreshToken);

        refreshToken.RefreshToken = newRefreshToken;
        refreshToken.ExpiryTime = DateTime.UtcNow.AddMonths(1);
    }

    public async Task DeleteRefreshTokenAsync(string userId, string refreshToken)
    {
        var refreshTokenSource =
            await _context.RefreshTokens.SingleOrDefaultAsync(x => x.UserId == userId && x.RefreshToken == refreshToken);

        if (refreshTokenSource is not null)
        {
            _context.RefreshTokens.Remove(refreshTokenSource);
        }
    }

    public void DeleteAllRefreshTokensAsync(string userId)
    {
        _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(x => x.UserId == userId));
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}