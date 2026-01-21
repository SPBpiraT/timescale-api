using System.ComponentModel.DataAnnotations;

namespace TimeScale.DAL.Entities
{
    public class ValueEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double ExecutionTime { get; set; }

        [Required]
        public double Value { get; set; }

    }
}
