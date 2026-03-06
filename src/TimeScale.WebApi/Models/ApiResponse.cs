using System.Text.Json.Serialization;

namespace TimeScale.WebApi.Models
{
    public record ApiResponse(bool success, int statusCode, string? message) { } //TODO: Use StatusCodes instead int
    public record ApiResponse<T>(bool success, int statusCode, string? message, T? data) : ApiResponse(success, statusCode, message) { }
}
