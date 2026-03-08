using domain;

namespace Shared;

public interface IUserRepository
{
    Task<User> Add(User user);
    Task<User> Update(User user);
    Task<bool> Delete(Guid id);
    Task<User> GetById(Guid id);
    Task<List<User>> GetAllByFilter(string? name, string? Cpf, int page, int pageSize);
}
