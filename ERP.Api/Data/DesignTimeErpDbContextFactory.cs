using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ERP.Api.Domain;

namespace ERP.Api.Data;

public class DesignTimeErpDbContextFactory : IDesignTimeDbContextFactory<ErpDbContext>
{
    public ErpDbContext CreateDbContext(string[] args)
    {
        // Cargar .env según el entorno, igual que Program.cs
        var envName = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development").ToLowerInvariant();
        var envFile = envName switch
        {
            "development" => ".env.local",
            "production" => ".env.production",
            _ => $".env.{envName}"
        };

        var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), envFile);
        if (File.Exists(envFilePath))
        {
            LoadEnvFile(envFilePath);
        }
        else
        {
            LoadEnvFile(Path.Combine(Directory.GetCurrentDirectory(), ".env"));
        }

        // También cargar .env desde el directorio padre (raíz del repo) si existe
        LoadEnvFile(Path.Combine(Directory.GetCurrentDirectory(), "..", ".env"));

        var dbServer = GetEnv("DB_SERVER")
            ?? GetEnv("DB_HOST")
            ?? "(localdb)\\MSSQLLocalDB";

        var dbName = GetEnv("DB_NAME")
            ?? GetEnv("DB_DATABASE")
            ?? "ERPApiDb";
        var dbUser = GetEnv("DB_USER")
            ?? GetEnv("DB_USERNAME");
        var dbPassword = GetEnv("DB_PASSWORD")
            ?? GetEnv("DB_PASS");
        var dbPort = GetEnv("DB_PORT");


        Console.WriteLine(dbServer);
        Console.WriteLine(dbName);
        Console.WriteLine(dbUser);
        Console.WriteLine(dbPassword);
        Console.WriteLine(dbPort);

        var serverAddress = string.IsNullOrWhiteSpace(dbPort)
            ? dbServer
            : $"{dbServer},{dbPort}";

        var connectionString = string.IsNullOrWhiteSpace(dbUser) || string.IsNullOrWhiteSpace(dbPassword)
            ? $"Server={serverAddress};Database={dbName};Integrated Security=True;TrustServerCertificate=True;"
            : $"Server={serverAddress};Database={dbName};User Id={dbUser};Password={dbPassword};Encrypt=True;TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<ErpDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ErpDbContext(optionsBuilder.Options);
    }

    private static string? GetEnv(string key)
    {
        return Environment.GetEnvironmentVariable(key);
    }

    private static void LoadEnvFile(string path)
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
}
