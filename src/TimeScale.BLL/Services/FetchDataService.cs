using TimeScale.BLL.Interfaces;
using TimeScale.BLL.Models.Result;
using TimeScale.BLL.Models.Value;
using TimeScale.Shared.Models;

namespace TimeScale.BLL.Services
{
    internal class FetchDataService : IFetchDataService
    {
        public async Task<IEnumerable<ResultDto>> GetFilteredResultsAsync(ResultFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ValueDto>> GetLastValuesAsync(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
