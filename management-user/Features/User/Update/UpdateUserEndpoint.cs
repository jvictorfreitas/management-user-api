using Microsoft.AspNetCore.Mvc;
using shared.jsonapi;

namespace feature.user;

public static class UpdateUserEndpoint
{
    public static void MapUpdateUserEndpoint(this WebApplication app)
    {
        app.MapPatch(
                "/v1/users/{id:guid}",
                async (
                    UpdateUserHandler handler,
                    Guid id,
                    JsonApiRequest<UpdateUserRequest> request
                ) =>
                {
                    if (request.Data.Id != id.ToString())
                    {
                        return JsonApiErrorResults.BadRequest(
                            "Invalid resource id",
                            "Body id must match route id"
                        );
                    }

                    (string id, UpdateUserResponse response) result = await handler.Handle(
                        id,
                        request.Data.Attributes
                    );

                    return JsonApiResults.Ok("users", result.id, result.response);
                }
            )
            .WithName("UpdateUser")
            .WithTags("Users")
            .WithOpenApi();
    }
}
