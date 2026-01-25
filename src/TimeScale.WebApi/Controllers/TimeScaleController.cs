using Microsoft.AspNetCore.Mvc;
using TimeScale.BLL.Interfaces;
using TimeScale.BLL.Models;
using TimeScale.BLL.Models.Result;
using TimeScale.BLL.Models.Value;
using TimeScale.Shared.Models;

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
        [HttpPost]
        [RequestSizeLimit(10_485_760)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceResponse>> UploadDataFromCsv(IFormFile file, 
            CancellationToken cancellationToken)
        {
            var response = await _uploadDataService.LoadDataFromCSVAsync(file, cancellationToken);

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
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
        /// <response code="400">Invalid filter parameters</response>
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceResponse<IReadOnlyList<ResultDto>>>> GetFilteredResults([FromQuery] ResultFilterDto filter,
            CancellationToken cancellationToken)
        {
            var response = await _fetchDataService.GetFilteredResultsAsync(filter, cancellationToken);

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
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
        /// <response code="400">Invalid file name</response>
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServiceResponse<IReadOnlyList<ValueDto>>>> GetLastValues([FromQuery] string fileName,
            CancellationToken cancellationToken)
        {
            var response = await _fetchDataService.GetLastValuesAsync(fileName, cancellationToken);

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
