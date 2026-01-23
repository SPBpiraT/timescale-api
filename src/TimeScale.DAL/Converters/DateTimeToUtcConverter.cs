using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TimeScale.DAL.Converters
{
    internal class DateTimeToUtcConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeToUtcConverter() : base( 
            v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
        {}
    }
}
