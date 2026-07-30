using Microsoft.EntityFrameworkCore;
using ERP.Api.Domain;

namespace ERP.Api.Data;

public class ErpDbContext : DbContext
{
    public ErpDbContext(DbContextOptions<ErpDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(x => x.Description)
                .HasMaxLength(500);
            entity.Property(x => x.Price)
                .HasPrecision(18, 2);
            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });
    }
}
