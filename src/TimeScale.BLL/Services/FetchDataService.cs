using Microsoft.Extensions.Logging;
using TimeScale.BLL.Interfaces;
using TimeScale.BLL.Mapping;
using TimeScale.BLL.Models;
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
        public async Task<ServiceResponse<IReadOnlyList<ResultDto>>> GetFilteredResultsAsync(ResultFilterDto filter, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var resultList = await _assessmentRepository.GetFilteredResultsAsync(filter, cancellationToken);

                if (!resultList.Any()) throw new Exception("Data not found."); //TODO: Create custom exceptions. Return Not Found

                var resultDtoList = new List<ResultDto>();
                foreach (var entity in resultList)
                {
                    resultDtoList.Add(entity.MapToDto());
                }

                return new ServiceResponse<IReadOnlyList<ResultDto>>(true, 200, "Results retrieved successfully.", resultDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve results.");
                return new ServiceResponse<IReadOnlyList<ResultDto>>(false, 400, "Error. Could not fetch results.", new List<ResultDto>());
            }
        }

        public async Task<ServiceResponse<IReadOnlyList<ValueDto>>> GetLastValuesAsync(string fileName, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var lastValues = await _assessmentRepository.GetLastValuesAsync(fileName, cancellationToken);

                if (!lastValues.Any()) throw new Exception("Data not found."); //TODO: Create custom exceptions. Return Not Found

                var valuesDtoList = new List<ValueDto>();
                foreach (var entity in lastValues)
                {
                    valuesDtoList.Add(entity.MapToDto());
                }

                return new ServiceResponse<IReadOnlyList<ValueDto>>(true, 200, "Values retrieved successfully.", valuesDtoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve values.");

                return new ServiceResponse<IReadOnlyList<ValueDto>>(false, 400, "Error. Could not fetch values.", new List<ValueDto>());
            }
        }
    }
}
