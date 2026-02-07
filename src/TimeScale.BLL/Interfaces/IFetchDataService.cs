using TimeScale.BLL.Models;
using TimeScale.BLL.Models.Result;
using TimeScale.BLL.Models.Value;
using TimeScale.Shared.Models;

namespace TimeScale.BLL.Interfaces
{
    public interface IFetchDataService
    {
        Task<IReadOnlyList<ResultDto>> GetFilteredResultsAsync(ResultFilterDto filter, CancellationToken cancellationToken);
        Task<IReadOnlyList<ValueDto>> GetLastValuesAsync(string fileName, CancellationToken cancellationToken);
    }
}
