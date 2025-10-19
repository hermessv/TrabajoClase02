using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using efscaffold.Models.Prq;

namespace efscaffold.Repositories
{
    public interface IAutomovilesRepository : IRepository<PrqAutomovil>
    {
        Task<IEnumerable<PrqAutomovil>> ListByColorAsync(string? color);
        Task<IEnumerable<PrqAutomovil>> ListByYearRangeAsync(short? yearFrom, short? yearTo);
        Task<IEnumerable<PrqAutomovil>> ListByManufacturerAsync(string? manufacturer);
        Task<IEnumerable<PrqAutomovil>> ListByTypeAsync(string? type);
    }
}