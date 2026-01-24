using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TimeScale.BLL.Helpers;
using TimeScale.BLL.Interfaces;
using TimeScale.BLL.Mapping;
using TimeScale.BLL.Models;
using TimeScale.BLL.Models.Value;
using TimeScale.DAL.Entities;
using TimeScale.DAL.Interfaces;

namespace TimeScale.BLL.Services
{
    public class UploadDataService : IUploadDataService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IValidator<ValueDto> _valueValidator;
        private readonly ILogger<UploadDataService> _logger;

        public UploadDataService(IAssessmentRepository assessmentRepository,
            IValidator<ValueDto> valueValidator,
            ILogger<UploadDataService> logger)
        {
            _assessmentRepository = assessmentRepository;
            _valueValidator = valueValidator;
            _logger = logger;
        }

        public async Task<ServiceResponse> LoadDataFromCSVAsync(IFormFile file, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    throw new ArgumentException("No file provided.");
                }

                var fileName = file.FileName;

                _logger.LogInformation($"File processing started. Filename: {fileName}.");

                var valuesDtoList = await CSVFileHelper.ParseCsvAsync(file, _valueValidator, fileName);

                if (valuesDtoList.Count < 1 || valuesDtoList.Count > 10000)
                {
                    throw new Exception($"The number of rows must be between 1 and 10,000. Filename: {fileName}.");
                }

                var valuesList = new List<ValueEntity>();
                foreach (var dto in valuesDtoList)
                {
                    valuesList.Add(dto.MapToEntity());
                }

                var resultEntity = MapResults(fileName, valuesDtoList);

                await _assessmentRepository.SaveCSVDataAsync(fileName, valuesList, resultEntity, cancellationToken);

                _logger.LogInformation($"The data was uploaded successfully. Filename: {fileName}.");

                return new ServiceResponse(true, 200, "The data was uploaded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the CSV file.");

                return new ServiceResponse(false, 500, "Error while processing the CSV file.");
            }
        }

        private ResultEntity MapResults(string fileName, List<ValueDto> valuesList)
        {
            var values = valuesList.Select(v => v.Value).ToList();

            var minDate = valuesList.Min(v => v.Date);
            var maxDate = valuesList.Max(v => v.Date);
            var deltaTime = (maxDate - minDate).TotalSeconds;
            var avgExecutionTime = valuesList.Average(v => v.ExecutionTime);
            var avgValue = valuesList.Average(v => v.Value);
            var medianValue = StatisticsHelper.ComputeMedianValue(values);
            var maxValue = valuesList.Max(v => v.Value);
            var minValue = valuesList.Min(v => v.Value);

            return new ResultEntity
            {
                FileName = fileName,
                DeltaTime = deltaTime,
                FirstOperationDate = minDate,
                AvgExecutionTime = avgExecutionTime,
                AvgValue = avgValue,
                MedianValue = medianValue,
                MaxValue = maxValue,
                MinValue = minValue,
            };
        }
    }
}
