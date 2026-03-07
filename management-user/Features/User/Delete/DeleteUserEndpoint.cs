using Microsoft.AspNetCore.Mvc;

namespace feature.user;

public static class DeleteUserEndpoint
{
    public static void MapDeleteUserEndpoint(this WebApplication app)
    {
        app.MapDelete(
                "/v1/users/{id}",
                async (DeleteUserHandler handler, [FromRoute] Guid id) =>
                {
                    await handler.Handle(id);
                    return Results.NoContent();
                }
            )
            .WithName("DeleteUser")
            .WithTags("Users")
            .WithOpenApi();
    }
}
