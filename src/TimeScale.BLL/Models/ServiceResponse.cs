namespace TimeScale.BLL.Models
{
    public record ServiceResponse (bool Success, int StatusCode, string? Message){}

    public record ServiceResponse<T>(bool Success, int StatusCode, string? Message, T? Data) : ServiceResponse(Success, StatusCode, Message){}
}
