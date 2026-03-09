namespace Shared;

public class ValidationResult
{
    private readonly List<ValidationError> _errors = [];

    public IReadOnlyCollection<ValidationError> Errors => _errors;

    public bool IsValid => !_errors.Any();

    public void Add(string field, string message)
    {
        _errors.Add(new ValidationError { Field = field, Message = message });
    }
}
