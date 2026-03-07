namespace domain;

class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public AccountStatus AccountStatus { get; set; }
}
