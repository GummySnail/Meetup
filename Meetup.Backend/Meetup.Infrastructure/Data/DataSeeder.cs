using Meetup.Core.Entities;
using Meetup.Infrastructure.Identity;
using Meetup.Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Infrastructure.Data;

public class DataSeeder
{
   // private static readonly IAuthUserRepository _authUserRepository;

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

    /*public static async Task SetAdmin()
    {
        var authUser = new AuthUser
        {
            UserName = "Wendigo000",
            Email = "wendigo_000.@mail.ru"
        };

        var user = new User
        {
            Id = authUser.Id,
            UserName = "Wendigo000",
            Email = "wendigo_000.@mail.ru"
        };
        
        await _authUserRepository.CreateUserAsync(authUser, "pa$$w0rd", user, "Admin");
    }*/
}