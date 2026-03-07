namespace feature.User;

public record CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
}
