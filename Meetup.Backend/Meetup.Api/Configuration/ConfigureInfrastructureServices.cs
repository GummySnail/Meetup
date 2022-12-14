using System.Text;
using Meetup.Core.Interfaces.Repositories;
using Meetup.Core.Interfaces.Services;
using Meetup.Infrastructure.Data;
using Meetup.Infrastructure.Data.Repositories;
using Meetup.Infrastructure.Identity;
using Meetup.Infrastructure.Identity.Services;
using Meetup.Infrastructure.Interfaces.Repositories;
using Meetup.Infrastructure.Mapping;
using Meetup.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Meetup.Api.Configuration;

public static class ConfigureInfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string connectionString = String.Empty;

            if (environment == "Development")
            {
                connectionString = config.GetConnectionString("AppDbContextConnection");
            }
            
            opt.UseNpgsql(connectionString, x => x.MigrationsAssembly("Meetup.Infrastructure"));
        });

        services.AddIdentity<AuthUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredUniqueChars = 0;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"])),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
            opt.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
        });

        services.AddScoped<IAuthUserRepository, AuthUserRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IdentityService>();
        
        services.AddAutoMapper(typeof(MapperProfile).Assembly);

        return services;
    }
}