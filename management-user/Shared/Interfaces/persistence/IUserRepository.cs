using domain;

namespace Shared;

public interface IUserRepository
{
    Task<User> Add(User user, CancellationToken cancellationToken);
    Task<User> Update(User user, CancellationToken cancellationToken);
    Task<bool> Delete(Guid id, CancellationToken cancellationToken);
    Task<User> GetById(Guid id, CancellationToken cancellationToken);
    Task<List<User>> GetAllByFilter(
        string? name,
        string? Cpf,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    );
}
