namespace domain;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public AccountStatus AccountStatus { get; set; }

    public User() { }

    public User(Guid id, string name, string cpf, AccountStatus accountStatus)
    {
        Id = id;
        Name = name;
        Cpf = cpf;
        AccountStatus = accountStatus;
    }

    public User(Guid id, string name, string cpf)
    {
        Id = id;
        Name = name;
        Cpf = cpf;
    }
}
