using System.Collections.Generic;
using System.Threading.Tasks;
using efscaffold.Models.Prq;

namespace efscaffold.Repositories
{
    public interface IParqueoRepository : IRepository<PrqParqueo>
    {
        Task<IEnumerable<PrqParqueo>> ListByProvinceAsync(string? province);
        Task<IEnumerable<PrqParqueo>> ListByNameAsync(string? name);
        Task<IEnumerable<PrqParqueo>> ListByPriceRangeAsync(decimal? minPrice, decimal? maxPrice);
    }
}