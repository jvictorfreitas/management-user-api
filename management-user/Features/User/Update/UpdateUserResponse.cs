namespace feature.user;

public record UpdateUserResponse(
    string Name,
    string Cpf,
    short AccountStatus,
    string AccountStatusDescription
);
