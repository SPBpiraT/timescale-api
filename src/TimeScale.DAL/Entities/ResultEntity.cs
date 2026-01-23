using System.ComponentModel.DataAnnotations;

namespace TimeScale.DAL.Entities
{
    public class ResultEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
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
