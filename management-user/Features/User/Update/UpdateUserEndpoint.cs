using Microsoft.AspNetCore.Mvc;

namespace feature.user;

public static class UpdateUserEndpoint
{
    public static void MapUpdateUserEndpoint(this WebApplication app)
    {
        app.MapPut(
                "/v1/users/{id:guid}",
                async (
                    UpdateUserHandler handler,
                    [FromRoute] Guid id,
                    [FromBody] UpdateUserRequest request
                ) =>
                {
                    UpdateUserResponse result = await handler.Handle(id, request);
                    return Results.Ok(result);
                }
            )
            .WithName("UpdateUser")
            .WithTags("Users")
            .WithOpenApi();
    }
}
