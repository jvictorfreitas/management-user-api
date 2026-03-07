namespace feature.user;

public record GetAllUsersByFiltersResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
}
