namespace TimeScale.BLL.Models.Value
{
    public sealed class ValueDto
    {
        public string FileName { get; set; } = string.Empty;
        public DateTime Date { get; }
        public double ExecutionTime { get; }
        public double Value { get; }

        public ValueDto(string fileName,
            DateTime date,
            double executionTime,
            double value)
        {
            FileName = fileName;
            Date = date;
            ExecutionTime = executionTime;
            Value = value;
        }
    }
}
