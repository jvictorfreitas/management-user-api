using Shared;

namespace feature.user;

public class CreateUserValidator : IValidator<CreateUserRequest>
{
    public ValidationResult Validate(CreateUserRequest request)
    {
        ValidationResult result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(request.Name))
            result.Add("name", "Name is required");

        if (string.IsNullOrWhiteSpace(request.Cpf))
            result.Add("cpf", "Cpf is required");

        if (request.Cpf?.Length != 11)
            result.Add("cpf", "Cpf must contain 11 digits");

        return result;
    }
}
