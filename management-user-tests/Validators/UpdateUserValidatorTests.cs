using feature.user;

namespace management_user_tests.Validators;

public class UpdateUserValidatorTests
{
    private readonly UpdateUserValidator _validator = new();

    [Fact]
    public void Validate_ShouldReturnValid_WhenAllFieldsAreCorrect()
    {
        var request = new UpdateUserRequest("John Doe", "12345678901", 0);

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenNameIsEmpty()
    {
        var request = new UpdateUserRequest("", "12345678901", 0);

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Field == "name");
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenCpfIsNotElevenDigits()
    {
        var request = new UpdateUserRequest("John", "12345", 0);

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Field == "cpf");
    }
}
