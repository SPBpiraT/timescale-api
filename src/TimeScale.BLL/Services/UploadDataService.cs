using Microsoft.AspNetCore.Http;
using TimeScale.BLL.Interfaces;

namespace TimeScale.BLL.Services
{
    internal class UploadDataService : IUploadDataService
    {
        public Task<string> LoadDataFromCSVAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
