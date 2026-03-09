namespace feature.user;

public record GetAllUsersByFiltersResponse(
    string Name,
    string Cpf,
    short AccountStatus,
    string AccountStatusDescription
);
