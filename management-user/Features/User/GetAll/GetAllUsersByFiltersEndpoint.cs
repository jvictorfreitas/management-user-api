using shared.jsonapi;

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
                    IEnumerable<(string id, GetAllUsersByFiltersResponse attributes)> result =
                        await handler.Handle(request);
                    return JsonApiResults.OkCollection("users", result);
                }
            )
            .WithName("GetAllUsersByFilters")
            .WithTags("Users")
            .WithOpenApi();
    }
}
