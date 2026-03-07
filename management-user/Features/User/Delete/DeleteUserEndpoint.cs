using Microsoft.AspNetCore.Mvc;

namespace feature.user;

public static class DeleteUserEndpoint
{
    public static void MapDeleteUserEndpoint(this WebApplication app)
    {
        app.MapDelete(
                "/v1/users/{id}",
                async (DeleteUserHandler handler, Guid id) =>
                {
                    bool deleted = await handler.Handle(id);

                    if (!deleted)
                    {
                        return JsonApiErrorResults.BadRequest(
                            "Resource not found",
                            $"User with id '{id}' was not found"
                        );
                    }

                    return Results.NoContent();
                }
            )
            .WithName("DeleteUser")
            .WithTags("Users")
            .WithOpenApi();
    }
}
