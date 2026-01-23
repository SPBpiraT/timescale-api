using Microsoft.AspNetCore.Mvc;
using TimeScale.Shared.Models;

namespace TimeScale.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TimeScaleController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> UploadDataFromCsv(IFormFile file)
        {
            return StatusCode(200);
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredResults([FromQuery] ResultFilterDto filter)
        {
            return StatusCode(200);
        }

        [HttpGet]
        public async Task<IActionResult> GetLastValues([FromQuery] string fileName)
        {
            return StatusCode(200);
        }
    }
}
