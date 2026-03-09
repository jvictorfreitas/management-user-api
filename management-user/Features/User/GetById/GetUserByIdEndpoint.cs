using Microsoft.AspNetCore.Mvc;
using Shared;

namespace feature.user;

public static class GetUserByIdEndpoint
{
    public static void MapGetUserByIdEndpoint(this WebApplication app)
    {
        app.MapGet(
                "/users/{id}",
                async (GetUserByIdHandler handler, [FromRoute] Guid id) =>
                {
                    var result = await handler.Handle(id);

                    if (!result.IsSuccess)
                        return Results.Problem(statusCode: 500, title: result.Errors.First().Title);

                    return JsonApiResults.Ok("users", result.Value.id, result.Value.response);
                }
            )
            .WithName("GetUserById")
            .WithTags("Users")
            .WithOpenApi();
    }
}
