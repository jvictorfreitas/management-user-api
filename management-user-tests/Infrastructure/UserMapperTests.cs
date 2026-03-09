using domain;
using Infrastructure.database;

namespace management_user_tests.Infrastructure;

public class UserMapperTests
{
    [Fact]
    public void ToEntity_ShouldMapAllFields()
    {
        var user = new User(
            Guid.NewGuid(),
            "John Doe",
            "12345678901",
            AccountStatus.Active
        );

        var entity = UserMapper.ToEntity(user);

        Assert.Equal(user.Id, entity.GuidId);
        Assert.Equal(user.Name, entity.Name);
        Assert.Equal(user.Cpf, entity.Cpf);
        Assert.Equal((short)user.AccountStatus, entity.AccountStatus);
    }

    [Fact]
    public void ToDomain_ShouldMapAllFields()
    {
        var entity = new UserEntity
        {
            GuidId = Guid.NewGuid(),
            Name = "Jane Doe",
            Cpf = "98765432100",
            AccountStatus = (short)AccountStatus.Inactive,
            CreatedAt = DateTime.UtcNow,
        };

        var user = UserMapper.ToDomain(entity);

        Assert.Equal(entity.GuidId, user.Id);
        Assert.Equal(entity.Name, user.Name);
        Assert.Equal(entity.Cpf, user.Cpf);
        Assert.Equal(AccountStatus.Inactive, user.AccountStatus);
    }

    [Fact]
    public void ToEntity_ThenToDomain_ShouldPreserveData()
    {
        var original = new User(
            Guid.NewGuid(),
            "John Doe",
            "12345678901",
            AccountStatus.Active
        );

        var entity = UserMapper.ToEntity(original);
        var restored = UserMapper.ToDomain(entity);

        Assert.Equal(original.Id, restored.Id);
        Assert.Equal(original.Name, restored.Name);
        Assert.Equal(original.Cpf, restored.Cpf);
        Assert.Equal(original.AccountStatus, restored.AccountStatus);
    }
}
