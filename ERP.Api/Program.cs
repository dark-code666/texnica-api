    using Microsoft.EntityFrameworkCore;
    using ERP.Api.Data;
    using ERP.Api.Domain;
    using ERP.Api.Interfaces;
    using ERP.Api.Repositories;
    using ERP.Api.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using System.Text;

    var builder = WebApplication.CreateBuilder(args);

    // Cargar .env según el entorno (ASPNETCORE_ENVIRONMENT)
    //   Development  → .env.local
    //   Production   → .env.production
    //   Staging      → .env.staging
    //   Si no existe el específico, cae en .env genérico
    var envName = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development").ToLowerInvariant();
    var envFile = envName switch
    {
        "development" => ".env.local",
        "production" => ".env.production",
        _ => $".env.{envName}"
    };

    var envFilePath = Path.Combine(builder.Environment.ContentRootPath, envFile);
    if (File.Exists(envFilePath))
    {
        LoadEnvFile(envFilePath);
    }
    else
    {
        // Fallback: .env genérico si no existe el específico del entorno
        LoadEnvFile(Path.Combine(builder.Environment.ContentRootPath, ".env"));
    }

    // También cargar .env desde el directorio padre (raíz del repo) si existe
    LoadEnvFile(Path.Combine(builder.Environment.ContentRootPath, "..", ".env"));


    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ERP API", Version = "v1" });

        // JWT in Swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Ingrese el token JWT en el siguiente formato: Bearer {token}"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    var dbServer = Environment.GetEnvironmentVariable("DB_SERVER")
        ?? Environment.GetEnvironmentVariable("DB_HOST")
        ?? "(localdb)\\MSSQLLocalDB";
    var dbName = Environment.GetEnvironmentVariable("DB_NAME")
        ?? Environment.GetEnvironmentVariable("DB_DATABASE")
        ?? "ERPApiDb";
    var dbUser = Environment.GetEnvironmentVariable("DB_USER")
        ?? Environment.GetEnvironmentVariable("DB_USERNAME");
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD")
        ?? Environment.GetEnvironmentVariable("DB_PASS");
    var dbPort = Environment.GetEnvironmentVariable("DB_PORT");

    var serverAddress = string.IsNullOrWhiteSpace(dbPort)
        ? dbServer
        : $"{dbServer},{dbPort}";

    var connectionString = string.IsNullOrWhiteSpace(dbUser) || string.IsNullOrWhiteSpace(dbPassword)
        ? $"Server={serverAddress};Database={dbName};Integrated Security=True;TrustServerCertificate=True;"
        : $"Server={serverAddress};Database={dbName};User Id={dbUser};Password={dbPassword};Encrypt=True;TrustServerCertificate=True;";

    builder.Services.AddDbContext<ErpDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IRoleService, RoleService>();
    builder.Services.AddScoped<IPermissionService, PermissionService>();
    builder.Services.AddScoped<IFgpoService, FgpoService>();
    builder.Services.AddScoped<ICustomerService, CustomerService>();
    builder.Services.AddScoped<IFactoryService, FactoryService>();
    builder.Services.AddScoped<IFabricRequirementService, FabricRequirementService>();
    builder.Services.AddScoped<IFabricPOService, FabricPOService>();
    builder.Services.AddScoped<IMillProductionService, MillProductionService>();
    builder.Services.AddScoped<IMillTestService, MillTestService>();
    builder.Services.AddScoped<IFabricShipmentService, FabricShipmentService>();
    builder.Services.AddScoped<IFabricReceivingService, FabricReceivingService>();
    builder.Services.AddScoped<IRollReceivingService, RollReceivingService>();
    builder.Services.AddScoped<IFourPointService, FourPointService>();
    builder.Services.AddScoped<IInternalTestService, InternalTestService>();
    builder.Services.AddScoped<IShadeMatchService, ShadeMatchService>();
    builder.Services.AddScoped<IInlineQualityService, InlineQualityService>();
    builder.Services.AddScoped<IAqlInspectionService, AqlInspectionService>();
    builder.Services.AddScoped<IPpSampleService, PpSampleService>();
    builder.Services.AddScoped<ITopSampleService, TopSampleService>();
    builder.Services.AddScoped<IProductionReadinessService, ProductionReadinessService>();
    builder.Services.AddScoped<ICuttingReleaseService, CuttingReleaseService>();
    builder.Services.AddScoped<ICuttingControlService, CuttingControlService>();
    builder.Services.AddScoped<ICuttingPanelQcService, CuttingPanelQcService>();
    builder.Services.AddScoped<IStyleService, StyleService>();
    builder.Services.AddScoped<IFabricService, FabricService>();
    builder.Services.AddScoped<IColorService, ColorService>();
    builder.Services.AddScoped<ISizeService, SizeService>();
    builder.Services.AddScoped<IComponentService, ComponentService>();
    builder.Services.AddScoped<IBoxTypeService, BoxTypeService>();
    builder.Services.AddScoped<IStyleYieldService, StyleYieldService>();
    builder.Services.AddScoped<IPriceService, PriceService>();
    builder.Services.AddScoped<IFgpoLineService, FgpoLineService>();
    builder.Services.AddScoped<ITrimsControlService, TrimsControlService>();
    builder.Services.AddScoped<ISewingProductionService, SewingProductionService>();
    builder.Services.AddScoped<IFabricInventoryService, FabricInventoryService>();
    builder.Services.AddScoped<IFabricReservationService, FabricReservationService>();
    builder.Services.AddScoped<ICatalogService, CatalogService>();
    builder.Services.AddScoped<ISupplierService, SupplierService>();



var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
                ?? builder.Configuration["Jwt:Secret"];

if (string.IsNullOrWhiteSpace(jwtSecret))
{
    throw new InvalidOperationException("JWT_SECRET no está configurado.");
}
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "ERP.Api",
            ValidAudience = "ERP.Client",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });
    builder.Services.AddAuthorization(options =>
    {
        // Política global: TODA petición requiere un token JWT válido,
        // salvo los endpoints marcados con [AllowAnonymous] (login, register).
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API V1");
        c.RoutePrefix = "swagger";
    });

    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapGet("/", () => Results.Redirect("/swagger")).AllowAnonymous();
    app.MapControllers();

    app.Run();

    static void LoadEnvFile(string path)
    {
        if (!File.Exists(path))
        {
            return;
        }

        foreach (var line in File.ReadAllLines(path))
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith('#'))
            {
                continue;
            }

            var separatorIndex = trimmedLine.IndexOf('=');
            if (separatorIndex <= 0)
            {
                continue;
            }

            var key = trimmedLine[..separatorIndex].Trim();
            var value = trimmedLine[(separatorIndex + 1)..].Trim().Trim('"');

            Environment.SetEnvironmentVariable(key, value);
        }
    }
