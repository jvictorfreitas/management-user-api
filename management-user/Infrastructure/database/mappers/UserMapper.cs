using domain;

namespace Infrastructure.database;

public static class UserMapper
{
    public static UserEntity ToEntity(User user)
    {
        return new UserEntity
        {
            GuidId = user.Id,
            Name = user.Name,
            Cpf = user.Cpf,
            AccountStatus = (short)user.AccountStatus,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public static User ToDomain(UserEntity entity)
    {
        return new User(
            entity.GuidId,
            entity.Name,
            entity.Cpf,
            (AccountStatus)entity.AccountStatus
        );
    }
}
