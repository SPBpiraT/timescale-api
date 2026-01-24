using Microsoft.AspNetCore.Http;
using TimeScale.BLL.Models;

namespace TimeScale.BLL.Interfaces
{
    public interface IUploadDataService
    {
        Task<ServiceResponse> LoadDataFromCSVAsync(IFormFile file, CancellationToken cancellationToken);
    }
}

