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

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> UploadDataFromCsv(IFormFile file, 
            CancellationToken cancellationToken)
        {
            var response = await _uploadDataService.LoadDataFromCSVAsync(file, cancellationToken);
            return response;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IReadOnlyList<ResultDto>>>> GetFilteredResults([FromQuery] ResultFilterDto filter,
            CancellationToken cancellationToken)
        {
            var response = await _fetchDataService.GetFilteredResultsAsync(filter, cancellationToken);
            return response;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IReadOnlyList<ValueDto>>>> GetLastValues([FromQuery] string fileName,
            CancellationToken cancellationToken)
        {
            var response = await _fetchDataService.GetLastValuesAsync(fileName, cancellationToken);
            return response;
        }
    }
}
