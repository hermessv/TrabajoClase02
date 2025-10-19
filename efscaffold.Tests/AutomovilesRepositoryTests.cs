using System.Threading.Tasks;
using Xunit;
using efscaffold.Repositories.Json;
using System.IO;
using efscaffold.Models.Prq;
using System.Text.Json;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using efscaffold.Repositories.Db;

namespace efscaffold.Tests
{
    public class AutomovilesRepositoryTests
    {
        [Fact]
        public async Task JsonRepository_CRUD_and_filters_work()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var repo = new JsonAutomovilesRepository(tempDir);

            var a1 = new PrqAutomovil { Id = 1, Color = "Rojo", Year = 2020, Manufacturer = "Toyota", Type = "sedán" };
            var a2 = new PrqAutomovil { Id = 2, Color = "Azul", Year = 2018, Manufacturer = "Honda", Type = "4x4" };

            await repo.AddAsync(a1);
            await repo.AddAsync(a2);

            var all = (await repo.GetAllAsync()).ToList();
            Assert.Equal(2, all.Count);

            var byColor = (await repo.ListByColorAsync("roj")).ToList();
            Assert.Single(byColor);

            var byType = (await repo.ListByTypeAsync("4x4")).ToList();
            Assert.Single(byType);

            // Update
            a1.Color = "Verde";
            await repo.UpdateAsync(a1);
            var fetched = await repo.GetByIdAsync(1);
            Assert.Equal("Verde", fetched!.Color);

            // Delete
            await repo.DeleteAsync(2);
            var afterDel = (await repo.GetAllAsync()).ToList();
            Assert.Single(afterDel);

            Directory.Delete(tempDir, true);
        }

        [Fact]
        public async Task DbRepository_ListByYearRange_works()
        {
            var options = new DbContextOptionsBuilder<efscaffold.Models.Prq.PrqDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new efscaffold.Models.Prq.PrqDbContext(options);
            db.PRQ_Automoviles.AddRange(new PrqAutomovil { Id = 1, Color = "R", Year = 2010, Manufacturer = "X", Type = "t" }, new PrqAutomovil { Id = 2, Color = "B", Year = 2020, Manufacturer = "Y", Type = "t2" });
            await db.SaveChangesAsync();

            var repo = new DbAutomovilesRepository(db);
            var res = (await repo.ListByYearRangeAsync(2015, 2025)).ToList();
            Assert.Single(res);
        }
    }
}