using Microsoft.AspNetCore.Mvc;

namespace feature.User;

public static class CreateUserEndpoint
{
    public static void MapCreateUserEndpoint(this WebApplication app)
    {
        app.MapPost(
                "/v1/users",
                async (CreateUserHandler handler, [FromBody] CreateUserRequest request) =>
                {
                    CreateUserResponse result = await handler.Handle(request);
                    return Results.Ok(result);
                }
            )
            .WithName("CreateUser")
            .WithTags("Users")
            .WithOpenApi();
    }
}
