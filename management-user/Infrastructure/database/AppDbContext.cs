using Microsoft.EntityFrameworkCore;

namespace Infrastructure.database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();

            entity.Property(x => x.GuidId).IsRequired();
            entity.HasIndex(x => x.GuidId);

            entity.Property(x => x.Name).IsRequired();

            entity.Property(x => x.Cpf).IsRequired();

            entity.Property(x => x.AccountStatus).IsRequired();

            entity.Property(x => x.CreatedAt).HasColumnName("created_at");
        });
    }
}
