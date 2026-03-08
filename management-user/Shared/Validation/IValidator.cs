namespace Shared;

public interface IValidator<T>
{
    ValidationResult Validate(T model);
}
