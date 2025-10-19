using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using efscaffold.Models.Prq;
using System.Linq;

namespace efscaffold.Repositories.Db
{
    public class DbAutomovilesRepository : IAutomovilesRepository
    {
        private readonly PrqDbContext _db;
        public DbAutomovilesRepository(PrqDbContext db) => _db = db;

        public async Task AddAsync(PrqAutomovil entity)
        {
            _db.PRQ_Automoviles.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _db.PRQ_Automoviles.FindAsync(id);
            if (e != null) { _db.PRQ_Automoviles.Remove(e); await _db.SaveChangesAsync(); }
        }

        public async Task<IEnumerable<PrqAutomovil>> GetAllAsync() => await _db.PRQ_Automoviles.ToListAsync();

        public async Task<PrqAutomovil?> GetByIdAsync(int id) => await _db.PRQ_Automoviles.FindAsync(id);

        public async Task UpdateAsync(PrqAutomovil entity)
        {
            _db.PRQ_Automoviles.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<PrqAutomovil>> ListByColorAsync(string? color)
        {
            if (string.IsNullOrWhiteSpace(color)) return await GetAllAsync();
            string pattern = $"%{color}%";
            return await _db.PRQ_Automoviles.Where(a => EF.Functions.Like(a.Color, pattern)).ToListAsync();
        }

        public async Task<IEnumerable<PrqAutomovil>> ListByManufacturerAsync(string? manufacturer)
        {
            if (string.IsNullOrWhiteSpace(manufacturer)) return await GetAllAsync();
            string pattern = $"%{manufacturer}%";
            return await _db.PRQ_Automoviles.Where(a => EF.Functions.Like(a.Manufacturer, pattern)).ToListAsync();
        }

        public async Task<IEnumerable<PrqAutomovil>> ListByTypeAsync(string? type)
        {
            if (string.IsNullOrWhiteSpace(type)) return await GetAllAsync();
            string pattern = $"%{type}%";
            return await _db.PRQ_Automoviles.Where(a => EF.Functions.Like(a.Type, pattern)).ToListAsync();
        }

        public async Task<IEnumerable<PrqAutomovil>> ListByYearRangeAsync(short? yearFrom, short? yearTo)
        {
            var q = _db.PRQ_Automoviles.AsQueryable();
            if (yearFrom.HasValue) q = q.Where(a => a.Year >= yearFrom.Value);
            if (yearTo.HasValue) q = q.Where(a => a.Year <= yearTo.Value);
            return await q.ToListAsync();
        }
    }
}