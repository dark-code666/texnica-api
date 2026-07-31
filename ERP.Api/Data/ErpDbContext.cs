using Microsoft.EntityFrameworkCore;
using ERP.Api.Domain;

namespace ERP.Api.Data;

public class ErpDbContext : DbContext
{
    public ErpDbContext(DbContextOptions<ErpDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    //public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

       modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.UserName).IsRequired();
            entity.Property(e => e.UserEmail).IsRequired();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.Active).HasDefaultValue(true);
        });
        // Configuraci�n de otras entidades...
        base.OnModelCreating(modelBuilder);

    }
}
