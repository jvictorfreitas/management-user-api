using feature.user;

namespace management.user;

public static class StartupExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<CreateUserHandler>();
        services.AddScoped<DeleteUserHandler>();
        services.AddScoped<GetAllUsersByFiltersHandler>();
        services.AddScoped<GetUserByIdHandler>();
        services.AddScoped<UpdateUserHandler>();
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
        DeleteUserEndpoint.MapDeleteUserEndpoint(app);
        GetAllUsersByFiltersEndPoint.MapGetAllUsersByFiltersEndpoint(app);
        GetUserByIdEndpoint.MapGetUserByIdEndpoint(app);
        UpdateUserEndpoint.MapUpdateUserEndpoint(app);
    }
}
