namespace TimeScale.BLL.Helpers
{
    internal static class StatisticsHelper
    {
        internal static double ComputeMedianValue(List<double> values)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));

            if (values.Count == 0)
                throw new ArgumentException("List cannot be empty.", nameof(values));

            values.Sort();
            int middleIndex = values.Count / 2;

            if (values.Count % 2 == 0)
            {
                return (values[middleIndex - 1] + values[middleIndex]) / 2.0;
            }

            return values[middleIndex];
        }
    }
}
