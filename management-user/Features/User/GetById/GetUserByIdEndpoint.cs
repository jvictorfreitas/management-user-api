using Microsoft.AspNetCore.Mvc;

namespace feature.user;

public static class GetUserByIdEndpoint
{
    public static void MapGetUserByIdEndpoint(this WebApplication app)
    {
        app.MapGet(
                "/users/{id}",
                async (GetUserByIdHandler handler, [FromRoute] Guid id) =>
                {
                    GetUserByIdResponse result = await handler.Handle(id);
                    return Results.Ok(result);
                }
            )
            .WithName("GetUserById")
            .WithTags("Users")
            .WithOpenApi();
    }
}
