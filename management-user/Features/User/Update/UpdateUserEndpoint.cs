using Shared;

namespace feature.user;

public static class UpdateUserEndpoint
{
    public static void MapUpdateUserEndpoint(this WebApplication app)
    {
        app.MapPatch(
                "/v1/users/{id:guid}",
                async (
                    UpdateUserHandler handler,
                    UpdateUserValidator validator,
                    Guid id,
                    UpdateUserRequest request
                ) =>
                {
                    ValidationResult validation = validator.Validate(request);

                    if (!validation.IsValid)
                    {
                        return Results.BadRequest(
                            ValidationErrorMapper.ToJsonApiErrors(validation)
                        );
                    }

                    var result = await handler.Handle(id, request);

                    if (!result.IsSuccess)
                        return Results.Problem(statusCode: 500, title: result.Errors.First().Title);

                    return JsonApiResults.Ok("users", result.Value.id, result.Value.response);
                }
            )
            .WithName("UpdateUser")
            .WithTags("Users")
            .WithOpenApi();
    }
}
