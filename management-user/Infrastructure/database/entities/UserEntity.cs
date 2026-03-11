using domain;

namespace Infrastructure.database;

public class UserEntity
{
    public long Id { get; set; }

    public Guid GuidId { get; set; }

    public string Name { get; set; } = default!;

    public string Cpf { get; set; } = default!;

    public short AccountStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public UserEntity(Guid guidId, string name, string cpf, short accountStatus, DateTime createdAt)
    {
        GuidId = guidId;
        Name = name;
        Cpf = cpf;
        AccountStatus = accountStatus;
        CreatedAt = createdAt;
    }

    public void SetValuesForUpdateFromDomain(User domain)
    {
        GuidId = domain.Id;
        Name = domain.Name;
        Cpf = domain.Cpf;
        AccountStatus = (short)domain.AccountStatus;
    }

    public User ToDomain()
    {
        return new User(GuidId, Name, Cpf, (AccountStatus)AccountStatus);
    }
}
