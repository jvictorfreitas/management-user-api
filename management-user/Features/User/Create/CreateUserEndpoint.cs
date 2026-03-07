using shared.jsonapi;

namespace feature.user;

public static class CreateUserEndpoint
{
    public static void MapCreateUserEndpoint(this WebApplication app)
    {
        app.MapPost(
                "/v1/users",
                async (CreateUserHandler handler, CreateUserRequest request) =>
                {
                    (string id, CreateUserResponse attributes) result = await handler.Handle(
                        request
                    );

                    return JsonApiResults.Created(
                        "users",
                        result.id,
                        result.attributes,
                        $"/v1/users/{result.id}"
                    );
                }
            )
            .WithName("CreateUser")
            .WithTags("Users")
            .WithOpenApi();
    }
}
