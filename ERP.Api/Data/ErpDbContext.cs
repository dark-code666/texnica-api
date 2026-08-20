using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ERP.Api.Domain;

namespace ERP.Api.Data;

public class ErpDbContext : DbContext
{
      private readonly IHttpContextAccessor _httpContextAccessor;

      public ErpDbContext(
            DbContextOptions<ErpDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options)
    {
            _httpContextAccessor = httpContextAccessor;
    }

      public int? CurrentCustomerId
      {
            get
            {
                  var value = _httpContextAccessor.HttpContext?.User.FindFirst("customer_id")?.Value;
                  return int.TryParse(value, out var customerId) ? customerId : null;
            }
      }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Fgpo> Fgpos => Set<Fgpo>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Factory> Factories => Set<Factory>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
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
    // AQL unificado (reemplaza EndlineInspection + PreFinalInspection + FinalInspection)
    public DbSet<AqlInspection> AqlInspections => Set<AqlInspection>();
    public DbSet<PpSample> PpSamples => Set<PpSample>();
    public DbSet<TopSample> TopSamples => Set<TopSample>();
    public DbSet<ProductionReadiness> ProductionReadiness => Set<ProductionReadiness>();
    public DbSet<CuttingRelease> CuttingReleases => Set<CuttingRelease>();
    public DbSet<CuttingControl> CuttingControls => Set<CuttingControl>();
    public DbSet<CuttingPanelQc> CuttingPanelQcs => Set<CuttingPanelQc>();
    public DbSet<Style> Styles => Set<Style>();
    public DbSet<Fabric> Fabrics => Set<Fabric>();
    public DbSet<Color> Colors => Set<Color>();
    public DbSet<Size> Sizes => Set<Size>();
    public DbSet<Component> Components => Set<Component>();
    public DbSet<BoxType> BoxTypes => Set<BoxType>();
    public DbSet<StyleYield> StyleYields => Set<StyleYield>();
    public DbSet<Price> Prices => Set<Price>();
    public DbSet<FgpoLine> FgpoLines => Set<FgpoLine>();
    public DbSet<TrimsControl> TrimsControls => Set<TrimsControl>();
    public DbSet<SewingProduction> SewingProductions => Set<SewingProduction>();
    public DbSet<FabricInventory> FabricInventories => Set<FabricInventory>();
    public DbSet<FabricReservation> FabricReservations => Set<FabricReservation>();
    public DbSet<PackingControl> PackingControls => Set<PackingControl>();
    public DbSet<FinishedGood> FinishedGoods => Set<FinishedGood>();
    public DbSet<ShipmentControl> ShipmentControls => Set<ShipmentControl>();
    public DbSet<Lot> Lots => Set<Lot>();
    public DbSet<CatalogValue> CatalogValues => Set<CatalogValue>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            // Customer is selected at login and stored in the JWT. All operational
            // records are scoped here so reads and writes cannot cross customers.
            modelBuilder.Entity<Fgpo>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<FgpoLine>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.Fgpo != null && x.Fgpo.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<FgpoLine>()
                  .HasIndex(x => new { x.FgpoId, x.StyleId, x.ColorId, x.SizeId })
                  .IsUnique()
                  .HasFilter("[Active] = 1");
            modelBuilder.Entity<FabricRequirement>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<FabricPOFgpo>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<FabricPO>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FabricPOFgpos.Any(link =>
                        link.FGPO != null && link.FGPO.CustomerId == CurrentCustomerId.Value));
            modelBuilder.Entity<Lot>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<MillProduction>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<MillTest>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<FabricShipment>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<FabricReceiving>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<RollReceiving>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<FourPointInspection>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<InternalTest>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<FabricInventory>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<FabricReservation>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<AqlInspection>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<InlineQuality>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<ShadeMatch>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<PpSample>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<TopSample>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<ProductionReadiness>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<CuttingRelease>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<CuttingControl>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<CuttingPanelQc>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<TrimsControl>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<SewingProduction>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<PackingControl>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<FinishedGood>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);
            modelBuilder.Entity<ShipmentControl>().HasQueryFilter(x =>
                  CurrentCustomerId.HasValue && x.FGPO != null && x.FGPO.CustomerId == CurrentCustomerId.Value);

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
            // Style y Color eliminados: la fuente real está en FgpoLines (FK a Styles/Colors)
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
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(f => f.Customer)
                  .WithMany()
                  .HasForeignKey(f => f.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

            // DataOwner: usuario logueado que creó/modificó el registro
            entity.HasOne(f => f.DataOwner)
                  .WithMany()
                  .HasForeignKey(f => f.DataOwnerId)
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

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.SupplierCode).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Contact).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<FabricRequirement>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Style).HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(50);
            // FabricComponent: normalizado a FK → Components
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
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(f => f.FGPO)
                  .WithMany()
                  .HasForeignKey(f => f.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.Component)
                  .WithMany()
                  .HasForeignKey(f => f.ComponentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.DataOwner)
                  .WithMany()
                  .HasForeignKey(f => f.DataOwnerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FabricPO>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.FabricPONumber).HasMaxLength(50);
            entity.HasIndex(e => e.FabricPONumber).IsUnique();
            // Supplier: normalizado a FK → Suppliers
            // FabricComponent: normalizado a FK → Components
            entity.Property(e => e.FabricMill).HasMaxLength(100);
            entity.Property(e => e.OrderedQuantity).HasPrecision(18, 4);
            entity.Property(e => e.UOM).HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 4);
            entity.Property(e => e.POAmount).HasPrecision(18, 4);
            entity.Property(e => e.OrderDate).IsRequired();
            entity.Property(e => e.RequiredCompletion).IsRequired();
            entity.Property(e => e.POStatus).HasMaxLength(50);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(f => f.Supplier)
                  .WithMany()
                  .HasForeignKey(f => f.SupplierId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.Component)
                  .WithMany()
                  .HasForeignKey(f => f.ComponentId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.PurchaseOwner)
                  .WithMany()
                  .HasForeignKey(f => f.PurchaseOwnerUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.ApprovedBy)
                  .WithMany()
                  .HasForeignKey(f => f.ApprovedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

            modelBuilder.Entity<Fabric>(entity =>
            {
                  entity.Property(e => e.FabricReference).HasMaxLength(100);
                  entity.Property(e => e.FabricName).IsRequired().HasMaxLength(200);
                  entity.Property(e => e.Color).HasMaxLength(100);
                  entity.Property(e => e.Content).HasMaxLength(200);
                  entity.Property(e => e.Construction).HasMaxLength(200);
                  entity.Property(e => e.ThreadTitle).HasMaxLength(200);
                  entity.Property(e => e.ThreadQuality).HasMaxLength(200);
                  entity.Property(e => e.Gsm).HasPrecision(18, 4);
                  entity.Property(e => e.WeightOz).HasPrecision(18, 4);
                  entity.Property(e => e.Comments).HasMaxLength(1000);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                  entity.Property(e => e.ColorCode).HasMaxLength(50);
                  entity.Property(e => e.AlternateCode).HasMaxLength(50);
                  entity.Property(e => e.ColorName).IsRequired().HasMaxLength(100);
                  entity.Property(e => e.DyeMethod).HasMaxLength(100);
            });

            modelBuilder.Entity<Size>(entity =>
            {
                  entity.Property(e => e.SizeCode).IsRequired().HasMaxLength(50);
                  entity.Property(e => e.Description).HasMaxLength(200);
            });

        modelBuilder.Entity<FabricPOFgpo>(entity =>
        {
            // FabricPOFgpo hereda BaseEntity: tiene Active, CreatedAt, UpdatedAt (soft-delete)
            entity.HasKey(pf => pf.ID);
            entity.HasIndex(pf => new { pf.FabricPOId, pf.FGPOId }).IsUnique();
            entity.Property(pf => pf.Active).HasDefaultValue(true);
            entity.Property(pf => pf.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

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
            // Supplier y FabricComponent derivados via FabricPO (JOIN) — no se almacenan redundantes
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

            entity.HasOne(m => m.DataOwner)
                  .WithMany()
                  .HasForeignKey(m => m.DataOwnerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MillTest>(entity =>
        {
            entity.HasKey(e => e.ID);
            // Supplier derivado via FabricPO.SupplierId (JOIN) — no se almacena redundante
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

            entity.HasOne(m => m.TestedBy)
                  .WithMany()
                  .HasForeignKey(m => m.TestedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FabricShipment>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ShipmentNumber).HasMaxLength(50);
            entity.HasIndex(e => e.ShipmentNumber).IsUnique();
            // Supplier derivado via FabricPO.SupplierId (JOIN) — no se almacena redundante
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

            entity.HasOne(s => s.DataOwner)
                  .WithMany()
                  .HasForeignKey(s => s.DataOwnerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FabricReceiving>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ReceivingNumber).HasMaxLength(50);
            entity.HasIndex(e => e.ReceivingNumber).IsUnique();
            entity.Property(e => e.ShipmentNumber).HasMaxLength(50);
            // Supplier derivado via FabricPO.SupplierId (JOIN) — no se almacena redundante
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

            entity.HasOne(r => r.ReceivedBy)
                  .WithMany()
                  .HasForeignKey(r => r.ReceivedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.DataOwner)
                  .WithMany()
                  .HasForeignKey(r => r.DataOwnerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RollReceiving>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ReceivingNumber).HasMaxLength(50);
            // Supplier derivado via FabricPO.SupplierId (JOIN) — no se almacena redundante
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

            entity.HasOne(r => r.DataOwner)
                  .WithMany()
                  .HasForeignKey(r => r.DataOwnerId)
                  .OnDelete(DeleteBehavior.Restrict);
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

            // Inspector: FK a Users
            entity.HasOne(i => i.Inspector)
                  .WithMany()
                  .HasForeignKey(i => i.InspectorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<InternalTest>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.TestDate).IsRequired();
            // Supplier derivado via FabricPO.SupplierId (JOIN) — no se almacena redundante
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

            entity.HasOne(t => t.TestedBy)
                  .WithMany()
                  .HasForeignKey(t => t.TestedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.ApprovedBy)
                  .WithMany()
                  .HasForeignKey(t => t.ApprovedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
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

            // Inspector: FK a Users
            entity.HasOne(i => i.Inspector)
                  .WithMany()
                  .HasForeignKey(i => i.InspectorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // AQL Inspections unificadas: Endline + PreFinal + Final → tabla única con InspectionType
        modelBuilder.Entity<AqlInspection>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.InspectionType).IsRequired().HasMaxLength(20);
            entity.Property(e => e.InspectionDate).IsRequired();
            entity.Property(e => e.LotShipment).HasMaxLength(100);
            entity.Property(e => e.LotSize).IsRequired();
            entity.Property(e => e.InspectionLevel).HasMaxLength(50);
            entity.Property(e => e.AqlMajor).HasPrecision(18, 4);
            entity.Property(e => e.AqlMinor).HasPrecision(18, 4);
            entity.Property(e => e.SampleSize).IsRequired();
            entity.Property(e => e.CriticalDefects).IsRequired();
            entity.Property(e => e.MajorDefects).IsRequired();
            entity.Property(e => e.MinorDefects).IsRequired();
            entity.Property(e => e.CriticalAc).IsRequired();
            entity.Property(e => e.MajorAc).IsRequired();
            entity.Property(e => e.MinorAc).IsRequired();
            entity.Property(e => e.CriticalRe).IsRequired();
            entity.Property(e => e.MajorRe).IsRequired();
            entity.Property(e => e.MinorRe).IsRequired();
            // Result calculado por AQL: Failed si cualquier defecto >= Re (fórmula del Excel)
            entity.Property(e => e.Result)
                  .HasComputedColumnSql("CAST((CASE WHEN [CriticalDefects] >= [CriticalRe] OR [MajorDefects] >= [MajorRe] OR [MinorDefects] >= [MinorRe] THEN 'Failed' ELSE 'Passed' END) AS nvarchar(50))");
            entity.Property(e => e.Disposition).HasMaxLength(100);
            entity.Property(e => e.ReportLink).HasMaxLength(500);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.InspectionType);
            entity.HasIndex(e => e.Result);
            entity.HasIndex(e => e.LotShipment);

            entity.HasOne(i => i.FGPO)
                  .WithMany()
                  .HasForeignKey(i => i.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Inspector: FK a Users
            entity.HasOne(i => i.Inspector)
                  .WithMany()
                  .HasForeignKey(i => i.InspectorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PpSample>(entity =>
        {
            entity.HasKey(e => e.ID);
            // Size: normalizado a FK → Sizes
            entity.Property(e => e.SampleVersion).HasMaxLength(50);
            entity.Property(e => e.FabricLot).HasMaxLength(100);
            entity.Property(e => e.TrimVersion).HasMaxLength(50);
            entity.Property(e => e.MeasurementResult).HasMaxLength(50);
            entity.Property(e => e.ConstructionResult).HasMaxLength(50);
            entity.Property(e => e.FitResult).HasMaxLength(50);
            entity.Property(e => e.FabricResult).HasMaxLength(50);
            entity.Property(e => e.TrimResult).HasMaxLength(50);
            entity.Property(e => e.LabelResult).HasMaxLength(50);
            entity.Property(e => e.InternalReview).HasMaxLength(50);
            entity.Property(e => e.CustomerReview).HasMaxLength(50);
            entity.Property(e => e.CustomerComments).HasMaxLength(2000);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.DocumentLink).HasMaxLength(500);
            entity.Property(e => e.PhotoLink).HasMaxLength(500);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.SampleVersion);

            entity.HasOne(s => s.FGPO)
                  .WithMany()
                  .HasForeignKey(s => s.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Size)
                  .WithMany()
                  .HasForeignKey(s => s.SizeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.ApprovedBy)
                  .WithMany()
                  .HasForeignKey(s => s.ApprovedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TopSample>(entity =>
        {
            entity.HasKey(e => e.ID);
            // Size: normalizado a FK → Sizes
            entity.Property(e => e.ProductionLine).HasMaxLength(100);
            entity.Property(e => e.FabricLot).HasMaxLength(100);
            entity.Property(e => e.CutLotBundle).HasMaxLength(100);
            entity.Property(e => e.TrimVersion).HasMaxLength(50);
            entity.Property(e => e.ThreadLot).HasMaxLength(100);
            entity.Property(e => e.TopQty).IsRequired();
            entity.Property(e => e.MeasurementResult).HasMaxLength(50);
            entity.Property(e => e.ConstructionResult).HasMaxLength(50);
            entity.Property(e => e.WorkmanshipResult).HasMaxLength(50);
            entity.Property(e => e.LabelResult).HasMaxLength(50);
            entity.Property(e => e.PackingResult).HasMaxLength(50);
            entity.Property(e => e.InternalReview).HasMaxLength(50);
            entity.Property(e => e.CustomerReview).HasMaxLength(50);
            entity.Property(e => e.CorrectiveAction).HasMaxLength(2000);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.DocumentLink).HasMaxLength(500);
            entity.Property(e => e.PhotoLink).HasMaxLength(500);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.ProductionLine);

            entity.HasOne(s => s.FGPO)
                  .WithMany()
                  .HasForeignKey(s => s.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Size)
                  .WithMany()
                  .HasForeignKey(s => s.SizeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.ApprovedBy)
                  .WithMany()
                  .HasForeignKey(s => s.ApprovedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProductionReadiness>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ReviewDate).IsRequired();
            entity.Property(e => e.PoConfirmed).HasMaxLength(50);
            entity.Property(e => e.TechPackCurrent).HasMaxLength(50);
            entity.Property(e => e.FabricApproved).HasMaxLength(50);
            entity.Property(e => e.TrimsApproved).HasMaxLength(50);
            entity.Property(e => e.TrimsAvailable).HasMaxLength(50);
            entity.Property(e => e.PpSampleApproved).HasMaxLength(50);
            entity.Property(e => e.PatternApproved).HasMaxLength(50);
            entity.Property(e => e.MarkerApproved).HasMaxLength(50);
            entity.Property(e => e.FabricWidthConfirmed).HasMaxLength(50);
            entity.Property(e => e.ShrinkageApproved).HasMaxLength(50);
            entity.Property(e => e.TorqueApproved).HasMaxLength(50);
            entity.Property(e => e.QualityStandardReady).HasMaxLength(50);
            entity.Property(e => e.LinePlanned).HasMaxLength(50);
            // OverallResult calculado por SQL (fórmula del Excel):
            // Blocked si algún 'Not Ready' > Not Ready si algún 'Pending' > Ready with Conditions si algún 'Exception Approved' > Ready
            entity.Property(e => e.OverallResult)
                  .HasComputedColumnSql("CAST((CASE WHEN [PoConfirmed]='Not Ready' OR [TechPackCurrent]='Not Ready' OR [FabricApproved]='Not Ready' OR [TrimsApproved]='Not Ready' OR [TrimsAvailable]='Not Ready' OR [PpSampleApproved]='Not Ready' OR [PatternApproved]='Not Ready' OR [MarkerApproved]='Not Ready' OR [FabricWidthConfirmed]='Not Ready' OR [ShrinkageApproved]='Not Ready' OR [TorqueApproved]='Not Ready' OR [QualityStandardReady]='Not Ready' OR [LinePlanned]='Not Ready' THEN 'Blocked' WHEN [PoConfirmed]='Pending' OR [TechPackCurrent]='Pending' OR [FabricApproved]='Pending' OR [TrimsApproved]='Pending' OR [TrimsAvailable]='Pending' OR [PpSampleApproved]='Pending' OR [PatternApproved]='Pending' OR [MarkerApproved]='Pending' OR [FabricWidthConfirmed]='Pending' OR [ShrinkageApproved]='Pending' OR [TorqueApproved]='Pending' OR [QualityStandardReady]='Pending' OR [LinePlanned]='Pending' THEN 'Not Ready' WHEN [PoConfirmed]='Exception Approved' OR [TechPackCurrent]='Exception Approved' OR [FabricApproved]='Exception Approved' OR [TrimsApproved]='Exception Approved' OR [TrimsAvailable]='Exception Approved' OR [PpSampleApproved]='Exception Approved' OR [PatternApproved]='Exception Approved' OR [MarkerApproved]='Exception Approved' OR [FabricWidthConfirmed]='Exception Approved' OR [ShrinkageApproved]='Exception Approved' OR [TorqueApproved]='Exception Approved' OR [QualityStandardReady]='Exception Approved' OR [LinePlanned]='Exception Approved' THEN 'Ready with Conditions' ELSE 'Ready' END) AS nvarchar(50))");
            entity.Property(e => e.OpenConditions).HasMaxLength(2000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.OverallResult);

            entity.HasOne(p => p.FGPO)
                  .WithMany()
                  .HasForeignKey(p => p.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.ResponsibleOwner)
                  .WithMany()
                  .HasForeignKey(p => p.ResponsibleOwnerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.ApprovedBy)
                  .WithMany()
                  .HasForeignKey(p => p.ApprovedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CuttingRelease>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ReleaseNumber).HasMaxLength(50);
            entity.HasIndex(e => e.ReleaseNumber).IsUnique();
            entity.Property(e => e.ReleaseDate).IsRequired();
            entity.Property(e => e.FabricLot).HasMaxLength(100);
            entity.Property(e => e.ApprovedCutQty).HasPrecision(18, 4);
            entity.Property(e => e.ApprovedWidth).HasPrecision(18, 4);
            entity.Property(e => e.MarkerNumber).HasMaxLength(100);
            entity.Property(e => e.ApprovedYield).HasPrecision(18, 4);
            entity.Property(e => e.PrrResult).HasMaxLength(50);
            entity.Property(e => e.Exception).HasMaxLength(1000);
            entity.Property(e => e.Conditions).HasMaxLength(2000);
            entity.Property(e => e.ReleaseStatus).HasMaxLength(50);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.ReleaseStatus);

            entity.HasOne(r => r.FGPO)
                  .WithMany()
                  .HasForeignKey(r => r.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.ReleasedBy)
                  .WithMany()
                  .HasForeignKey(r => r.ReleasedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.ReviewedBy)
                  .WithMany()
                  .HasForeignKey(r => r.ReviewedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CuttingControl>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.CutDate).IsRequired();
            // Size: normalizado a FK → Sizes
            entity.Property(e => e.FabricLot).HasMaxLength(100);
            entity.Property(e => e.MarkerNumber).HasMaxLength(100);
            entity.Property(e => e.PlannedCut).IsRequired();
            entity.Property(e => e.ActualCut).IsRequired();
            entity.Property(e => e.GoodCut).IsRequired();
            entity.Property(e => e.DamagedQty).IsRequired();
            entity.Property(e => e.ReplacementCut).IsRequired();
            entity.Property(e => e.SentToSewing).IsRequired();
            // Columnas calculadas por SQL (fórmulas del Excel)
            entity.Property(e => e.CuttingVariance)
                  .HasComputedColumnSql("CAST(([GoodCut] - [PlannedCut]) AS int)");
            entity.Property(e => e.PendingCut)
                  .HasComputedColumnSql("CAST((CASE WHEN [PlannedCut] - [GoodCut] > 0 THEN [PlannedCut] - [GoodCut] ELSE 0 END) AS int)");
            entity.Property(e => e.OvercutQty)
                  .HasComputedColumnSql("CAST((CASE WHEN [GoodCut] - [PlannedCut] > 0 THEN [GoodCut] - [PlannedCut] ELSE 0 END) AS int)");
            entity.Property(e => e.CutToSewDifference)
                  .HasComputedColumnSql("CAST(([GoodCut] - [SentToSewing]) AS int)");
            entity.Property(e => e.ReleaseStatus).HasMaxLength(50);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.ReleaseStatus);

            entity.HasOne(c => c.FGPO)
                  .WithMany()
                  .HasForeignKey(c => c.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Size)
                  .WithMany()
                  .HasForeignKey(c => c.SizeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.ResponsiblePerson)
                  .WithMany()
                  .HasForeignKey(c => c.ResponsiblePersonId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CuttingPanelQc>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.InspectionDate).IsRequired();
            // Size: normalizado a FK → Sizes
            entity.Property(e => e.FabricLot).HasMaxLength(100);
            entity.Property(e => e.CutLotLay).HasMaxLength(100);
            entity.Property(e => e.BundleNo).HasMaxLength(100);
            entity.Property(e => e.SampleQty).IsRequired();
            entity.Property(e => e.PanelDefects).IsRequired();
            entity.Property(e => e.NotchesDefects).IsRequired();
            entity.Property(e => e.DrillMarkDefects).IsRequired();
            entity.Property(e => e.ShadeDefects).IsRequired();
            entity.Property(e => e.MeasurementDefects).IsRequired();
            // Columnas calculadas por SQL (fórmulas del Excel); no referencian otras calculadas
            entity.Property(e => e.TotalDefects)
                  .HasComputedColumnSql("CAST(([PanelDefects] + [NotchesDefects] + [DrillMarkDefects] + [ShadeDefects] + [MeasurementDefects]) AS int)");
            entity.Property(e => e.DefectRatePct)
                  .HasComputedColumnSql("CAST((CASE WHEN [SampleQty] = 0 THEN 0 ELSE (([PanelDefects] + [NotchesDefects] + [DrillMarkDefects] + [ShadeDefects] + [MeasurementDefects]) / CAST([SampleQty] AS decimal(18,4))) END) AS decimal(18,4))");
            entity.Property(e => e.MaxAllowed).HasPrecision(18, 4);
            // Result calculado: Failed si DefectRate > MaxAllowed (2% según Excel)
            entity.Property(e => e.Result)
                  .HasComputedColumnSql("CAST((CASE WHEN [SampleQty] = 0 THEN 'Pending' WHEN (([PanelDefects] + [NotchesDefects] + [DrillMarkDefects] + [ShadeDefects] + [MeasurementDefects]) / CAST([SampleQty] AS decimal(18,4))) <= [MaxAllowed] THEN 'Passed' ELSE 'Failed' END) AS nvarchar(50))");
            entity.Property(e => e.CorrectiveAction).HasMaxLength(1000);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            // Índices para consultas frecuentes
            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.Result);

            entity.HasOne(c => c.FGPO)
                  .WithMany()
                  .HasForeignKey(c => c.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Size)
                  .WithMany()
                  .HasForeignKey(c => c.SizeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Inspector)
                  .WithMany()
                  .HasForeignKey(c => c.InspectorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Style>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.StyleCode).HasMaxLength(50);
            entity.HasIndex(e => e.StyleCode).IsUnique();
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.FabricDescription).HasMaxLength(300);
            entity.Property(e => e.FabricContent).HasMaxLength(300);
            entity.Property(e => e.Construction).HasMaxLength(200);
            entity.Property(e => e.Gsm).HasPrecision(18, 4);
            entity.Property(e => e.WeightOz).HasPrecision(18, 4);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Fabric>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.FabricReference).HasMaxLength(100);
            entity.Property(e => e.FabricName).HasMaxLength(200);
            entity.Property(e => e.Color).HasMaxLength(100);
            entity.Property(e => e.Content).HasMaxLength(300);
            entity.Property(e => e.Construction).HasMaxLength(200);
            entity.Property(e => e.Gsm).HasPrecision(18, 4);
            entity.Property(e => e.WeightOz).HasPrecision(18, 4);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.HasIndex(e => e.FabricName);
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ColorName).HasMaxLength(100);
            entity.HasIndex(e => e.ColorName).IsUnique();
            entity.Property(e => e.DyeMethod).HasMaxLength(100);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.SizeCode).HasMaxLength(20);
            entity.HasIndex(e => e.SizeCode).IsUnique();
            entity.Property(e => e.SortOrder).IsRequired();
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ComponentCode).HasMaxLength(50);
            entity.HasIndex(e => e.ComponentCode).IsUnique();
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<BoxType>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.BoxCode).HasMaxLength(50);
            entity.HasIndex(e => e.BoxCode).IsUnique();
            entity.Property(e => e.Length).HasPrecision(18, 4);
            entity.Property(e => e.Width).HasPrecision(18, 4);
            entity.Property(e => e.Height).HasPrecision(18, 4);
            entity.Property(e => e.EmptyCartonWeight).HasPrecision(18, 4);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<StyleYield>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.YieldQuoted).HasPrecision(18, 6);
            entity.Property(e => e.YieldReal).HasPrecision(18, 6);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => new { e.StyleId, e.ComponentId }).IsUnique();

            entity.HasOne(y => y.Style)
                  .WithMany()
                  .HasForeignKey(y => y.StyleId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(y => y.Component)
                  .WithMany()
                  .HasForeignKey(y => y.ComponentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Sku).HasMaxLength(50);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 4);
            entity.Property(e => e.Comments).HasMaxLength(500);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => new { e.StyleId, e.ColorId, e.SizeId }).IsUnique();

            entity.HasOne(p => p.Style)
                  .WithMany()
                  .HasForeignKey(p => p.StyleId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(p => p.Color)
                  .WithMany()
                  .HasForeignKey(p => p.ColorId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(p => p.Size)
                  .WithMany()
                  .HasForeignKey(p => p.SizeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FgpoLine>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.UnitPrice).HasPrecision(18, 4);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => new { e.FgpoId, e.StyleId, e.ColorId, e.SizeId }).IsUnique();

            entity.HasOne(l => l.Fgpo)
                  .WithMany(f => f.FgpoLines)
                  .HasForeignKey(l => l.FgpoId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(l => l.Style)
                  .WithMany()
                  .HasForeignKey(l => l.StyleId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(l => l.Color)
                  .WithMany()
                  .HasForeignKey(l => l.ColorId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(l => l.Size)
                  .WithMany()
                  .HasForeignKey(l => l.SizeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TrimsControl>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.TrimType).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Uom).HasMaxLength(50);
            entity.Property(e => e.ConsumptionPerGarment).HasPrecision(18, 4);
            entity.Property(e => e.RequiredQty).HasPrecision(18, 4);
            entity.Property(e => e.OrderedQty).HasPrecision(18, 4);
            entity.Property(e => e.ReceivedQty).HasPrecision(18, 4);
            entity.Property(e => e.ApprovedQty).HasPrecision(18, 4);
            entity.Property(e => e.RejectedQty).HasPrecision(18, 4);
            entity.Property(e => e.ReservedQty).HasPrecision(18, 4);
            entity.Property(e => e.IssuedQty).HasPrecision(18, 4);
            // Columnas calculadas por SQL (fórmulas del Excel); AvailabilityStatus no referencia otras calculadas
            entity.Property(e => e.AvailableQty)
                  .HasComputedColumnSql("CAST((CASE WHEN [ApprovedQty] - [ReservedQty] - [IssuedQty] > 0 THEN [ApprovedQty] - [ReservedQty] - [IssuedQty] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.ShortageQty)
                  .HasComputedColumnSql("CAST((CASE WHEN [RequiredQty] - [ApprovedQty] > 0 THEN [RequiredQty] - [ApprovedQty] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.AvailabilityStatus)
                  .HasComputedColumnSql("CAST((CASE WHEN [RequiredQty] - [ApprovedQty] > 0 THEN 'Shortage' WHEN (CASE WHEN [ApprovedQty] - [ReservedQty] - [IssuedQty] > 0 THEN [ApprovedQty] - [ReservedQty] - [IssuedQty] ELSE 0 END) >= [RequiredQty] THEN 'Ready' WHEN (CASE WHEN [ApprovedQty] - [ReservedQty] - [IssuedQty] > 0 THEN [ApprovedQty] - [ReservedQty] - [IssuedQty] ELSE 0 END) > 0 THEN 'Partially Ready' ELSE 'Pending' END) AS nvarchar(50))");
            entity.Property(e => e.DevelopmentStatus).HasMaxLength(100);
            entity.Property(e => e.ApprovalStatus).HasMaxLength(100);
            entity.Property(e => e.DataOwner).HasMaxLength(100);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.AvailabilityStatus);

            entity.HasOne(t => t.FGPO)
                  .WithMany()
                  .HasForeignKey(t => t.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Supplier)
                  .WithMany()
                  .HasForeignKey(t => t.SupplierId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SewingProduction>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ProductionDate).IsRequired();
            entity.Property(e => e.Shift).HasMaxLength(50);
            entity.Property(e => e.Line).HasMaxLength(100);
            entity.Property(e => e.SewingInput).IsRequired();
            entity.Property(e => e.DailyTarget).IsRequired();
            entity.Property(e => e.DailyOutput).IsRequired();
            entity.Property(e => e.CumulativeOutput).IsRequired();
            entity.Property(e => e.Wip).IsRequired();
            entity.Property(e => e.Rework).IsRequired();
            entity.Property(e => e.Reject).IsRequired();
            entity.Property(e => e.DowntimeMinutes).IsRequired();
            // Columnas calculadas por SQL (fórmulas del Excel)
            entity.Property(e => e.TargetAchievementPct)
                  .HasComputedColumnSql("CAST((CASE WHEN [DailyTarget] = 0 THEN 0 ELSE ([DailyOutput] / CAST([DailyTarget] AS decimal(18,4))) END) AS decimal(18,4))");
            entity.Property(e => e.SewingVariance)
                  .HasComputedColumnSql("CAST(([CumulativeOutput] - [SewingInput]) AS int)");
            entity.Property(e => e.PendingSewing)
                  .HasComputedColumnSql("CAST((CASE WHEN [SewingInput] - [CumulativeOutput] > 0 THEN [SewingInput] - [CumulativeOutput] ELSE 0 END) AS int)");
            entity.Property(e => e.Overproduction)
                  .HasComputedColumnSql("CAST((CASE WHEN [CumulativeOutput] - [SewingInput] > 0 THEN [CumulativeOutput] - [SewingInput] ELSE 0 END) AS int)");
            entity.Property(e => e.TopStatus).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.Line);

            entity.HasOne(s => s.FGPO)
                  .WithMany()
                  .HasForeignKey(s => s.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Size)
                  .WithMany()
                  .HasForeignKey(s => s.SizeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Supervisor)
                  .WithMany()
                  .HasForeignKey(s => s.SupervisorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FabricInventory>(entity =>
        {
            entity.HasKey(e => e.ID);
            // FabricComponent: derivado via FabricPO.ComponentId → se obtiene por JOIN
            // UOM: derivado via FabricPO.UOM → se obtiene por JOIN
            entity.Property(e => e.ReceivedQuantity).HasPrecision(18, 4);
            entity.Property(e => e.ApprovedQuantity).HasPrecision(18, 4);
            entity.Property(e => e.RejectedQuantity).HasPrecision(18, 4);
            entity.Property(e => e.HoldQuantity).HasPrecision(18, 4);
            entity.Property(e => e.ReservedQuantity).HasPrecision(18, 4);
            entity.Property(e => e.IssuedQuantity).HasPrecision(18, 4);
            entity.Property(e => e.ReturnedQuantity).HasPrecision(18, 4);
            // Columna calculada por SQL: Available = MAX(0, Approved − Reserved − Issued + Returned)
            entity.Property(e => e.AvailableQuantity)
                  .HasComputedColumnSql("CAST((CASE WHEN [ApprovedQuantity] - [ReservedQuantity] - [IssuedQuantity] + [ReturnedQuantity] > 0 THEN [ApprovedQuantity] - [ReservedQuantity] - [IssuedQuantity] + [ReturnedQuantity] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.ShortageQuantity).HasPrecision(18, 4);
            entity.Property(e => e.WarehouseLocation).HasMaxLength(100);
            entity.Property(e => e.InventoryStatus).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => new { e.FabricPOId, e.FGPOId });
            entity.HasIndex(e => e.InventoryStatus);

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
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.DataOwner)
                  .WithMany()
                  .HasForeignKey(i => i.DataOwnerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FabricReservation>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ReservationDate).IsRequired();
            // FabricComponent: derivado via FabricPO.ComponentId → se obtiene por JOIN
            // UOM: derivado via FabricPO.UOM → se obtiene por JOIN
            entity.Property(e => e.ReservedQuantity).HasPrecision(18, 4);
            entity.Property(e => e.ReleasedQuantity).HasPrecision(18, 4);
            // Columna calculada por SQL: Remaining = MAX(0, Reserved − Released)
            entity.Property(e => e.RemainingReservation)
                  .HasComputedColumnSql("CAST((CASE WHEN [ReservedQuantity] - [ReleasedQuantity] > 0 THEN [ReservedQuantity] - [ReleasedQuantity] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => new { e.FabricPOId, e.FGPOId });
            entity.HasIndex(e => e.Status);

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
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.ReservedBy)
                  .WithMany()
                  .HasForeignKey(r => r.ReservedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.ApprovedBy)
                  .WithMany()
                  .HasForeignKey(r => r.ApprovedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PackingControl>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.PackingDate).IsRequired();
            entity.Property(e => e.QcPassedQty).HasPrecision(18, 4);
            entity.Property(e => e.ReceivedByPackingQty).HasPrecision(18, 4);
            entity.Property(e => e.FoldedQty).HasPrecision(18, 4);
            entity.Property(e => e.PolybaggedQty).HasPrecision(18, 4);
            entity.Property(e => e.PackedQty).HasPrecision(18, 4);
            // Columnas calculadas por SQL:
            // ReadyToShipQty = PackedQty
            // PackingVariance = PackedQty - QcPassedQty
            // PendingPacking = MAX(0, QcPassedQty - PackedQty)
            // OverpackedQty = MAX(0, PackedQty - QcPassedQty)
            entity.Property(e => e.ReadyToShipQty)
                  .HasComputedColumnSql("CAST([PackedQty] AS decimal(18,4))");
            entity.Property(e => e.PackingVariance)
                  .HasComputedColumnSql("CAST(([PackedQty] - [QcPassedQty]) AS decimal(18,4))");
            entity.Property(e => e.PendingPacking)
                  .HasComputedColumnSql("CAST((CASE WHEN [QcPassedQty] - [PackedQty] > 0 THEN [QcPassedQty] - [PackedQty] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.OverpackedQty)
                  .HasComputedColumnSql("CAST((CASE WHEN [PackedQty] - [QcPassedQty] > 0 THEN [PackedQty] - [QcPassedQty] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.PackingDate);

            entity.HasOne(p => p.FGPO)
                  .WithMany()
                  .HasForeignKey(p => p.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.ResponsiblePerson)
                  .WithMany()
                  .HasForeignKey(p => p.ResponsiblePersonId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FinishedGood>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ReceiptDate).IsRequired();
            entity.Property(e => e.PackedQty).HasPrecision(18, 4);
            entity.Property(e => e.WarehouseReceived).HasPrecision(18, 4);
            entity.Property(e => e.ReservedForShipment).HasPrecision(18, 4);
            entity.Property(e => e.LoadedQty).HasPrecision(18, 4);
            entity.Property(e => e.ShippedQty).HasPrecision(18, 4);
            // Columnas calculadas por SQL:
            // ReadyToShipQty = MAX(0, WarehouseReceived - ReservedForShipment - LoadedQty - ShippedQty)
            // WarehouseBalance = MAX(0, WarehouseReceived - LoadedQty - ShippedQty)
            entity.Property(e => e.ReadyToShipQty)
                  .HasComputedColumnSql("CAST((CASE WHEN [WarehouseReceived] - [ReservedForShipment] - [LoadedQty] - [ShippedQty] > 0 THEN [WarehouseReceived] - [ReservedForShipment] - [LoadedQty] - [ShippedQty] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.WarehouseBalance)
                  .HasComputedColumnSql("CAST((CASE WHEN [WarehouseReceived] - [LoadedQty] - [ShippedQty] > 0 THEN [WarehouseReceived] - [LoadedQty] - [ShippedQty] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.WarehouseLocation).HasMaxLength(150);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.ReceiptDate);
            entity.HasIndex(e => e.Status);

            entity.HasOne(f => f.FGPO)
                  .WithMany()
                  .HasForeignKey(f => f.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.DataOwner)
                  .WithMany()
                  .HasForeignKey(f => f.DataOwnerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ShipmentControl>(entity =>
        {
            entity.HasKey(e => e.ID);
            entity.Property(e => e.ShipmentNumber).HasMaxLength(100).IsRequired();
            entity.Property(e => e.PlannedQty).HasPrecision(18, 4);
            entity.Property(e => e.ActualLoadedQty).HasPrecision(18, 4);
            entity.Property(e => e.InTransitQty).HasPrecision(18, 4);
            entity.Property(e => e.CustomerReceivedQty).HasPrecision(18, 4);
            entity.Property(e => e.TotalShippedQty).HasPrecision(18, 4);
            // Columnas calculadas por SQL:
            // ShipmentVariance = TotalShippedQty - PlannedQty
            // PendingToShip    = MAX(0, PlannedQty - TotalShippedQty)
            // OvershipmentQty  = MAX(0, TotalShippedQty - PlannedQty)
            entity.Property(e => e.ShipmentVariance)
                  .HasComputedColumnSql("CAST(([TotalShippedQty] - [PlannedQty]) AS decimal(18,4))");
            entity.Property(e => e.PendingToShip)
                  .HasComputedColumnSql("CAST((CASE WHEN [PlannedQty] - [TotalShippedQty] > 0 THEN [PlannedQty] - [TotalShippedQty] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.OvershipmentQty)
                  .HasComputedColumnSql("CAST((CASE WHEN [TotalShippedQty] - [PlannedQty] > 0 THEN [TotalShippedQty] - [PlannedQty] ELSE 0 END) AS decimal(18,4))");
            entity.Property(e => e.ContainerType).HasMaxLength(100);
            entity.Property(e => e.ContainerNumber).HasMaxLength(100);
            entity.Property(e => e.BookingNumber).HasMaxLength(100);
            entity.Property(e => e.Destination).HasMaxLength(200);
            entity.Property(e => e.ShipmentStatus).HasMaxLength(100);
            entity.Property(e => e.PackingList).HasMaxLength(300);
            entity.Property(e => e.InvoiceNumber).HasMaxLength(100);
            entity.Property(e => e.LoadPlan).HasMaxLength(500);
            entity.Property(e => e.Remarks).HasMaxLength(1000);
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            entity.HasIndex(e => e.ShipmentNumber);
            entity.HasIndex(e => e.FGPOId);
            entity.HasIndex(e => e.ShipmentStatus);

            entity.HasOne(s => s.FGPO)
                  .WithMany()
                  .HasForeignKey(s => s.FGPOId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.DataOwner)
                  .WithMany()
                  .HasForeignKey(s => s.DataOwnerId)
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
