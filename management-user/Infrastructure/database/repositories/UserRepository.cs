using domain;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.database;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<User> Add(User user, CancellationToken cancellationToken)
    {
        UserEntity? entity = new(
            user.Id,
            user.Name,
            user.Cpf,
            (short)user.AccountStatus,
            DateTime.UtcNow
        );

        await _appDbContext.Users.AddAsync(entity, cancellationToken);

        return entity.ToDomain();
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
    {
        UserEntity? entity = await _appDbContext.Users.FirstOrDefaultAsync(
            x => x.GuidId == id,
            cancellationToken
        );

        if (entity == null)
            return false;

        await _appDbContext.Users.AddAsync(entity, cancellationToken);

        return true;
    }

    public async Task<List<User>> GetAllByFilter(
        string? name,
        string? cpf,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        IQueryable<UserEntity> query = _appDbContext.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{name}%"));
        }

        if (!string.IsNullOrWhiteSpace(cpf))
        {
            query = query.Where(x => EF.Functions.ILike(x.Cpf, $"%{cpf}%"));
        }

        int skip = (page - 1) * pageSize;

        List<UserEntity> entities = await query
            .OrderBy(x => x.Name)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return entities.Select(entity => entity.ToDomain()).ToList();
    }

    public async Task<User> GetById(Guid id, CancellationToken cancellationToken)
    {
        UserEntity? entity = await _appDbContext
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.GuidId == id, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException($"User {id} not found");

        return entity.ToDomain();
    }

    public async Task<User> Update(User user, CancellationToken cancellationToken)
    {
        UserEntity? entity = await _appDbContext.Users.FirstOrDefaultAsync(
            x => x.GuidId == user.Id,
            cancellationToken
        );

        if (entity == null)
            throw new KeyNotFoundException($"User {user.Id} not found");

        entity.SetValuesForUpdateFromDomain(user);

        return entity.ToDomain();
    }
}
