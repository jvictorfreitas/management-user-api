namespace Shared;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public List<JsonApiError> Errors { get; }

    private Result(bool isSuccess, T? value, List<JsonApiError> errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    public static Result<T> Success(T value) => new(true, value, new());

    public static Result<T> Failure(List<JsonApiError> errors) => new(false, default, errors);
}
