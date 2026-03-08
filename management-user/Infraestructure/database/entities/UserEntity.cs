namespace Infraestructure.database;

public class UserEntity
{
    public long Id { get; set; }

    public Guid GuidId { get; set; }

    public string Name { get; set; } = default!;

    public string Cpf { get; set; } = default!;

    public short AccountStatus { get; set; }

    public DateTime CreatedAt { get; set; }
}
