using Meetup.Core.Logic.Event;
using Meetup.Core.Logic.RefreshToken;

namespace Meetup.Api.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<TokenService>();
        services.AddScoped<EventService>();
        return services;
    }
}