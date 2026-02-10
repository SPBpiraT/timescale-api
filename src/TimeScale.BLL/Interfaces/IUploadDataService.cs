using Microsoft.AspNetCore.Http;

namespace TimeScale.BLL.Interfaces
{
    public interface IUploadDataService : IService
    {
        Task LoadDataFromCSVAsync(IFormFile file, CancellationToken cancellationToken);
    }
}

