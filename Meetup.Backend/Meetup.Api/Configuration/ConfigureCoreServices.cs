using Meetup.Core.Logic.RefreshToken;

namespace Meetup.Api.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<TokenService>();
        return services;
    }
}