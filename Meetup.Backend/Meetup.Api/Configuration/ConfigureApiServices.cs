namespace Meetup.Api.Configuration;

public static class ConfigureApiServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddControllers();
        services.AddCors();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.Configure<RouteOptions>(opt => opt.LowercaseUrls = true);

        return services;
    }
}