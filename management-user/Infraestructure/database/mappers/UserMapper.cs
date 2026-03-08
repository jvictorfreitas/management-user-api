using domain;

namespace Infraestructure.database;

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
        };
    }

    public static User ToDomain(UserEntity entity)
    {
        return new User
        {
            Id = entity.GuidId,
            Name = entity.Name,
            Cpf = entity.Cpf,
            AccountStatus = (AccountStatus)entity.AccountStatus,
        };
    }
}
