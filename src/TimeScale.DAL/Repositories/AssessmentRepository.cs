using Microsoft.EntityFrameworkCore;
using TimeScale.DAL.EF;
using TimeScale.DAL.Entities;
using TimeScale.DAL.Interfaces;
using TimeScale.Shared.Models;

namespace TimeScale.DAL.Repositories
{
    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly AppDbContext _appDbContext;
        public AssessmentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IReadOnlyList<ResultEntity>> GetFilteredResultsAsync(ResultFilterDto filter, 
            CancellationToken cancellationToken = default)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var query = _appDbContext.Results.AsNoTracking();

            query = query
                .Where(r => string.IsNullOrWhiteSpace(filter.FileName) || r.FileName == filter.FileName)
                .Where(r => !filter.StartDate.HasValue || r.FirstOperationDate >= filter.StartDate.Value)
                .Where(r => !filter.EndDate.HasValue || r.FirstOperationDate <= filter.EndDate.Value)
                .Where(r => !filter.MinAvgValue.HasValue || r.AvgValue >= filter.MinAvgValue.Value)
                .Where(r => !filter.MaxAvgValue.HasValue || r.AvgValue <= filter.MaxAvgValue.Value)
                .Where(r => !filter.MinAvgExecutionTime.HasValue || r.AvgExecutionTime >= filter.MinAvgExecutionTime.Value)
                .Where(r => !filter.MaxAvgExecutionTime.HasValue || r.AvgExecutionTime <= filter.MaxAvgExecutionTime.Value);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ValueEntity>> GetLastValuesAsync(string fileName, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return Array.Empty<ValueEntity>();
            }

            return await _appDbContext.Values
                .AsNoTracking()
                .Where(v => v.FileName == fileName)
                .OrderByDescending(v => v.Date)
                .Take(10)
                .ToListAsync(cancellationToken);
        }

        public async Task SaveCSVDataAsync(string fileName, 
            List<ValueEntity> valuesList, 
            ResultEntity resultEntity,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be empty", nameof(fileName));

            if (valuesList == null)
                throw new ArgumentNullException(nameof(valuesList));

            if (resultEntity == null)
                throw new ArgumentNullException(nameof(resultEntity));

            await using var transaction = await _appDbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var existingResult = await _appDbContext.Results
                    .Where(r => r.FileName == fileName)
                    .Select(r => new { r.Id })
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingResult != null)
                {
                    await _appDbContext.Values
                        .Where(v => v.FileName == fileName)
                        .ExecuteDeleteAsync(cancellationToken);

                    await _appDbContext.Results
                        .Where(r => r.FileName == fileName)
                        .ExecuteDeleteAsync(cancellationToken);
                }

                await _appDbContext.Values.AddRangeAsync(valuesList, cancellationToken);
                await _appDbContext.Results.AddAsync(resultEntity, cancellationToken);

                await _appDbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(CancellationToken.None);
                throw;
            }
        }
    }
}
