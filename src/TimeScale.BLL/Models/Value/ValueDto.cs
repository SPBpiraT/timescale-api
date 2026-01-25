namespace TimeScale.BLL.Models.Value
{
    public sealed class ValueDto
    {
        public string FileName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public double ExecutionTime { get; set; }
        public double Value { get; set; }
    }
}
