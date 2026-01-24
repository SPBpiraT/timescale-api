using TimeScale.BLL.Interfaces;
using TimeScale.BLL.Models;
using TimeScale.BLL.Models.Result;
using TimeScale.BLL.Models.Value;
using TimeScale.Shared.Models;

namespace TimeScale.BLL.Services
{
    internal class FetchDataService : IFetchDataService
    {
        public async Task<ServiceResponse<IEnumerable<ResultDto>>> GetFilteredResultsAsync(ResultFilterDto filter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<IEnumerable<ValueDto>>> GetLastValuesAsync(string fileName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
