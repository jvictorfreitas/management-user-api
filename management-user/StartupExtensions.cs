using feature.User;

namespace management.user;

public static class StartupExtensions
{
    public static IServiceCollection AddHandlers(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<CreateUserHandler>();
        return services;
    }

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services;
    }

    public static void ConfigureRoutes(this WebApplication app)
    {
        CreateUserEndpoint.MapCreateUserEndpoint(app);
    }
}
