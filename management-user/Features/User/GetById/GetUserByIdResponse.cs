namespace feature.user;

public record GetUserByIdResponse(
    string Name,
    string Cpf,
    short AccountStatus,
    string AccountStatusDescription
);
