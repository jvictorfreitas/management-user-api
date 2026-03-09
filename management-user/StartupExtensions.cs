using feature.user;
using Infrastructure.cache;
using Infrastructure.database;
using queue.rabbit;
using RabbitMQ.Client;
using Shared;
using worker.outbox;
using worker.outBox;

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

    public static IServiceCollection AddQueue(this IServiceCollection services)
    {
        services.AddScoped<OutboxService>();
        services.AddSingleton<RabbitMqPublisher>();

        services.AddHostedService<OutboxCleaner>();
        services.AddHostedService<OutboxProcessor>();

        services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest",
            };

            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "redis:6379";
            options.InstanceName = "api_cache";
        });
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

    public static void ConfigureValidations(this IServiceCollection services)
    {
        services.AddScoped<CreateUserValidator>();
        services.AddScoped<UpdateUserValidator>();
    }
}
