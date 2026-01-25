using CsvHelper;
using CsvHelper.Configuration;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using TimeScale.BLL.Mapping.CSVHelper;
using TimeScale.BLL.Models.Value;

namespace TimeScale.BLL.Helpers
{
    internal static class CSVFileHelper
    {
        internal static async Task<List<ValueDto>> ParseCsvAsync(IFormFile file,
            IValidator<ValueDto> valueValidator,
            string fileName,
            CancellationToken cancellationToken = default)
        {
            if (valueValidator == null) throw new ArgumentNullException(nameof(valueValidator));

            var valuesDtoList = new List<ValueDto>();

            using var stream = file.OpenReadStream();
            using var streamReader = new StreamReader(stream);

            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                MissingFieldFound = null,
                HeaderValidated = null,
                BadDataFound = c => throw new Exception($"Bad data.")

            };

            using var csvReader = new CsvReader(streamReader, csvConfiguration);
            csvReader.Context.RegisterClassMap<ValueCsvMapping>();
            var rowNumber = 0;

            await foreach (var valueDto in csvReader
                .GetRecordsAsync<ValueDto>()
                .WithCancellation(cancellationToken))
            {
                rowNumber = csvReader.Parser.Row;
                var validationResult = await valueValidator.ValidateAsync(valueDto, cancellationToken);

                if (!validationResult.IsValid)
                {
                    var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    throw new Exception($"CSV validation error at row {rowNumber}: {errors}"); //TODO: Create custom exception
                }

                valueDto.FileName = fileName;
                valuesDtoList.Add(valueDto);
            }

            return valuesDtoList;
        }
    }
}
