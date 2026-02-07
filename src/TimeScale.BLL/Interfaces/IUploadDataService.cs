using Microsoft.AspNetCore.Http;
using TimeScale.BLL.Models;

namespace TimeScale.BLL.Interfaces
{
    public interface IUploadDataService
    {
        Task LoadDataFromCSVAsync(IFormFile file, CancellationToken cancellationToken);
    }
}

