using Microsoft.EntityFrameworkCore;
using ERP.Api.Data;
using ERP.Api.Domain;
using ERP.Api.Interfaces;
using ERP.Api.Repositories;
using ERP.Api.Services;

var builder = WebApplication.CreateBuilder(args);

LoadEnvFile(Path.Combine(builder.Environment.ContentRootPath, ".env"));
LoadEnvFile(Path.Combine(builder.Environment.ContentRootPath, "..", ".env"));
LoadEnvFile(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

builder.WebHost.UseUrls("https://localhost:7123", "http://localhost:5123");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var dbServer = Environment.GetEnvironmentVariable("DB_SERVER")
    ?? throw new InvalidOperationException("DB_SERVER is not configured.");
var dbName = Environment.GetEnvironmentVariable("DB_NAME")
    ?? throw new InvalidOperationException("DB_NAME is not configured.");
var dbUser = Environment.GetEnvironmentVariable("DB_USER")
    ?? throw new InvalidOperationException("DB_USER is not configured.");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD")
    ?? throw new InvalidOperationException("DB_PASSWORD is not configured.");

var connectionString =
    $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword};Encrypt=True;TrustServerCertificate=True;";

builder.Services.AddDbContext<ErpDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
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
