namespace TimeScale.BLL.Models.Result
{
    public record ResultDto
    {
        public string FileName { get; set; }
        public double DeltaTime { get; set; }
        public DateTime FirstOperationDate { get; set; }
        public double AvgExecutionTime { get; set; }
        public double AvgValue { get; set; }
        public double MedianValue { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
    }
}
