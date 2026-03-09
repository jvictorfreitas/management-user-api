namespace Shared;

public static class ValidationErrorMapper
{
    public static object ToJsonApiErrors(ValidationResult validation)
    {
        return new
        {
            errors = validation.Errors.Select(e => new
            {
                detail = e.Message,
                source = new { pointer = e.Field },
            }),
        };
    }
}
