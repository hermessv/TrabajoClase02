using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using efscaffold.Models.Prq;
using System.Linq;

namespace efscaffold.Repositories.Db
{
    public class DbParqueoRepository : IParqueoRepository
    {
        private readonly PrqDbContext _db;
        public DbParqueoRepository(PrqDbContext db) => _db = db;

        public async Task AddAsync(PrqParqueo entity)
        {
            _db.PRQ_Parqueo.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _db.PRQ_Parqueo.FindAsync(id);
            if (e != null) { _db.PRQ_Parqueo.Remove(e); await _db.SaveChangesAsync(); }
        }

        public async Task<IEnumerable<PrqParqueo>> GetAllAsync() => await _db.PRQ_Parqueo.ToListAsync();

        public async Task<PrqParqueo?> GetByIdAsync(int id) => await _db.PRQ_Parqueo.FindAsync(id);

        public async Task UpdateAsync(PrqParqueo entity)
        {
            _db.PRQ_Parqueo.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<PrqParqueo>> ListByNameAsync(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return await GetAllAsync();
            string pattern = $"%{name}%";
            return await _db.PRQ_Parqueo.Where(p => EF.Functions.Like(p.Name, pattern)).ToListAsync();
        }

        public async Task<IEnumerable<PrqParqueo>> ListByPriceRangeAsync(decimal? minPrice, decimal? maxPrice)
        {
            var q = _db.PRQ_Parqueo.AsQueryable();
            if (minPrice.HasValue) q = q.Where(p => p.PricePerHour >= minPrice.Value);
            if (maxPrice.HasValue) q = q.Where(p => p.PricePerHour <= maxPrice.Value);
            return await q.ToListAsync();
        }

        public async Task<IEnumerable<PrqParqueo>> ListByProvinceAsync(string? province)
        {
            if (string.IsNullOrWhiteSpace(province)) return await GetAllAsync();
            string pattern = $"%{province}%";
            return await _db.PRQ_Parqueo.Where(p => EF.Functions.Like(p.ProvinceName, pattern)).ToListAsync();
        }
    }
}