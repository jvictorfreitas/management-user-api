using domain;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infraestructure.database;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<User> Add(User user)
    {
        UserEntity? entity = UserMapper.ToEntity(user);

        _appDbContext.Users.Add(entity);

        await _appDbContext.SaveChangesAsync();

        return UserMapper.ToDomain(entity);
    }

    public async Task<bool> Delete(Guid id)
    {
        UserEntity? entity = await _appDbContext.Users.FirstOrDefaultAsync(x => x.GuidId == id);

        if (entity == null)
            return false;

        _appDbContext.Users.Remove(entity);

        await _appDbContext.SaveChangesAsync();

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

        return entities.Select(UserMapper.ToDomain).ToList();
    }

    public async Task<User> GetById(Guid id)
    {
        UserEntity? entity = await _appDbContext
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.GuidId == id);

        if (entity == null)
            throw new KeyNotFoundException($"User {id} not found");

        return UserMapper.ToDomain(entity);
    }

    public async Task<User> Update(User user)
    {
        UserEntity? entity = await _appDbContext.Users.FirstOrDefaultAsync(x =>
            x.GuidId == user.Id
        );

        if (entity == null)
            throw new KeyNotFoundException($"User {user.Id} not found");

        entity.Name = user.Name;
        entity.Cpf = user.Cpf;

        await _appDbContext.SaveChangesAsync();

        return UserMapper.ToDomain(entity);
    }
}
