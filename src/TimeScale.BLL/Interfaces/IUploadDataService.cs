using Microsoft.AspNetCore.Http;

namespace TimeScale.BLL.Interfaces
{
    public interface IUploadDataService
    {
        Task<string> LoadDataFromCSVAsync(IFormFile file);
    }
}

