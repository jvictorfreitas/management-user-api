namespace Shared;

public class JsonApiExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public JsonApiExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadHttpRequestException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/vnd.api+json";

            var response = new
            {
                errors = new[]
                {
                    new
                    {
                        status = "400",
                        title = "Invalid request parameter",
                        detail = ex.Message,
                    },
                },
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
