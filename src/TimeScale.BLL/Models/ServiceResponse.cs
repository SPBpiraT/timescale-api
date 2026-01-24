namespace TimeScale.BLL.Models
{
    public record ServiceResponse (bool Success, int StatusCode, string? Message){} //TODO: Use StatusCodes instead int
    public record ServiceResponse<T>(bool Success, int StatusCode, string? Message, T? Data) : ServiceResponse(Success, StatusCode, Message){}
}
