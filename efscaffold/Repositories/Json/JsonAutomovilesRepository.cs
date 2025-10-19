using System.Collections.Generic;
using System.Threading.Tasks;
using efscaffold.Models.Prq;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace efscaffold.Repositories.Json
{
    public class JsonAutomovilesRepository : IAutomovilesRepository
    {
        private readonly string _path;
        public JsonAutomovilesRepository(string baseDirectory)
        {
            _path = Path.Combine(baseDirectory, "prq_automoviles.json");
        }

        private async Task<List<PrqAutomovil>> LoadAsync()
        {
            if (!File.Exists(_path)) return new List<PrqAutomovil>();
            using var s = File.OpenRead(_path);
            return await JsonSerializer.DeserializeAsync<List<PrqAutomovil>>(s) ?? new List<PrqAutomovil>();
        }

        public async Task AddAsync(PrqAutomovil entity)
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

        public async Task<IEnumerable<PrqAutomovil>> GetAllAsync() => await LoadAsync();

        public async Task<PrqAutomovil?> GetByIdAsync(int id)
        {
            var list = await LoadAsync();
            return list.FirstOrDefault(x => x.Id == id);
        }

        public async Task UpdateAsync(PrqAutomovil entity)
        {
            var list = await LoadAsync();
            var idx = list.FindIndex(x => x.Id == entity.Id);
            if (idx >= 0) { list[idx] = entity; await File.WriteAllTextAsync(_path, JsonSerializer.Serialize(list, new JsonSerializerOptions{WriteIndented=true})); }
        }

        public async Task<IEnumerable<PrqAutomovil>> ListByColorAsync(string? color)
        {
            var list = await LoadAsync();
            if (string.IsNullOrWhiteSpace(color)) return list;
            return list.Where(a => a.Color != null && a.Color.Contains(color, System.StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<PrqAutomovil>> ListByManufacturerAsync(string? manufacturer)
        {
            var list = await LoadAsync();
            if (string.IsNullOrWhiteSpace(manufacturer)) return list;
            return list.Where(a => a.Manufacturer != null && a.Manufacturer.Contains(manufacturer, System.StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<PrqAutomovil>> ListByTypeAsync(string? type)
        {
            var list = await LoadAsync();
            if (string.IsNullOrWhiteSpace(type)) return list;
            return list.Where(a => a.Type != null && a.Type.Contains(type, System.StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<PrqAutomovil>> ListByYearRangeAsync(short? yearFrom, short? yearTo)
        {
            var list = await LoadAsync();
            var q = list.AsEnumerable();
            if (yearFrom.HasValue) q = q.Where(a => a.Year >= yearFrom.Value);
            if (yearTo.HasValue) q = q.Where(a => a.Year <= yearTo.Value);
            return q;
        }
    }
}