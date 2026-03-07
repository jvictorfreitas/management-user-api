namespace feature.user;

public record GetUserByIdResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
}
