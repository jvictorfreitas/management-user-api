namespace feature.user;

public record UpdateUserResponse(
    string Name,
    string Cpf,
    short accountStatus,
    string accountStatusDescription
);
