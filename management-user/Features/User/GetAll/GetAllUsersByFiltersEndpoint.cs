using Shared;

namespace feature.user;

public static class GetAllUsersByFiltersEndPoint
{
    public static void MapGetAllUsersByFiltersEndpoint(this WebApplication app)
    {
        app.MapGet(
                "/v1/users",
                async (
                    [AsParameters] GetAllUsersByFiltersRequest request,
                    GetAllUsersByFiltersHandler handler
                ) =>
                {
                    var result = await handler.Handle(request);

                    if (!result.IsSuccess)
                        return Results.Problem(statusCode: 500, title: result.Errors.First().Title);

                    return JsonApiResults.OkCollection("users", result.Value);
                }
            )
            .WithName("GetAllUsersByFilters")
            .WithTags("Users")
            .WithOpenApi();
    }
}
