using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using efscaffold.Models.Prq;
using efscaffold.Repositories.Dtos;

namespace efscaffold.Repositories
{
    public interface IIngresoRepository : IRepository<PrqIngresoAutomovil>
    {
        Task<decimal?> GetPricePerHourByParkingIdAsync(int parkingId);
        Task<IEnumerable<IngresoResultDto>> ListByVehicleTypeBetweenDatesAsync(string type, DateTime start, DateTime end);
        Task<IEnumerable<IngresoResultDto>> ListByProvinceBetweenDatesAsync(string province, DateTime start, DateTime end);
    }
}