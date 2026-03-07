public class JsonApiError
{
    public string Status { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Detail { get; init; } = default!;
}

public class JsonApiErrorResponse
{
    public IEnumerable<JsonApiError> Errors { get; init; } = [];
}

public static class JsonApiErrorResults
{
    public static IResult BadRequest(string title, string detail)
    {
        var error = new JsonApiErrorResponse
        {
            Errors = new[]
            {
                new JsonApiError
                {
                    Status = "400",
                    Title = title,
                    Detail = detail,
                },
            },
        };

        return Results.Json(
            error,
            contentType: "application/vnd.api+json",
            statusCode: StatusCodes.Status400BadRequest
        );
    }
}
