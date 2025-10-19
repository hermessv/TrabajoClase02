using System.Collections.Generic;
using System.Threading.Tasks;
using efscaffold.Models.Prq;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace efscaffold.Repositories.Json
{
    public class JsonParqueoRepository : IParqueoRepository
    {
        private readonly string _path;
        public JsonParqueoRepository(string baseDirectory)
        {
            _path = Path.Combine(baseDirectory, "prq_parqueo.json");
        }

        private async Task<List<PrqParqueo>> LoadAsync()
        {
            if (!File.Exists(_path)) return new List<PrqParqueo>();
            using var s = File.OpenRead(_path);
            return await JsonSerializer.DeserializeAsync<List<PrqParqueo>>(s) ?? new List<PrqParqueo>();
        }

        public async Task AddAsync(PrqParqueo entity)
        {
            var list = await LoadAsync();
            list.Add(entity);
            await File.WriteAllTextAsync(_path, JsonSerializer.Serialize(list, new JsonSerializerOptions{WriteIndented=true}));
        }

        public async Task DeleteAsync(int id)
        {
            var list = await LoadAsync();
            var e = list.FirstOrDefault(x => x.Id == id);
            if (e != null) { list.Remove(e); await File.WriteAllTextAsync(_path, JsonSerializer.Serialize(list, new JsonSerializerOptions{WriteIndented=true})); }
        }

        public async Task<IEnumerable<PrqParqueo>> GetAllAsync() => await LoadAsync();

        public async Task<PrqParqueo?> GetByIdAsync(int id)
        {
            var list = await LoadAsync();
            return list.FirstOrDefault(x => x.Id == id);
        }

        public async Task UpdateAsync(PrqParqueo entity)
        {
            var list = await LoadAsync();
            var idx = list.FindIndex(x => x.Id == entity.Id);
            if (idx >= 0) { list[idx] = entity; await File.WriteAllTextAsync(_path, JsonSerializer.Serialize(list, new JsonSerializerOptions{WriteIndented=true})); }
        }

        public async Task<IEnumerable<PrqParqueo>> ListByNameAsync(string? name)
        {
            var list = await LoadAsync();
            if (string.IsNullOrWhiteSpace(name)) return list;
            return list.Where(p => p.Name != null && p.Name.Contains(name, System.StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<PrqParqueo>> ListByPriceRangeAsync(decimal? minPrice, decimal? maxPrice)
        {
            var list = await LoadAsync();
            var q = list.AsEnumerable();
            if (minPrice.HasValue) q = q.Where(p => p.PricePerHour >= minPrice.Value);
            if (maxPrice.HasValue) q = q.Where(p => p.PricePerHour <= maxPrice.Value);
            return q;
        }

        public async Task<IEnumerable<PrqParqueo>> ListByProvinceAsync(string? province)
        {
            var list = await LoadAsync();
            if (string.IsNullOrWhiteSpace(province)) return list;
            return list.Where(p => p.ProvinceName != null && p.ProvinceName.Contains(province, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}