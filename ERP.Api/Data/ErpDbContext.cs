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
    public DbSet<MillProduction> MillProductions => Set<MillProduction>();
    public DbSet<MillTest> MillTests => Set<MillTest>();
    public DbSet<FabricShipment> FabricShipments => Set<FabricShipment>();
    public DbSet<FabricReceiving> FabricReceivings => Set<FabricReceiving>();
    public DbSet<RollReceiving> RollReceivings => Set<RollReceiving>();
    public DbSet<FourPointInspection> FourPointInspections => Set<FourPointInspection>();
    public DbSet<InternalTest> InternalTests => Set<InternalTest>();
    public DbSet<ShadeMatch> ShadeMatches => Set<ShadeMatch>();
    public DbSet<InlineQuality> InlineQualities => Set<InlineQuality>();
    public DbSet<Lot> Lots => Set<Lot>();
    public DbSet<CatalogValue> CatalogValues => Set<CatalogValue>();

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
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

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
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Module).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
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
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
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
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

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
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
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
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
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
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

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
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
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

        modelBuilder.Entity<MillProduction>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Supplier).HasMaxLength(100);
            entity.Property(e => e.FabricComponent).HasMaxLength(50);
            entity.Property(e => e.Style).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.PlannedQuantity).HasPrecision(18, 4);
            entity.Property(e => e.ProducedQuantity).HasPrecision(18, 4);
            // Columna calculada por SQL — nunca se desincroniza
            entity.Property(e => e.CompletionPercentage)
                  .HasComputedColumnSql("CAST((CASE WHEN [PlannedQuantity] = 0 THEN 0 ELSE ([ProducedQuantity] / [PlannedQuantity]) * 100 END) AS decimal(18,4))");
            entity.Property(e => e.LotNumber).HasMaxLength(50);
            entity.Property(e => e.RollQuantity).HasPrecision(18, 4);
            entity.Property(e => e.YardageOrQty).HasPrecision(18, 4);
            entity.Property(e => e.Weight).HasPrecision(18, 4);
            entity.Property(e => e.StartDate).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.DataOwner).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas paginadas/filtros frecuentes
            entity.HasIndex(e => new { e.FGPOId, e.FabricPOId });
            entity.HasIndex(e => e.LotNumber);
            entity.HasIndex(e => e.Status);

            entity.HasOne(m => m.FabricPO)
                  .WithMany()
                  .HasForeignKey(m => m.FabricPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.FGPO)
                  .WithMany()
                  .HasForeignKey(m => m.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Lot)
                  .WithMany()
                  .HasForeignKey(m => m.LotId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<MillTest>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Supplier).HasMaxLength(100);
            entity.Property(e => e.LotNumber).HasMaxLength(50);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.RollQty).HasPrecision(18, 4);
            entity.Property(e => e.ActualWidth).HasPrecision(18, 4);
            entity.Property(e => e.ActualGSM).HasPrecision(18, 4);
            entity.Property(e => e.LengthShrinkagePercentage).HasPrecision(18, 4);
            entity.Property(e => e.WidthShrinkagePercentage).HasPrecision(18, 4);
            entity.Property(e => e.TorquePercentage).HasPrecision(18, 4);
            entity.Property(e => e.BowingPercentage).HasPrecision(18, 4);
            entity.Property(e => e.SkewingPercentage).HasPrecision(18, 4);
            entity.Property(e => e.Colorfastness).HasMaxLength(100);
            entity.Property(e => e.WashAppearance).HasMaxLength(100);
            entity.Property(e => e.HandFeel).HasMaxLength(100);
            entity.Property(e => e.TestDate).IsRequired();
            entity.Property(e => e.TestedBy).HasMaxLength(100);
            entity.Property(e => e.TestResult).HasMaxLength(50);
            entity.Property(e => e.ReportLink).HasMaxLength(500);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => new { e.FGPOId, e.FabricPOId });
            entity.HasIndex(e => e.LotNumber);
            entity.HasIndex(e => e.TestResult);

            entity.HasOne(m => m.FabricPO)
                  .WithMany()
                  .HasForeignKey(m => m.FabricPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.FGPO)
                  .WithMany()
                  .HasForeignKey(m => m.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Lot)
                  .WithMany()
                  .HasForeignKey(m => m.LotId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<FabricShipment>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ShipmentNumber).HasMaxLength(50);
            entity.HasIndex(e => e.ShipmentNumber).IsUnique();
            entity.Property(e => e.Supplier).HasMaxLength(100);
            entity.Property(e => e.LotNumber).HasMaxLength(50);
            entity.Property(e => e.RollQty).HasPrecision(18, 4);
            entity.Property(e => e.ShippedQuantity).HasPrecision(18, 4);
            entity.Property(e => e.UOM).HasMaxLength(20);
            entity.Property(e => e.ShippedWeight).HasPrecision(18, 4);
            entity.Property(e => e.PackingList).HasMaxLength(200);
            entity.Property(e => e.InvoiceNumber).HasMaxLength(100);
            entity.Property(e => e.ContainerAWB).HasMaxLength(100);
            entity.Property(e => e.ShippingMethod).HasMaxLength(100);
            entity.Property(e => e.ETD).IsRequired();
            entity.Property(e => e.ETA).IsRequired();
            entity.Property(e => e.ShipmentStatus).HasMaxLength(50);
            // Columnas calculadas por SQL — nunca se desincronizan
            entity.Property(e => e.InTransitQuantity)
                  .HasComputedColumnSql("CAST((CASE WHEN [DeliveredToTexnicaDate] IS NULL THEN [ShippedQuantity] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.RemainingToDeliver)
                  .HasComputedColumnSql("CAST((CASE WHEN [DeliveredToTexnicaDate] IS NULL THEN [ShippedQuantity] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.DataOwner).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => new { e.FGPOId, e.FabricPOId });
            entity.HasIndex(e => e.LotNumber);
            entity.HasIndex(e => e.ShipmentStatus);

            entity.HasOne(s => s.FabricPO)
                  .WithMany()
                  .HasForeignKey(s => s.FabricPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.FGPO)
                  .WithMany()
                  .HasForeignKey(s => s.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Lot)
                  .WithMany()
                  .HasForeignKey(s => s.LotId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<FabricReceiving>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ReceivingNumber).HasMaxLength(50);
            entity.HasIndex(e => e.ReceivingNumber).IsUnique();
            entity.Property(e => e.ShipmentNumber).HasMaxLength(50);
            entity.Property(e => e.Supplier).HasMaxLength(100);
            entity.Property(e => e.PackingListQty).HasPrecision(18, 4);
            entity.Property(e => e.ActualReceivedQty).HasPrecision(18, 4);
            // Columnas calculadas por SQL — varianzas automáticas
            entity.Property(e => e.ReceivingVariance)
                  .HasComputedColumnSql("CAST(([ActualReceivedQty] - [PackingListQty]) AS decimal(18,4))");
            entity.Property(e => e.ReceivingShortage)
                  .HasComputedColumnSql("CAST((CASE WHEN [PackingListQty] > [ActualReceivedQty] THEN [PackingListQty] - [ActualReceivedQty] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.ReceivingOverQty)
                  .HasComputedColumnSql("CAST((CASE WHEN [ActualReceivedQty] > [PackingListQty] THEN [ActualReceivedQty] - [PackingListQty] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.ExpectedRolls);
            entity.Property(e => e.ReceivedRolls);
            entity.Property(e => e.MissingRolls)
                  .HasComputedColumnSql("CAST((CASE WHEN [ExpectedRolls] > [ReceivedRolls] THEN [ExpectedRolls] - [ReceivedRolls] ELSE 0 END) AS int)");
            entity.Property(e => e.ReceivingStatus).HasMaxLength(50);
            entity.Property(e => e.WarehouseLocation).HasMaxLength(100);
            entity.Property(e => e.ReceivedBy).HasMaxLength(100);
            entity.Property(e => e.DataOwner).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => new { e.FGPOId, e.FabricPOId });
            entity.HasIndex(e => e.ShipmentNumber);
            entity.HasIndex(e => e.ReceivingStatus);

            entity.HasOne(r => r.FabricPO)
                  .WithMany()
                  .HasForeignKey(r => r.FabricPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.FGPO)
                  .WithMany()
                  .HasForeignKey(r => r.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RollReceiving>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ReceivingNumber).HasMaxLength(50);
            entity.Property(e => e.Supplier).HasMaxLength(100);
            entity.Property(e => e.LotNumber).HasMaxLength(50);
            entity.Property(e => e.RollNumber).HasMaxLength(50);
            entity.Property(e => e.SupplierRollNumber).HasMaxLength(50);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.GrossWeight).HasPrecision(18, 4);
            entity.Property(e => e.NetWeight).HasPrecision(18, 4);
            entity.Property(e => e.ActualYardage).HasPrecision(18, 4);
            entity.Property(e => e.ActualWidth).HasPrecision(18, 4);
            entity.Property(e => e.ActualGSM).HasPrecision(18, 4);
            entity.Property(e => e.ShadeGroup).HasMaxLength(50);
            entity.Property(e => e.DamagedQty).HasPrecision(18, 4);
            entity.Property(e => e.Condition).HasMaxLength(100);
            entity.Property(e => e.WarehouseLocation).HasMaxLength(100);
            entity.Property(e => e.ReceivedDate).IsRequired();
            entity.Property(e => e.DataOwner).HasMaxLength(100);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => new { e.FGPOId, e.FabricPOId });
            entity.HasIndex(e => e.LotNumber);
            entity.HasIndex(e => e.RollNumber);

            entity.HasOne(r => r.Receiving)
                  .WithMany()
                  .HasForeignKey(r => r.ReceivingId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.FabricPO)
                  .WithMany()
                  .HasForeignKey(r => r.FabricPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.FGPO)
                  .WithMany()
                  .HasForeignKey(r => r.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Lot)
                  .WithMany()
                  .HasForeignKey(r => r.LotId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<FourPointInspection>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.InspectionDate).IsRequired();
            entity.Property(e => e.ReceivingNumber).HasMaxLength(50);
            entity.Property(e => e.LotNumber).HasMaxLength(50);
            entity.Property(e => e.RollNumber).HasMaxLength(50);
            entity.Property(e => e.Width).HasPrecision(18, 4);
            entity.Property(e => e.InspectedLength).HasPrecision(18, 4);
            entity.Property(e => e.Points1).IsRequired();
            entity.Property(e => e.Points2).IsRequired();
            entity.Property(e => e.Points3).IsRequired();
            entity.Property(e => e.Points4).IsRequired();
            // Columnas calculadas por SQL — TotalPoints y puntos por 100 yd²
            entity.Property(e => e.TotalPoints)
                  .HasComputedColumnSql("CAST(([Points1] + (2 * [Points2]) + (3 * [Points3]) + (4 * [Points4])) AS int)");
            entity.Property(e => e.PointsPer100SqYd)
                  .HasComputedColumnSql("CAST((CASE WHEN [Width] = 0 OR [InspectedLength] = 0 THEN 0 ELSE (([Points1] + (2 * [Points2]) + (3 * [Points3]) + (4 * [Points4])) * 3600.0) / ([Width] * [InspectedLength]) END) AS decimal(18,4))");
            entity.Property(e => e.MaxAllowed).HasPrecision(18, 4);
            entity.Property(e => e.AcceptedQty).IsRequired();
            entity.Property(e => e.RejectedQty).IsRequired();
            entity.Property(e => e.HoldQty).IsRequired();
            entity.Property(e => e.Result).HasMaxLength(50);
            entity.Property(e => e.Inspector).HasMaxLength(100);
            entity.Property(e => e.ReportLink).HasMaxLength(500);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => new { e.FGPOId, e.FabricPOId });
            entity.HasIndex(e => e.RollNumber);
            entity.HasIndex(e => e.Result);
            entity.HasIndex(e => e.ReceivingId);

            entity.HasOne(i => i.Receiving)
                  .WithMany()
                  .HasForeignKey(i => i.ReceivingId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(i => i.FabricPO)
                  .WithMany()
                  .HasForeignKey(i => i.FabricPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.FGPO)
                  .WithMany()
                  .HasForeignKey(i => i.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.Lot)
                  .WithMany()
                  .HasForeignKey(i => i.LotId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<InternalTest>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.TestDate).IsRequired();
            entity.Property(e => e.Supplier).HasMaxLength(100);
            entity.Property(e => e.LotNumber).HasMaxLength(50);
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.ActualWidth).HasPrecision(18, 4);
            entity.Property(e => e.SpecimenAreaCm2).HasPrecision(18, 4);
            entity.Property(e => e.WeightBeforeG).HasPrecision(18, 4);
            entity.Property(e => e.WeightAfterG).HasPrecision(18, 4);
            entity.Property(e => e.TargetGSM).HasPrecision(18, 4);
            // Columnas calculadas por SQL
            entity.Property(e => e.GsmBefore)
                  .HasComputedColumnSql("CAST((CASE WHEN [SpecimenAreaCm2] = 0 THEN 0 ELSE ([WeightBeforeG] / ([SpecimenAreaCm2] / 10000.0)) END) AS decimal(18,4))");
            entity.Property(e => e.GsmAfter)
                  .HasComputedColumnSql("CAST((CASE WHEN [SpecimenAreaCm2] = 0 THEN 0 ELSE ([WeightAfterG] / ([SpecimenAreaCm2] / 10000.0)) END) AS decimal(18,4))");
            entity.Property(e => e.GsmVariancePct)
                  .HasComputedColumnSql("CAST((CASE WHEN [TargetGSM] = 0 OR [SpecimenAreaCm2] = 0 THEN 0 ELSE ((([WeightAfterG] / ([SpecimenAreaCm2] / 10000.0)) - [TargetGSM]) / [TargetGSM]) * 100 END) AS decimal(18,4))");
            entity.Property(e => e.LengthBefore).HasPrecision(18, 4);
            entity.Property(e => e.LengthAfter).HasPrecision(18, 4);
            entity.Property(e => e.LengthShrinkagePct)
                  .HasComputedColumnSql("CAST((CASE WHEN [LengthBefore] = 0 THEN 0 ELSE (([LengthBefore] - [LengthAfter]) / [LengthBefore]) * 100 END) AS decimal(18,4))");
            entity.Property(e => e.WidthBefore).HasPrecision(18, 4);
            entity.Property(e => e.WidthAfter).HasPrecision(18, 4);
            entity.Property(e => e.WidthShrinkagePct)
                  .HasComputedColumnSql("CAST((CASE WHEN [WidthBefore] = 0 THEN 0 ELSE (([WidthBefore] - [WidthAfter]) / [WidthBefore]) * 100 END) AS decimal(18,4))");
            entity.Property(e => e.TorquePct).HasPrecision(18, 4);
            entity.Property(e => e.BowingPct).HasPrecision(18, 4);
            entity.Property(e => e.SkewingPct).HasPrecision(18, 4);
            entity.Property(e => e.ShadeResult).HasMaxLength(50);
            entity.Property(e => e.WashAppearance).HasMaxLength(100);
            entity.Property(e => e.HandFeel).HasMaxLength(100);
            entity.Property(e => e.TestResult).HasMaxLength(50);
            entity.Property(e => e.TestedBy).HasMaxLength(100);
            entity.Property(e => e.ApprovedBy).HasMaxLength(100);
            entity.Property(e => e.ReportLink).HasMaxLength(500);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => new { e.FGPOId, e.FabricPOId });
            entity.HasIndex(e => e.LotNumber);
            entity.HasIndex(e => e.TestResult);

            entity.HasOne(t => t.FabricPO)
                  .WithMany()
                  .HasForeignKey(t => t.FabricPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.FGPO)
                  .WithMany()
                  .HasForeignKey(t => t.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Lot)
                  .WithMany()
                  .HasForeignKey(t => t.LotId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ShadeMatch>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ReviewDate).IsRequired();
            entity.Property(e => e.BodyFabricLot).HasMaxLength(50);
            entity.Property(e => e.RibLot).HasMaxLength(50);
            entity.Property(e => e.ShoulderTapeLot).HasMaxLength(50);
            entity.Property(e => e.BodyShadeGroup).HasMaxLength(50);
            entity.Property(e => e.RibShadeGroup).HasMaxLength(50);
            entity.Property(e => e.TapeShadeGroup).HasMaxLength(50);
            entity.Property(e => e.BodyVsRib).HasMaxLength(100);
            entity.Property(e => e.BodyVsTape).HasMaxLength(100);
            entity.Property(e => e.LightSource).HasMaxLength(50);
            entity.Property(e => e.BeforeWashResult).HasMaxLength(100);
            entity.Property(e => e.AfterWashResult).HasMaxLength(100);
            entity.Property(e => e.OverallResult).HasMaxLength(50);
            entity.Property(e => e.ApprovedBy).HasMaxLength(100);
            entity.Property(e => e.ReportLink).HasMaxLength(500);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.OverallResult);
            entity.HasIndex(e => e.BodyFabricLot);

            entity.HasOne(s => s.FGPO)
                  .WithMany()
                  .HasForeignKey(s => s.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<InlineQuality>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.InspectionDate).IsRequired();
            entity.Property(e => e.Time).HasMaxLength(20);
            entity.Property(e => e.Line).HasMaxLength(50);
            entity.Property(e => e.Operation).HasMaxLength(100);
            entity.Property(e => e.Operator).HasMaxLength(100);
            entity.Property(e => e.CheckedQty).IsRequired();
            entity.Property(e => e.CriticalDefects).IsRequired();
            entity.Property(e => e.MajorDefects).IsRequired();
            entity.Property(e => e.MinorDefects).IsRequired();
            // Columnas calculadas por SQL — totales y porcentajes automáticos
            entity.Property(e => e.TotalDefects)
                  .HasComputedColumnSql("CAST(([CriticalDefects] + [MajorDefects] + [MinorDefects]) AS int)");
            entity.Property(e => e.DhuPct)
                  .HasComputedColumnSql("CAST((CASE WHEN [CheckedQty] = 0 THEN 0 ELSE (([CriticalDefects] + [MajorDefects] + [MinorDefects]) / CAST([CheckedQty] AS decimal(18,4))) * 100 END) AS decimal(18,4))");
            entity.Property(e => e.DefectivePieces).IsRequired();
            entity.Property(e => e.DefectiveRatePct)
                  .HasComputedColumnSql("CAST((CASE WHEN [CheckedQty] = 0 THEN 0 ELSE ([DefectivePieces] / CAST([CheckedQty] AS decimal(18,4))) * 100 END) AS decimal(18,4))");
            entity.Property(e => e.MaxAllowed).HasPrecision(18, 4);
            // Result calculado: Failed si hay defectos críticos O Defective Rate% > MaxAllowed (según Excel), si no Passed
            entity.Property(e => e.Result)
                  .HasComputedColumnSql("CAST((CASE WHEN [CheckedQty] = 0 THEN 'Pending' WHEN [CriticalDefects] > 0 OR ([DefectivePieces] / CAST([CheckedQty] AS decimal(18,4))) * 100 > [MaxAllowed] THEN 'Failed' ELSE 'Passed' END) AS nvarchar(50))");
            entity.Property(e => e.Inspector).HasMaxLength(100);
            entity.Property(e => e.ImmediateCorrection).HasMaxLength(1000);
            entity.Property(e => e.RootCause).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.Line);
            entity.HasIndex(e => e.Result);

            entity.HasOne(i => i.FGPO)
                  .WithMany()
                  .HasForeignKey(i => i.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Lot>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.LotNumber).HasMaxLength(50);
            entity.HasIndex(e => e.LotNumber).IsUnique();
            entity.Property(e => e.ProducedQuantity).HasPrecision(18, 4);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => new { e.FabricPOId, e.FGPOId });

            entity.HasOne(l => l.FabricPO)
                  .WithMany()
                  .HasForeignKey(l => l.FabricPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(l => l.FGPO)
                  .WithMany()
                  .HasForeignKey(l => l.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CatalogValue>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Value).HasMaxLength(100);
            entity.HasIndex(e => new { e.Type, e.Value }).IsUnique();
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Seed de catálogos maestros
            entity.HasData(
                // UOM
                new CatalogValue { ID = 1, Type = "UOM", Value = "Yards" },
                new CatalogValue { ID = 2, Type = "UOM", Value = "Meters" },
                new CatalogValue { ID = 3, Type = "UOM", Value = "Kilograms" },
                new CatalogValue { ID = 4, Type = "UOM", Value = "Pounds" },
                new CatalogValue { ID = 5, Type = "UOM", Value = "Rolls" },
                new CatalogValue { ID = 6, Type = "UOM", Value = "Pieces" },
                // Fabric Component
                new CatalogValue { ID = 7, Type = "FabricComponent", Value = "Body Fabric" },
                new CatalogValue { ID = 8, Type = "FabricComponent", Value = "Rib" },
                new CatalogValue { ID = 9, Type = "FabricComponent", Value = "Shoulder Tape" },
                new CatalogValue { ID = 10, Type = "FabricComponent", Value = "Neck Tape" },
                new CatalogValue { ID = 11, Type = "FabricComponent", Value = "Pocketing" },
                new CatalogValue { ID = 12, Type = "FabricComponent", Value = "Other" },
                // Production Status
                new CatalogValue { ID = 13, Type = "ProductionStatus", Value = "Not Started" },
                new CatalogValue { ID = 14, Type = "ProductionStatus", Value = "Pending" },
                new CatalogValue { ID = 15, Type = "ProductionStatus", Value = "In Progress" },
                new CatalogValue { ID = 16, Type = "ProductionStatus", Value = "Partially Completed" },
                new CatalogValue { ID = 17, Type = "ProductionStatus", Value = "Completed" },
                new CatalogValue { ID = 18, Type = "ProductionStatus", Value = "On Hold" },
                new CatalogValue { ID = 19, Type = "ProductionStatus", Value = "Cancelled" },
                // Test Result
                new CatalogValue { ID = 20, Type = "TestResult", Value = "Pending" },
                new CatalogValue { ID = 21, Type = "TestResult", Value = "Testing" },
                new CatalogValue { ID = 22, Type = "TestResult", Value = "Passed" },
                new CatalogValue { ID = 23, Type = "TestResult", Value = "Conditionally Passed" },
                new CatalogValue { ID = 24, Type = "TestResult", Value = "Failed" },
                // Shipment Status
                new CatalogValue { ID = 25, Type = "ShipmentStatus", Value = "Planned" },
                new CatalogValue { ID = 26, Type = "ShipmentStatus", Value = "Booking Confirmed" },
                new CatalogValue { ID = 27, Type = "ShipmentStatus", Value = "Exported" },
                new CatalogValue { ID = 28, Type = "ShipmentStatus", Value = "In Transit" },
                new CatalogValue { ID = 29, Type = "ShipmentStatus", Value = "Delivered" },
                new CatalogValue { ID = 30, Type = "ShipmentStatus", Value = "Cancelled" },
                // PO Status
                new CatalogValue { ID = 31, Type = "POStatus", Value = "Not Started" },
                new CatalogValue { ID = 32, Type = "POStatus", Value = "Pending" },
                new CatalogValue { ID = 33, Type = "POStatus", Value = "In Progress" },
                new CatalogValue { ID = 34, Type = "POStatus", Value = "Partially Completed" },
                new CatalogValue { ID = 35, Type = "POStatus", Value = "Completed" },
                new CatalogValue { ID = 36, Type = "POStatus", Value = "Approved" },
                new CatalogValue { ID = 37, Type = "POStatus", Value = "Conditionally Approved" },
                new CatalogValue { ID = 38, Type = "POStatus", Value = "Rejected" },
                new CatalogValue { ID = 39, Type = "POStatus", Value = "On Hold" },
                new CatalogValue { ID = 40, Type = "POStatus", Value = "Closed" },
                new CatalogValue { ID = 41, Type = "POStatus", Value = "Cancelled" },
                // Receiving Status
                new CatalogValue { ID = 42, Type = "ReceivingStatus", Value = "Pending" },
                new CatalogValue { ID = 43, Type = "ReceivingStatus", Value = "Partially Received" },
                new CatalogValue { ID = 44, Type = "ReceivingStatus", Value = "Fully Received" },
                new CatalogValue { ID = 45, Type = "ReceivingStatus", Value = "Quantity Difference" },
                new CatalogValue { ID = 46, Type = "ReceivingStatus", Value = "Rejected" }
            );
        });

        base.OnModelCreating(modelBuilder);
    }
}
