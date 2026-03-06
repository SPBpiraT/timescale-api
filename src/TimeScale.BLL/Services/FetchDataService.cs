using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using TimeScale.BLL.Exceptions;
using TimeScale.BLL.Interfaces;
using TimeScale.BLL.Mapping;
using TimeScale.BLL.Models.Result;
using TimeScale.BLL.Models.Value;
using TimeScale.DAL.Interfaces;
using TimeScale.Shared.Models;

namespace TimeScale.BLL.Services
{
    public class FetchDataService : IFetchDataService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<FetchDataService> _logger;

        public FetchDataService(IAssessmentRepository assessmentRepository,
            ILogger<FetchDataService> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }
        public async Task<IReadOnlyList<ResultDto>> GetFilteredResultsAsync(ResultFilterDto filter, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var resultList = await _assessmentRepository.GetFilteredResultsAsync(filter, cancellationToken);

                var resultDtoList = new List<ResultDto>();
                foreach (var entity in resultList)
                {
                    resultDtoList.Add(entity.MapToDto());
                }

                return resultDtoList.AsReadOnly();
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is DbException)
            {
                var details = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, $"Internal database error. Details: {details}.");
                throw new DatabaseOperationException($"Internal database error.");
            }
        }

        public async Task<IReadOnlyList<ValueDto>> GetLastValuesAsync(string fileName, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var lastValues = await _assessmentRepository.GetLastValuesAsync(fileName, cancellationToken);

                var valuesDtoList = new List<ValueDto>();
                foreach (var entity in lastValues)
                {
                    valuesDtoList.Add(entity.MapToDto());
                }

                return valuesDtoList.AsReadOnly();
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is DbException)
            {
                var details = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, $"Internal database error. Details: {details}.");
                throw new DatabaseOperationException($"Internal database error.");
            }
        }
    }
}
