namespace feature.user;

public static class GetAllUsersByFiltersEndPoint
{
    public static void MapGetAllUsersByFiltersEndpoint(this WebApplication app)
    {
        app.MapGet(
                "/v1/users",
                async (GetAllUsersByFiltersRequest request, GetAllUsersByFiltersHandler handler) =>
                {
                    IEnumerable<GetAllUsersByFiltersResponse> result = await handler.Handle(
                        request
                    );
                    return Results.Ok(result);
                }
            )
            .WithName("GetAllUsersByFilters")
            .WithTags("Users")
            .WithOpenApi();
    }
}
