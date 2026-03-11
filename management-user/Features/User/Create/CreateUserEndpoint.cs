using Shared;

namespace feature.user;

public static class CreateUserEndpoint
{
    public static void MapCreateUserEndpoint(this WebApplication app)
    {
        app.MapPost(
                "/v1/users",
                async (
                    CreateUserHandler handler,
                    CreateUserValidator validator,
                    CreateUserRequest request,
                    CancellationToken cancellationToken
                ) =>
                {
                    ValidationResult validation = validator.Validate(request);

                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(
                            ValidationErrorMapper.ToJsonApiErrors(validation)
                        );
                    }

                    var result = await handler.Handle(request, cancellationToken);

                    if (!result.IsSuccess)
                        return Results.Problem(statusCode: 500, title: result.Errors.First().Title);

                    return JsonApiResults.Created("users", result.Value.id, result.Value.response);
                }
            )
            .WithName("CreateUser")
            .WithTags("Users")
            .WithOpenApi();
    }
}
