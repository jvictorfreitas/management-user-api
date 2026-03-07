using Microsoft.AspNetCore.Mvc;
using shared.jsonapi;

namespace feature.user;

public static class GetUserByIdEndpoint
{
    public static void MapGetUserByIdEndpoint(this WebApplication app)
    {
        app.MapGet(
                "/users/{id}",
                async (GetUserByIdHandler handler, [FromRoute] Guid id) =>
                {
                    (string id, GetUserByIdResponse response) result = await handler.Handle(id);
                    return JsonApiResults.Ok("users", result.id, result.response);
                }
            )
            .WithName("GetUserById")
            .WithTags("Users")
            .WithOpenApi();
    }
}
