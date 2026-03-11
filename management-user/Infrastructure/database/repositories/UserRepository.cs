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

    public async Task<User> Add(User user)
    {
        UserEntity? entity = new(
            user.Id,
            user.Name,
            user.Cpf,
            (short)user.AccountStatus,
            DateTime.UtcNow
        );

        _appDbContext.Users.Add(entity);

        return entity.ToDomain();
    }

    public async Task<bool> Delete(Guid id)
    {
        UserEntity? entity = await _appDbContext.Users.FirstOrDefaultAsync(x => x.GuidId == id);

        if (entity == null)
            return false;

        _appDbContext.Users.Remove(entity);

        return true;
    }

    public async Task<List<User>> GetAllByFilter(string? name, string? cpf, int page, int pageSize)
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
            .ToListAsync();

        return entities.Select(entity => entity.ToDomain()).ToList();
    }

    public async Task<User> GetById(Guid id)
    {
        UserEntity? entity = await _appDbContext
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.GuidId == id);

        if (entity == null)
            throw new KeyNotFoundException($"User {id} not found");

        return entity.ToDomain();
    }

    public async Task<User> Update(User user)
    {
        UserEntity? entity = await _appDbContext.Users.FirstOrDefaultAsync(x =>
            x.GuidId == user.Id
        );

        if (entity == null)
            throw new KeyNotFoundException($"User {user.Id} not found");

        entity.SetValuesForUpdateFromDomain(user);

        return entity.ToDomain();
    }
}
