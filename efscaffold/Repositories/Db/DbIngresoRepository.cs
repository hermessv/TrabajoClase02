using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using efscaffold.Models.Prq;
using efscaffold.Repositories.Dtos;
using System.Linq;
using System;

namespace efscaffold.Repositories.Db
{
    public class DbIngresoRepository : IIngresoRepository
    {
        private readonly PrqDbContext _db;
        public DbIngresoRepository(PrqDbContext db) => _db = db;

        public async Task AddAsync(PrqIngresoAutomovil entity)
        {
            _db.PRQ_IngresoAutomoviles.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _db.PRQ_IngresoAutomoviles.FindAsync(id);
            if (e != null) { _db.PRQ_IngresoAutomoviles.Remove(e); await _db.SaveChangesAsync(); }
        }

        public async Task<IEnumerable<PrqIngresoAutomovil>> GetAllAsync() => await _db.PRQ_IngresoAutomoviles.ToListAsync();

        public async Task<PrqIngresoAutomovil?> GetByIdAsync(int id) => await _db.PRQ_IngresoAutomoviles.FindAsync(id);

        public async Task UpdateAsync(PrqIngresoAutomovil entity)
        {
            _db.PRQ_IngresoAutomoviles.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<decimal?> GetPricePerHourByParkingIdAsync(int parkingId)
        {
            var p = await _db.PRQ_Parqueo.FindAsync(parkingId);
            return p?.PricePerHour;
        }

        public async Task<IEnumerable<IngresoResultDto>> ListByVehicleTypeBetweenDatesAsync(string type, DateTime start, DateTime end)
        {
            string pattern = $"%{type}%";
            var q = from i in _db.PRQ_IngresoAutomoviles
                    join a in _db.PRQ_Automoviles on i.PrqAutomovilId equals a.Id
                    join p in _db.PRQ_Parqueo on i.PrqParqueoId equals p.Id
                    where EF.Functions.Like(a.Type, pattern) && i.EntryDateTime >= start && i.EntryDateTime <= end
                    select new IngresoResultDto
                    {
                        AutomovilId = a.Id,
                        Tipo = a.Type,
                        ParqueoId = p.Id,
                        Provincia = p.ProvinceName,
                        FechaEntrada = i.EntryDateTime,
                        FechaSalida = i.ExitDateTime,
                        MontoTotal = i.ExitDateTime.HasValue ? (decimal?) Math.Round(((decimal) ( (decimal?) ( (i.ExitDateTime.Value - i.EntryDateTime).TotalMinutes ) / 60m)) * p.PricePerHour, 2) : null
                    };
            return await q.ToListAsync();
        }

        public async Task<IEnumerable<IngresoResultDto>> ListByProvinceBetweenDatesAsync(string province, DateTime start, DateTime end)
        {
            string pattern = $"%{province}%";
            var q = from i in _db.PRQ_IngresoAutomoviles
                    join a in _db.PRQ_Automoviles on i.PrqAutomovilId equals a.Id
                    join p in _db.PRQ_Parqueo on i.PrqParqueoId equals p.Id
                    where EF.Functions.Like(p.ProvinceName, pattern) && i.EntryDateTime >= start && i.EntryDateTime <= end
                    select new IngresoResultDto
                    {
                        AutomovilId = a.Id,
                        Tipo = a.Type,
                        ParqueoId = p.Id,
                        Provincia = p.ProvinceName,
                        FechaEntrada = i.EntryDateTime,
                        FechaSalida = i.ExitDateTime,
                        MontoTotal = i.ExitDateTime.HasValue ? (decimal?) Math.Round(((decimal) ( (decimal?) ( (i.ExitDateTime.Value - i.EntryDateTime).TotalMinutes ) / 60m)) * p.PricePerHour, 2) : null
                    };
            return await q.ToListAsync();
        }
    }
}