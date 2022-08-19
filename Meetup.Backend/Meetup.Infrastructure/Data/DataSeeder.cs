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

    public async Task AddAdminAsync()
    {
       /* var user = new AuthUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "admin@meetup.com",
            UserName = "Admin",
            
        }*/
    }
}