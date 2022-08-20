using Meetup.Core.Entities;
using Meetup.Infrastructure.Identity;
using Meetup.Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.Data;

public class DataSeeder
{
    public static async Task SetApplicationRoleConfiguration(RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.Roles.AnyAsync()) return;

        var roles = new List<IdentityRole>()
        {
            new IdentityRole { Name = "User" },
            new IdentityRole { Name = "Admin" }
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
    }

    public static async Task AddAdminAsync(IAuthUserRepository authUserRepository)
    {
        var authUser = new AuthUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "admin_ad_79@mail.ru",
            UserName = "MeetupAdmin001"
        };
        var user = new User
        {
            Id = authUser.Id,
            Email = authUser.Email,
            UserName = authUser.UserName
        };
        
        await authUserRepository.CreateUserAsync(authUser, "Pa$$w0rd", user, "Admin");
    }
    
    public static async Task AddUsersAsync(IAuthUserRepository authUserRepository)
    {
        var authUser1 = new AuthUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "first_user@mail.ru",
            UserName = "Cheburechek7"
        };
        
        var user1 = new User
        {
            Id = authUser1.Id,
            Email = authUser1.Email,
            UserName = authUser1.UserName
        };
        
        await authUserRepository.CreateUserAsync(authUser1, "rooT98^root", user1);
        
        var authUser2 = new AuthUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "second_user@mail.ru",
            UserName = "Wendigo557"
        };
        
        var user2 = new User
        {
            Id = authUser2.Id,
            Email = authUser2.Email,
            UserName = authUser2.UserName
        };
        
        await authUserRepository.CreateUserAsync(authUser2, "rooT98^root", user2);
        
        var authUser3 = new AuthUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "third_user@mail.ru",
            UserName = "ApacheJun300"
        };
        
        var user3 = new User
        {
            Id = authUser3.Id,
            Email = authUser3.Email,
            UserName = authUser3.UserName
        };
        
        await authUserRepository.CreateUserAsync(authUser3, "rooT98^root", user3);
    }
}