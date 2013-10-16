using OpenAccess.Glimpse.Model;

namespace OpenAccess.Glimpse.Converters
{
    public class CommandMetricsConverter : BaseMetricsConverter
    {
        protected override object GetMetricsValue(OAEventLog eventLog)
        {
            string query = string.IsNullOrEmpty(eventLog.Query)
                ? eventLog.Information
                : eventLog.Query;

            return string.Format("{0}\n{1}\nConnection #{2}\n{3}\n", query, eventLog.Params, eventLog.ConnectionId, eventLog.StackTrace);
        }
    }
}
