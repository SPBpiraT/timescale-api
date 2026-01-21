using TimeScale.BLL.Models.Result;
using TimeScale.BLL.Models.Value;

namespace TimeScale.BLL.Interfaces
{
    public interface IFetchDataService
    {
        Task<IEnumerable<ResultDto>> GetFilteredResultsAsync(ResultFilterDto filter);
        Task<IEnumerable<ValueDto>> GetLastValuesAsync(string fileName);
    }
}
