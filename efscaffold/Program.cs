using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using efscaffold.Models.Prq;
using efscaffold.Repositories;
using efscaffold.Repositories.Json;
using efscaffold.Repositories.Db;

// Build a simple host to demonstrate DI registration for repositories.
var host = Host.CreateDefaultBuilder(args)
	.ConfigureAppConfiguration((ctx, cfg) =>
	{
		// Load example secrets if present (optional)
		cfg.AddJsonFile("secreto.json.example", optional: true, reloadOnChange: false);
	})
	.ConfigureServices((ctx, services) =>
	{
		// Switch between JSON-backed repositories and DB-backed repositories
		var useJson = Environment.GetEnvironmentVariable("USE_JSON") == "1";

		if (useJson)
		{
			var baseDir = Directory.GetCurrentDirectory();
			services.AddSingleton<IAutomovilesRepository>(_ => new JsonAutomovilesRepository(baseDir));
			services.AddSingleton<IParqueoRepository>(_ => new JsonParqueoRepository(baseDir));
			services.AddSingleton<IIngresoRepository>(_ => new JsonIngresoRepository(baseDir));
		}
		else
		{
			// Database connection from configuration (secreto.json.example or environment)
			var cfg = ctx.Configuration;
			var hostVal = cfg["DB_HOST"] ?? "127.0.0.1";
			var portVal = cfg["DB_PORT"] ?? "3306";
			var user = cfg["DB_USER"] ?? "root";
			var pwd = cfg["DB_PASSWORD"] ?? string.Empty;
			var dbName = cfg["DB_NAME"] ?? "trabajoclase02";
			var conn = $"server={hostVal};port={portVal};user={user};password={pwd};database={dbName};";

			services.AddDbContext<PrqDbContext>(options => options.UseMySql(conn, ServerVersion.AutoDetect(conn)));
			services.AddScoped<IAutomovilesRepository, DbAutomovilesRepository>();
			services.AddScoped<IParqueoRepository, DbParqueoRepository>();
			services.AddScoped<IIngresoRepository, DbIngresoRepository>();
		}
	})
	.Build();

// Example usage: resolve a repository and print a count of automobiles
using (var scope = host.Services.CreateScope())
{
	var sp = scope.ServiceProvider;
	var autosRepo = sp.GetRequiredService<IAutomovilesRepository>();
	var autos = await autosRepo.GetAllAsync();
	Console.WriteLine($"Automóviles cargados: {autos?.Count() ?? 0}");
}

// Dispose host
await host.StopAsync();
