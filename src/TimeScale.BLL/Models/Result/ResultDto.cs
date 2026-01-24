namespace TimeScale.BLL.Models.Result
{
    public sealed class ResultDto
    {
        public string FileName { get; set; }
        public double DeltaTime { get; set; }
        public DateTime FirstOperationDate { get; set; }
        public double AvgExecutionTime { get; set; }
        public double AvgValue { get; set; }
        public double MedianValue { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }

        public ResultDto(string fileName,
            double deltaTime,
            DateTime firstOperationDate,
            double avgExecutionTime,
            double avgValue,
            double medianValue,
            double maxValue,
            double minValue)
        {
            FileName = fileName;
            DeltaTime = deltaTime;
            FirstOperationDate = firstOperationDate;
            AvgExecutionTime = avgExecutionTime;
            AvgValue = avgValue;
            MedianValue = medianValue;
            MaxValue = maxValue;
            MinValue = minValue;
        }
    }
}
