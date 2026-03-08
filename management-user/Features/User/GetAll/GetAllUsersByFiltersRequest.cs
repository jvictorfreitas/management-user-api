namespace feature.user;

public record GetAllUsersByFiltersRequest(string? Name, string? Cpf, int Page, int PageSize);
