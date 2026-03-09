namespace Shared;

public static class JsonApiResults
{
    public static IResult Ok<T>(string type, string id, T attributes)
    {
        var response = JsonApiMapper.ToResponse(type, id, attributes);

        return Results.Json(
            response,
            contentType: "application/vnd.api+json",
            statusCode: StatusCodes.Status200OK
        );
    }

    public static IResult Created<T>(string type, string id, T attributes)
    {
        var response = JsonApiMapper.ToResponse(type, id, attributes);

        return Results.Json(
            response,
            contentType: "application/vnd.api+json",
            statusCode: StatusCodes.Status201Created
        );
    }

    public static IResult OkCollection<T>(string type, IEnumerable<(string id, T attributes)> items)
    {
        var response = JsonApiMapper.ToCollection(type, items);

        return Results.Json(
            response,
            contentType: "application/vnd.api+json",
            statusCode: StatusCodes.Status200OK
        );
    }
}
