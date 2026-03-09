using feature.user;

namespace management_user_tests.Validators;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator = new();

    [Fact]
    public void Validate_ShouldReturnValid_WhenAllFieldsAreCorrect()
    {
        var request = new CreateUserRequest("John Doe", "12345678901");

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenNameIsEmpty()
    {
        var request = new CreateUserRequest("", "12345678901");

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Field == "name");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenCpfIsEmpty()
    {
        var request = new CreateUserRequest("John", "");

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Field == "cpf");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenCpfIsNotElevenDigits()
    {
        var request = new CreateUserRequest("John", "12345");

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Field == "cpf");
    }
}
