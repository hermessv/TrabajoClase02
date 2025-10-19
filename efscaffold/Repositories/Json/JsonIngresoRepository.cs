using System.Collections.Generic;
using System.Threading.Tasks;
using efscaffold.Models.Prq;
using efscaffold.Repositories.Dtos;
using System.IO;
using System.Text.Json;
using System.Linq;
using System;

namespace efscaffold.Repositories.Json
{
    public class JsonIngresoRepository : IIngresoRepository
    {
        private readonly string _baseDir;
        private string AutomovilesPath => Path.Combine(_baseDir, "prq_automoviles.json");
        private string ParqueoPath => Path.Combine(_baseDir, "prq_parqueo.json");
        private string IngresoPath => Path.Combine(_baseDir, "prq_ingresoautomoviles.json");

        public JsonIngresoRepository(string baseDirectory)
        {
            _baseDir = baseDirectory;
        }

        private async Task<List<T>> LoadListAsync<T>(string path)
        {
            if (!File.Exists(path)) return new List<T>();
            using var s = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<List<T>>(s) ?? new List<T>();
        }

        public async Task AddAsync(PrqIngresoAutomovil entity)
        {
            var list = await LoadListAsync<PrqIngresoAutomovil>(IngresoPath);
            list.Add(entity);
            await File.WriteAllTextAsync(IngresoPath, JsonSerializer.Serialize(list, new JsonSerializerOptions{WriteIndented=true}));
        }

        public async Task DeleteAsync(int id)
        {
            var list = await LoadListAsync<PrqIngresoAutomovil>(IngresoPath);
            var e = list.FirstOrDefault(x => x.Consecutive == id);
            if (e != null) { list.Remove(e); await File.WriteAllTextAsync(IngresoPath, JsonSerializer.Serialize(list, new JsonSerializerOptions{WriteIndented=true})); }
        }

        public async Task<IEnumerable<PrqIngresoAutomovil>> GetAllAsync() => await LoadListAsync<PrqIngresoAutomovil>(IngresoPath);

        public async Task<PrqIngresoAutomovil?> GetByIdAsync(int id)
        {
            var list = await LoadListAsync<PrqIngresoAutomovil>(IngresoPath);
            return list.FirstOrDefault(x => x.Consecutive == id);
        }

        public async Task UpdateAsync(PrqIngresoAutomovil entity)
        {
            var list = await LoadListAsync<PrqIngresoAutomovil>(IngresoPath);
            var idx = list.FindIndex(x => x.Consecutive == entity.Consecutive);
            if (idx >= 0) { list[idx] = entity; await File.WriteAllTextAsync(IngresoPath, JsonSerializer.Serialize(list, new JsonSerializerOptions{WriteIndented=true})); }
        }

        public async Task<decimal?> GetPricePerHourByParkingIdAsync(int parkingId)
        {
            var parques = await LoadListAsync<PrqParqueo>(ParqueoPath);
            return parques.FirstOrDefault(p => p.Id == parkingId)?.PricePerHour;
        }

        public async Task<IEnumerable<IngresoResultDto>> ListByVehicleTypeBetweenDatesAsync(string type, DateTime start, DateTime end)
        {
            var autos = await LoadListAsync<PrqAutomovil>(AutomovilesPath);
            var parques = await LoadListAsync<PrqParqueo>(ParqueoPath);
            var ingresos = await LoadListAsync<PrqIngresoAutomovil>(IngresoPath);

            var q = from i in ingresos
                    join a in autos on i.PrqAutomovilId equals a.Id
                    join p in parques on i.PrqParqueoId equals p.Id
                    where a.Type != null && a.Type.IndexOf(type ?? string.Empty, StringComparison.OrdinalIgnoreCase) >= 0
                          && i.EntryDateTime >= start && i.EntryDateTime <= end
                    select new IngresoResultDto
                    {
                        AutomovilId = a.Id,
                        Tipo = a.Type ?? string.Empty,
                        ParqueoId = p.Id,
                        Provincia = p.ProvinceName ?? string.Empty,
                        FechaEntrada = i.EntryDateTime,
                        FechaSalida = i.ExitDateTime,
                        MontoTotal = i.ExitDateTime.HasValue ? (decimal?) Math.Round((decimal)(((i.ExitDateTime.Value - i.EntryDateTime).TotalMinutes / 60.0) * (double)p.PricePerHour), 2) : null
                    };

            return q;
        }

        public async Task<IEnumerable<IngresoResultDto>> ListByProvinceBetweenDatesAsync(string province, DateTime start, DateTime end)
        {
            var autos = await LoadListAsync<PrqAutomovil>(AutomovilesPath);
            var parques = await LoadListAsync<PrqParqueo>(ParqueoPath);
            var ingresos = await LoadListAsync<PrqIngresoAutomovil>(IngresoPath);

            var q = from i in ingresos
                    join a in autos on i.PrqAutomovilId equals a.Id
                    join p in parques on i.PrqParqueoId equals p.Id
                    where p.ProvinceName != null && p.ProvinceName.IndexOf(province ?? string.Empty, StringComparison.OrdinalIgnoreCase) >= 0
                          && i.EntryDateTime >= start && i.EntryDateTime <= end
                    select new IngresoResultDto
                    {
                        AutomovilId = a.Id,
                        Tipo = a.Type ?? string.Empty,
                        ParqueoId = p.Id,
                        Provincia = p.ProvinceName ?? string.Empty,
                        FechaEntrada = i.EntryDateTime,
                        FechaSalida = i.ExitDateTime,
                        MontoTotal = i.ExitDateTime.HasValue ? (decimal?) Math.Round((decimal)(((i.ExitDateTime.Value - i.EntryDateTime).TotalMinutes / 60.0) * (double)p.PricePerHour), 2) : null
                    };

            return q;
        }
    }
}