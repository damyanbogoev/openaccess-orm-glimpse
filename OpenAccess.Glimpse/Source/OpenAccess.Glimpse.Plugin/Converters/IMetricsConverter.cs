using Glimpse.Core.Tab.Assist;
using OpenAccess.Glimpse.Model;

namespace OpenAccess.Glimpse.Converters
{
    public interface IMetricsConverter
    {
        void Convert(TabSection section, OAEventLog eventLog);
    }
}
