    using Microsoft.EntityFrameworkCore;
    using ERP.Api.Data;
    using ERP.Api.Domain;
    using ERP.Api.Interfaces;
    using ERP.Api.Repositories;
    using ERP.Api.Services;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using System.Text;

    var builder = WebApplication.CreateBuilder(args);

    LoadEnvFile(Path.Combine(builder.Environment.ContentRootPath, ".env"));
    LoadEnvFile(Path.Combine(builder.Environment.ContentRootPath, "..", ".env"));
    LoadEnvFile(Path.Combine(Directory.GetCurrentDirectory(), ".env"));


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
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IRoleService, RoleService>();
    builder.Services.AddScoped<IPermissionService, PermissionService>();
    builder.Services.AddScoped<IFgpoService, FgpoService>();
    builder.Services.AddScoped<ICustomerService, CustomerService>();
    builder.Services.AddScoped<IFactoryService, FactoryService>();
    builder.Services.AddScoped<IFabricRequirementService, FabricRequirementService>();
    builder.Services.AddScoped<IFabricPOService, FabricPOService>();



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
    builder.Services.AddAuthorization();

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
    app.MapGet("/", () => Results.Redirect("/swagger"));
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
