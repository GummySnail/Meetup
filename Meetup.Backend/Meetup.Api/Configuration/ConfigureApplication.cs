using Meetup.Api.Middleware;
using Meetup.Infrastructure.Data;
using Meetup.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Meetup.Api.Configuration;

public static class ConfigureApplication
{
    public static WebApplication AddApplicationConfiguration(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.UseDefaultFiles();
        app.UseStaticFiles();

        return app;
    }

    public static async Task AddDatabaseConfiguration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            var userManager = services.GetRequiredService<UserManager<AuthUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.MigrateAsync();

            await DataSeeder.SetApplicationRoleConfiguration(roleManager);
            //await DataSeeder.SetAdmin();
        }
        catch (Exception ex)
        {
            services.GetRequiredService<ILogger<Program>>().LogError(ex, "Error during database configuration");
        }
    }
    
}