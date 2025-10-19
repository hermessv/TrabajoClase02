using System.Threading.Tasks;
using Xunit;
using efscaffold.Repositories.Json;
using System.IO;
using efscaffold.Models.Prq;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using efscaffold.Repositories.Db;

namespace efscaffold.Tests
{
    public class ParqueoRepositoryTests
    {
        [Fact]
        public async Task JsonParqueo_CRUD_and_filters_work()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var repo = new JsonParqueoRepository(tempDir);

            var p1 = new PrqParqueo { Id = 1, ProvinceName = "Lima", Name = "Central", PricePerHour = 5.5m };
            var p2 = new PrqParqueo { Id = 2, ProvinceName = "Cusco", Name = "Andes", PricePerHour = 10m };

            await repo.AddAsync(p1);
            await repo.AddAsync(p2);

            var all = (await repo.GetAllAsync()).ToList();
            Assert.Equal(2, all.Count);

            var byProvince = (await repo.ListByProvinceAsync("lim")).ToList();
            Assert.Single(byProvince);

            var byPrice = (await repo.ListByPriceRangeAsync(5m, 6m)).ToList();
            Assert.Single(byPrice);

            // Update
            p1.Name = "Central2";
            await repo.UpdateAsync(p1);
            var fetched = await repo.GetByIdAsync(1);
            Assert.Equal("Central2", fetched!.Name);

            // Delete
            await repo.DeleteAsync(2);
            var afterDel = (await repo.GetAllAsync()).ToList();
            Assert.Single(afterDel);

            Directory.Delete(tempDir, true);
        }

        [Fact]
        public async Task DbParqueo_ListByName_works()
        {
            var options = new DbContextOptionsBuilder<efscaffold.Models.Prq.PrqDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var db = new efscaffold.Models.Prq.PrqDbContext(options);
            db.PRQ_Parqueo.AddRange(new PrqParqueo { Id = 1, ProvinceName = "X", Name = "Foo", PricePerHour = 1m }, new PrqParqueo { Id = 2, ProvinceName = "Y", Name = "Bar", PricePerHour = 2m });
            await db.SaveChangesAsync();

            var repo = new DbParqueoRepository(db);
            var res = (await repo.ListByNameAsync("oo")).ToList();
            Assert.Single(res);
        }
    }
}