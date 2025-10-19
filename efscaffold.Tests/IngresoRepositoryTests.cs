using System.Threading.Tasks;
using Xunit;
using efscaffold.Repositories.Json;
using System.IO;
using efscaffold.Models.Prq;
using efscaffold.Repositories.Dtos;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using efscaffold.Repositories.Db;

namespace efscaffold.Tests
{
    public class IngresoRepositoryTests
    {
        [Fact]
        public async Task JsonIngreso_ListByVehicleTypeAndAmount()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            var autos = new[] { new PrqAutomovil { Id = 1, Color = "R", Year = 2020, Manufacturer = "X", Type = "sedán" } };
            var parques = new[] { new PrqParqueo { Id = 10, ProvinceName = "Lima", Name = "P1", PricePerHour = 6m } };
            var ingresos = new[] { new PrqIngresoAutomovil { Consecutive = 100, PrqParqueoId = 10, PrqAutomovilId = 1, EntryDateTime = DateTime.UtcNow.AddHours(-2), ExitDateTime = DateTime.UtcNow } };

            await File.WriteAllTextAsync(Path.Combine(tempDir, "prq_automoviles.json"), System.Text.Json.JsonSerializer.Serialize(autos));
            await File.WriteAllTextAsync(Path.Combine(tempDir, "prq_parqueo.json"), System.Text.Json.JsonSerializer.Serialize(parques));
            await File.WriteAllTextAsync(Path.Combine(tempDir, "prq_ingresoautomoviles.json"), System.Text.Json.JsonSerializer.Serialize(ingresos));

            var repo = new JsonIngresoRepository(tempDir);
            var results = (await repo.ListByVehicleTypeBetweenDatesAsync("sed", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1))).ToList();
            Assert.Single(results);
            Assert.NotNull(results[0].MontoTotal);

            Directory.Delete(tempDir, true);
        }

        [Fact]
        public async Task DbIngreso_GetPriceAndListByProvince()
        {
            var options = new DbContextOptionsBuilder<efscaffold.Models.Prq.PrqDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new efscaffold.Models.Prq.PrqDbContext(options);
            var p = new PrqParqueo { Id = 5, ProvinceName = "Piura", Name = "P", PricePerHour = 7m };
            var a = new PrqAutomovil { Id = 2, Color = "B", Year = 2019, Manufacturer = "M", Type = "moto" };
            var i = new PrqIngresoAutomovil { Consecutive = 200, PrqParqueoId = 5, PrqAutomovilId = 2, EntryDateTime = DateTime.UtcNow.AddHours(-3), ExitDateTime = DateTime.UtcNow };
            db.PRQ_Parqueo.Add(p); db.PRQ_Automoviles.Add(a); db.PRQ_IngresoAutomoviles.Add(i);
            await db.SaveChangesAsync();

            var repo = new DbIngresoRepository(db);
            var price = await repo.GetPricePerHourByParkingIdAsync(5);
            Assert.Equal(7m, price);

            var res = (await repo.ListByProvinceBetweenDatesAsync("Piur", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1))).ToList();
            Assert.Single(res);
            Assert.NotNull(res[0].MontoTotal);
        }
    }
}