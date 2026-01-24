using TimeScale.BLL.Models;
using TimeScale.BLL.Models.Result;
using TimeScale.BLL.Models.Value;
using TimeScale.Shared.Models;

namespace TimeScale.BLL.Interfaces
{
    public interface IFetchDataService
    {
        Task<ServiceResponse<IEnumerable<ResultDto>>> GetFilteredResultsAsync(ResultFilterDto filter, CancellationToken cancellationToken);
        Task<ServiceResponse<IEnumerable<ValueDto>>> GetLastValuesAsync(string fileName, CancellationToken cancellationToken);
    }
}
