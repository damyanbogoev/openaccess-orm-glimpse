using Glimpse.Core.Tab.Assist;
using OpenAccess.Glimpse.Model;

namespace OpenAccess.Glimpse.Converters
{
    public class BaseMetricsConverter : IMetricsConverter
    {
        public void Convert(TabSection section, OAEventLog eventLog)
        {
            section.AddRow()
                .Column(eventLog.DisplayName)
                .Column(this.GetMetricsValue(eventLog))
                   .Column(BaseMetricsConverter.GetDurationMetricValue(eventLog))
                   .Column(eventLog.Timestamp.ToString("HH:mm:ss.fff"));
        }

        private static string GetDurationMetricValue(OAEventLog eventLog)
        {
            int milliseconds = eventLog.Duration.Milliseconds;

            return milliseconds == 0 ? null : string.Format("{0}ms", milliseconds);
        }

        protected virtual object GetMetricsValue(OAEventLog eventLog)
        {
            return null;
        }
    }
}
