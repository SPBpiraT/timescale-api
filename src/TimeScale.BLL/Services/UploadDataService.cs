using CsvHelper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using TimeScale.BLL.Exceptions;
using TimeScale.BLL.Helpers;
using TimeScale.BLL.Interfaces;
using TimeScale.BLL.Mapping;
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

        public async Task LoadDataFromCSVAsync(IFormFile file, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var fileName = file.FileName;

                _logger.LogInformation($"File processing started. Filename: {fileName}.");

                var valuesDtoList = await CSVFileHelper.ParseCsvAsync(file, _valueValidator, fileName);

                if (valuesDtoList.Count < 1 || valuesDtoList.Count > 10000)
                {
                    throw new ValidationAppException($"The number of rows must be between 1 and 10,000. Filename: {fileName}.");
                }

                var valuesList = new List<ValueEntity>();
                foreach (var dto in valuesDtoList)
                {
                    valuesList.Add(dto.MapToEntity());
                }

                var resultEntity = MapResults(fileName, valuesDtoList);

                await _assessmentRepository.SaveCSVDataAsync(fileName, valuesList, resultEntity, cancellationToken);

                _logger.LogInformation($"The data was uploaded successfully. Filename: {fileName}.");
            }
            catch (CsvHelperException ex)
            {
                var details = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, $"CSV Helper error. Details: {details}.");
                throw new ServiceAppException("An error occurred while processing the CSV file.");
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogError(ex, $"CSV data validation error. Details: {ex.Message}.");
                throw new ValidationAppException($"CSV data validation error. Details: {ex.Message}.");
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is DbException)
            {
                var details = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, $"Internal database error. Details: {details}.");
                throw new DatabaseOperationException($"Internal database error.");
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
