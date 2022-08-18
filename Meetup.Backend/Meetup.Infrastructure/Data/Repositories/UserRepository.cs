using Meetup.Core.Entities;
using Meetup.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
    }
}