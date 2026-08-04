using Microsoft.EntityFrameworkCore;
using ERP.Api.Domain;

namespace ERP.Api.Data;

public class ErpDbContext : DbContext
{
    public ErpDbContext(DbContextOptions<ErpDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Fgpo> Fgpos => Set<Fgpo>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Factory> Factories => Set<Factory>();
    public DbSet<FabricRequirement> FabricRequirements => Set<FabricRequirement>();
    public DbSet<FabricPO> FabricPOs => Set<FabricPO>();
    public DbSet<FabricPOFgpo> FabricPOFgpos => Set<FabricPOFgpo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.UserName).IsRequired();
            entity.Property(e => e.UserEmail).IsRequired();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.MustChangePassword).HasDefaultValue(true);

            entity.HasOne(u => u.Role)
                  .WithMany(r => r.Users)
                  .HasForeignKey(u => u.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Module).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.ID);

            entity.HasOne(rp => rp.Role)
                  .WithMany(r => r.RolePermissions)
                  .HasForeignKey(rp => rp.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rp => rp.Permission)
                  .WithMany(p => p.RolePermissions)
                  .HasForeignKey(rp => rp.PermissionId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique();
        });

        modelBuilder.Entity<Fgpo>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.FGPONumber).HasMaxLength(50);
            entity.HasIndex(e => e.FGPONumber).IsUnique();
            entity.Property(e => e.TemporaryNumber).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Style).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.OrderQuantity).IsRequired();
            entity.Property(e => e.DeliveryDate).IsRequired();
            entity.Property(e => e.InTransitQty).HasPrecision(18, 4);
            entity.Property(e => e.ReceivedQty).HasPrecision(18, 4);
            entity.Property(e => e.TotalShippedQty).HasPrecision(18, 4);
            entity.Property(e => e.ShipmentVariance).HasPrecision(18, 4);
            entity.Property(e => e.PendingToShip).HasPrecision(18, 4);
            entity.Property(e => e.OvershipmentQty).HasPrecision(18, 4);
            entity.Property(e => e.ProducedQty).HasPrecision(18, 4);
            entity.Property(e => e.ProductionVariance).HasPrecision(18, 4);
            entity.Property(e => e.PendingProduction).HasPrecision(18, 4);
            entity.Property(e => e.OverproductionQty).HasPrecision(18, 4);
            entity.Property(e => e.DataOwner).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(f => f.Customer)
                  .WithMany()
                  .HasForeignKey(f => f.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Contact).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<Factory>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Contact).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<FabricRequirement>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Style).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.FabricComponent).HasMaxLength(50);
            entity.Property(e => e.FabricDescription).HasMaxLength(200);
            entity.Property(e => e.Composition).HasMaxLength(200);
            entity.Property(e => e.GSM).HasPrecision(18, 4);
            entity.Property(e => e.RequiredWidth).HasMaxLength(50);
            entity.Property(e => e.UOM).HasMaxLength(20);
            entity.Property(e => e.OrderQuantity).HasPrecision(18, 4);
            entity.Property(e => e.ApprovedYield).HasPrecision(18, 4);
            entity.Property(e => e.GrossRequirement).HasPrecision(18, 4);
            entity.Property(e => e.AllowancePercentage).HasPrecision(18, 4);
            entity.Property(e => e.AllowanceQty).HasPrecision(18, 4);
            entity.Property(e => e.AvailableInventory).HasPrecision(18, 4);
            entity.Property(e => e.NetPurchaseRequirement).HasPrecision(18, 4);
            entity.Property(e => e.RequiredDate).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.DataOwner).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);

            entity.HasOne(f => f.FGPO)
                  .WithMany()
                  .HasForeignKey(f => f.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FabricPO>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.FabricPONumber).HasMaxLength(50);
            entity.HasIndex(e => e.FabricPONumber).IsUnique();
            entity.Property(e => e.Supplier).HasMaxLength(100);
            entity.Property(e => e.FabricMill).HasMaxLength(100);
            entity.Property(e => e.FabricComponent).HasMaxLength(50);
            entity.Property(e => e.OrderedQuantity).HasPrecision(18, 4);
            entity.Property(e => e.UOM).HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 4);
            entity.Property(e => e.POAmount).HasPrecision(18, 4);
            entity.Property(e => e.OrderDate).IsRequired();
            entity.Property(e => e.RequiredCompletion).IsRequired();
            entity.Property(e => e.POStatus).HasMaxLength(50);
            entity.Property(e => e.PurchaseOwner).HasMaxLength(100);
            entity.Property(e => e.ApprovedBy).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        modelBuilder.Entity<FabricPOFgpo>(entity =>
        {
            // FabricPOFgpo es una entidad propia con su propia llave primaria
            entity.HasKey(pf => pf.ID);
            entity.HasIndex(pf => new { pf.FabricPOId, pf.FGPOId }).IsUnique();

            // FabricPOFgpo - FabricPO relationship
            entity.HasOne(pf => pf.FabricPO)
                  .WithMany(p => p.FabricPOFgpos)
                  .HasForeignKey(pf => pf.FabricPOId)
                  .OnDelete(DeleteBehavior.Cascade);

            // FabricPOFgpo - Fgpo relationship
            entity.HasOne(pf => pf.FGPO)
                  .WithMany(f => f.FabricPOFgpos)
                  .HasForeignKey(pf => pf.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Style y Color viven en la tabla puente porque cada FGPO cubierto
            // por el mismo Fabric PO puede tener su propio estilo/color.
            entity.Property(pf => pf.Style).HasMaxLength(100);
            entity.Property(pf => pf.Color).HasMaxLength(50);
            entity.Property(pf => pf.AllocatedQuantity).HasPrecision(18, 4);
        });

        base.OnModelCreating(modelBuilder);
    }
}
