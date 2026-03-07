namespace feature.user;

public record CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
}
