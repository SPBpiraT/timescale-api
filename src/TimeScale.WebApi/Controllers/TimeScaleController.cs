using Microsoft.AspNetCore.Mvc;
using TimeScale.BLL.Exceptions;
using TimeScale.BLL.Interfaces;
using TimeScale.BLL.Models.Result;
using TimeScale.BLL.Models.Value;
using TimeScale.Shared.Models;
using TimeScale.WebApi.Models;

namespace TimeScale.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TimeScaleController : Controller
    {
        private readonly IUploadDataService _uploadDataService;
        private readonly IFetchDataService _fetchDataService;
        private readonly ILogger<TimeScaleController> _logger;

        public TimeScaleController(IUploadDataService uploadDataService,
            IFetchDataService fetchDataService,
            ILogger<TimeScaleController> logger)
        {
            _uploadDataService = uploadDataService;
            _fetchDataService = fetchDataService;
            _logger = logger;
        }

        /// <summary>
        /// Uploads data from CSV file
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /api/TimeScale/UploadDataFromCsv
        /// Content-Type: multipart/form-data
        /// </remarks>
        /// <param name="file">CSV file with data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Operation result</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid CSV file</response>
        /// <response code="413">File too large</response>
        /// <response code="422">Data validation failed</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 10_485_760)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UploadDataFromCsv(IFormFile file, 
            CancellationToken cancellationToken)
        {
            var allowedExtensions = new[] { ".csv" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                var response = new ApiResponse(false, 400, $"Unsupported file extension. Allowed extensions: {string.Join(",", allowedExtensions)}");
                return BadRequest(response);
            }

            if (file == null || file.Length == 0)
            {
                var response = new ApiResponse(false, 400, "File is missing or has no content.");
                return BadRequest(response);
            }

            try
            {
                await _uploadDataService.LoadDataFromCSVAsync(file, cancellationToken);
                var response = new ApiResponse(true, 200, "The data was uploaded successfully.");
                return Ok(response);
            }
            catch (ServiceAppException)
            {
                var response = new ApiResponse(false, 400, "An error occurred while processing the CSV file.");
                return BadRequest(response);
            }
            catch (ValidationAppException)
            {
                var response = new ApiResponse(false, 422, "CSV data validation error.");
                return UnprocessableEntity(response);
            }
            catch (DatabaseOperationException)
            {
                var response = new ApiResponse(false, 500, "Service is temporarily unavailable. Please try again later.");
                return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                var details = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, $"An error occurred while processing the file. Details: {details}.");
                var response = new ApiResponse(false, 400, "An error occurred while processing the file.");
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Gets filtered results
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/TimeScale/GetFilteredResults?FileName=test.csv&StartDate=2024-01-01
        /// </remarks>
        /// <param name="filter">Filter parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Filtered results</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFilteredResults([FromQuery] ResultFilterDto filter,
            CancellationToken cancellationToken)
        {
            try
            {
                var results = await _fetchDataService.GetFilteredResultsAsync(filter, cancellationToken);
                var response = new ApiResponse<IReadOnlyList<ResultDto>>(true, 200, "Results retrieved successfully.", results);
                return Ok(response);
            }
            catch (DatabaseOperationException)
            {
                var response = new ApiResponse(false, 500, "Service is temporarily unavailable. Please try again later.");
                return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                var details = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, $"Unexpected exception during [GetFilteredResultsAsync]. Details: {details}.");
                var response = new ApiResponse(false, 500, "An unexpected error occurred while retrieving data. Please try again later.");
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// Gets last values for a file
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/TimeScale/GetLastValues?FileName=data.csv
        /// </remarks>
        /// <param name="fileName">File name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Last values</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLastValues([FromQuery] string fileName,
            CancellationToken cancellationToken)
        {
            try
            {
                var values = await _fetchDataService.GetLastValuesAsync(fileName, cancellationToken);
                var response = new ApiResponse<IReadOnlyList<ValueDto>>(true, 200, "Values retrieved successfully.", values);
                return Ok(response);
            }
            catch (DatabaseOperationException)
            {
                var response = new ApiResponse(false, 500, "Service is temporarily unavailable. Please try again later.");
                return StatusCode(500, response);
            }
            catch (Exception ex)
            {
                var details = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError(ex, $"Unexpected exception during [GetLastValuesAsync]. Details: {details}.");
                var response = new ApiResponse(false, 500, "An unexpected error occurred while retrieving data. Please try again later.");
                return StatusCode(500, response);
            }
        }
    }
}
