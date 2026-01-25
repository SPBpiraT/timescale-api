using CsvHelper.Configuration;
using System.Globalization;
using TimeScale.BLL.Models.Value;

namespace TimeScale.BLL.Mapping.CSVHelper
{
    internal sealed class ValueCsvMapping : ClassMap<ValueDto>
    {
        public ValueCsvMapping()
        {
            Map(m => m.Date)
                .Name("Date")
                //.TypeConverterOption.DateTimeStyles(DateTimeStyles.AdjustToUniversal)
                .TypeConverterOption.Format("yyyy-MM-ddTHH-mm-ss.ffffZ");

            Map(m => m.ExecutionTime);

            Map(m => m.Value);
        }
    }
}
