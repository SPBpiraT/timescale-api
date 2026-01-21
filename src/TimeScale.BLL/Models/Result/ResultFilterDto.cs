
namespace TimeScale.BLL.Models
{
    public record ResultFilterDto
    {
        public string? FileName { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public double? MaxAvgValue { get; init; }
        public double? MinAvgValue { get; init; }
        public double? MinAvgExecutionTime { get; init; }
        public double? MaxAvgExecutionTime { get; init; }
    }
}
