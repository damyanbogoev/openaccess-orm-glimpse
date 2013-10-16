using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;
using OpenAccess.Glimpse.Converters;
using OpenAccess.Glimpse.Listeners;
using OpenAccess.Glimpse.Model;

namespace OpenAccess.Glimpse
{
    public class OAGlimpseTab : TabBase, IDocumentation
    {
        public string DocumentationUri
        {
            get
            {
                return Constants.DocumentationUri;
            }
        }

        public override object GetData(ITabContext context)
        {
            TabSection result = Plugin.Create(Constants.MetricsKey, Constants.MetricsValue, Constants.MetricsDuration, Constants.MetricsTimestamp);
            foreach (OAEventLog eventLog in OAGlimpseTraceListener.GetList())
            {
                MetricsConverterFactory
                    .GetConverter(eventLog.Type)
                    .Convert(result, eventLog);
            }

            return result;
        }

        public override string Name
        {
            get
            {
                return Constants.ProductName;
            }
        }
    }
}
