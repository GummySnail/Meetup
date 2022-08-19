using Meetup.Core.Entities;
using Meetup.Infrastructure.Identity;
using Meetup.Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.Data.Repositories;

public class AuthUserRepository : IAuthUserRepository
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly AppDbContext _context;

    public AuthUserRepository(UserManager<AuthUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<bool> IsUserExistByEmailAsync(string email)
    {
        return await _userManager.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpperInvariant());
    }

    public async Task<bool> IsUserExistByUserNameAsync(string userName)
    {
        return await _userManager.Users.AnyAsync(x => x.NormalizedUserName == userName.ToUpperInvariant());
    }
    
    public async Task CreateUserAsync(AuthUser authUser, string password, User user, string role = "User")
    {
        await _userManager.CreateAsync(authUser, password);
        await _userManager.AddToRoleAsync(authUser, role);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<AuthUser> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email.ToUpperInvariant());
    }

    public async Task<AuthUser> GetUserByUserNameAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName.ToUpperInvariant());
    }

    public async Task<bool> CheckPasswordAsync(AuthUser authUser, string password)
    {
        return await _userManager.CheckPasswordAsync(authUser, password);
    }
}