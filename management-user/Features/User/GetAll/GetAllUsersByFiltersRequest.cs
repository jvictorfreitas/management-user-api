namespace feature.user;

public record GetAllUsersByFiltersRequest(string? Name, string? Email, int? Page, int? PageSize);
