using OpenAccess.Glimpse.Model;

namespace OpenAccess.Glimpse.Converters
{
    public class ConnectionMetricsConverter : BaseMetricsConverter
    {
        protected override object GetMetricsValue(OAEventLog eventLog)
        {
            return string.Format("{0}\n{1}", eventLog.Information, eventLog.StackTrace);
        }
    }
}
