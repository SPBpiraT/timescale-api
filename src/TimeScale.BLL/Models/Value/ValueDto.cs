namespace TimeScale.BLL.Models.Value
{
    public record ValueDto
    {
        public string FileName { get; set; }
        public DateTime Date { get; set; }
        public double ExecutionTime { get; set; }
        public double Value { get; set; }
    }
}
