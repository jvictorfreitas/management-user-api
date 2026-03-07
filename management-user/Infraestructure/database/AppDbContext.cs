using Microsoft.EntityFrameworkCore;
using infrastructure.entities;

namespace infrastructure.database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired();

            entity.Property(x => x.Email)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .HasColumnName("created_at");
        });
    }
}