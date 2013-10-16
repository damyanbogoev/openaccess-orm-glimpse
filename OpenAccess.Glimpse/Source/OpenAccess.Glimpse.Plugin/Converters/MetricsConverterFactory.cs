using System;
using OpenAccess.Glimpse.Model;

namespace OpenAccess.Glimpse.Converters
{
    public static class MetricsConverterFactory
    {
        public static IMetricsConverter GetConverter(MetricsType type)
        {
            switch (type)
            {
                case MetricsType.SecondLevelCache:
                    return new SecondLevelCacheMetricsConverter();
                case MetricsType.Transaction:
                    return new TransactionMetricsConverter();
                case MetricsType.Connection:
                    return new ConnectionMetricsConverter();
                case MetricsType.Command:
                    return new CommandMetricsConverter();
                default:
                    throw new NotSupportedException(type.ToString());
            }
        }
    }
}
