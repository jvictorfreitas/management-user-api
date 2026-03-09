namespace feature.user;

public record CreateUserResponse(
    string Name,
    string Cpf,
    short AccountStatus,
    string AccountStatusDescription
);
