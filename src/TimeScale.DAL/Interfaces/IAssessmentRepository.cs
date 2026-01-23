using TimeScale.DAL.Entities;
using TimeScale.Shared.Models;

namespace TimeScale.DAL.Interfaces
{
    public interface IAssessmentRepository
    {
        Task SaveCSVDataAsync(string fileName, 
            List<ValueEntity> valuesList, 
            ResultEntity resultEntity,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<ResultEntity>> GetFilteredResultsAsync(ResultFilterDto filter, 
            CancellationToken cancellationToken);

        Task<IReadOnlyList<ValueEntity>> GetLastValuesAsync(string fileName, 
            CancellationToken cancellationToken);
    }
}
