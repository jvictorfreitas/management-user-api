using Shared;

namespace feature.user;

public static class DeleteUserEndpoint
{
    public static void MapDeleteUserEndpoint(this WebApplication app)
    {
        app.MapDelete(
                "/v1/users/{id}",
                async (DeleteUserHandler handler, Guid id, CancellationToken cancellationToken) =>
                {
                    Result<bool> result = await handler.Handle(id, cancellationToken);

                    if (!result.IsSuccess)
                        return Results.Problem(statusCode: 500, title: result.Errors.First().Title);

                    if (!result.Value)
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
