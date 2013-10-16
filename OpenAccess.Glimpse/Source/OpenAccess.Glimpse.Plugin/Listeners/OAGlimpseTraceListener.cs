using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using AutoMapper;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;
using OpenAccess.Glimpse.Model;

namespace OpenAccess.Glimpse.Listeners
{
    public class OAGlimpseTraceListener : TraceListener
    {
        private const string ContextKey = "OAGlimpseTraceListener.Key";

        private readonly static IDictionary<string, MetricsView> metrics;
        private IMessageBroker messageBroker;
        private IExecutionTimer timerStrategy;

        static OAGlimpseTraceListener()
        {
            OAGlimpseTraceListener.metrics = new Dictionary<string, MetricsView>();
            OAGlimpseTraceListener.InitializeMetrics();
        }

        private static void InitializeMetrics()
        {
            // Second Level Cache Metrics
            OAGlimpseTraceListener.metrics[Constants.EvictAll] = OAGlimpseTraceListener.GetMetricsView(Constants.EvictAllCaption, MetricsType.SecondLevelCache);
            OAGlimpseTraceListener.metrics[Constants.EvictObjects] = OAGlimpseTraceListener.GetMetricsView(Constants.EvictObjectsCaption, MetricsType.SecondLevelCache);
            OAGlimpseTraceListener.metrics[Constants.EvictClasses] = OAGlimpseTraceListener.GetMetricsView(Constants.EvictClassesCaption, MetricsType.SecondLevelCache);

            // Transaction Metrics
            OAGlimpseTraceListener.metrics[Constants.TransactionBegin] = OAGlimpseTraceListener.GetMetricsView(Constants.TransactionBeginCaption, MetricsType.Transaction);
            OAGlimpseTraceListener.metrics[Constants.TransationCommit] = OAGlimpseTraceListener.GetMetricsView(Constants.TransactionCommitCaption, MetricsType.Transaction);
            OAGlimpseTraceListener.metrics[Constants.TransactionRollback] = OAGlimpseTraceListener.GetMetricsView(Constants.TransactionRollbackCaption, MetricsType.Transaction);
            OAGlimpseTraceListener.metrics[Constants.TransactionEnlist] = OAGlimpseTraceListener.GetMetricsView(Constants.TransactionEnlistCaption, MetricsType.Transaction);
            OAGlimpseTraceListener.metrics[Constants.TransactionDelist] = OAGlimpseTraceListener.GetMetricsView(Constants.TransactionDelistCaption, MetricsType.Transaction);

            // Connection Metrics
            OAGlimpseTraceListener.metrics[Constants.ConnectionOpen] = OAGlimpseTraceListener.GetMetricsView(Constants.ConnectionOpenCaption, MetricsType.Connection);
            OAGlimpseTraceListener.metrics[Constants.ConnectionClose] = OAGlimpseTraceListener.GetMetricsView(Constants.ConnectionCloseCaption, MetricsType.Connection);
            OAGlimpseTraceListener.metrics[Constants.ConnectionTransactionCommit] = OAGlimpseTraceListener.GetMetricsView(Constants.TransactionCommitCaption, MetricsType.Connection);
            OAGlimpseTraceListener.metrics[Constants.ConnectionTransactionRollback] = OAGlimpseTraceListener.GetMetricsView(Constants.TransactionRollbackCaption, MetricsType.Connection);

            // Command Metrics
            OAGlimpseTraceListener.metrics[Constants.CommandQuery] = OAGlimpseTraceListener.GetMetricsView(Constants.CommandQueryCaption, MetricsType.Command);
            OAGlimpseTraceListener.metrics[Constants.CommandUpdate] = OAGlimpseTraceListener.GetMetricsView(Constants.CommandUpdateCaption, MetricsType.Command);
            OAGlimpseTraceListener.metrics[Constants.CommandExecuteScalar] = OAGlimpseTraceListener.GetMetricsView(Constants.CommandExecuteScalarCaption, MetricsType.Command);
            OAGlimpseTraceListener.metrics[Constants.CommandExecuteNonQuery] = OAGlimpseTraceListener.GetMetricsView(Constants.CommandExecuteNonQueryCaption, MetricsType.Command);
            OAGlimpseTraceListener.metrics[Constants.CloseReader] = OAGlimpseTraceListener.GetMetricsView(Constants.CloseReader, MetricsType.Command);
            OAGlimpseTraceListener.metrics[Constants.AdoCommandBatchExecute] = OAGlimpseTraceListener.GetMetricsView(Constants.AdoCommandBatchExecuteCaption, MetricsType.Command);
        }

        private static MetricsView GetMetricsView(string name, MetricsType type)
        {
            return new MetricsView { Name = name, Type = type };
        }

        public OAGlimpseTraceListener()
            : base(Constants.ListenerName)
        {
        }

        internal IMessageBroker MessageBroker
        {
            get
            {
#pragma warning disable 0618
                return this.messageBroker ?? (this.messageBroker = GlimpseConfiguration.GetConfiguredMessageBroker());
#pragma warning restore 0618

            }
        }

        internal IExecutionTimer TimerStrategy
        {
            get { return this.timerStrategy ?? (this.timerStrategy = GlimpseConfiguration.GetConfiguredTimerStrategy()()); }
        }

        public static IDictionary<string, MetricsView> Metrics
        {
            get
            {
                return metrics;
            }
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (data == null || HttpContext.Current == null)
            {
                return;
            }

            OAEventLog eventLog = Mapper.DynamicMap<OAEventLog>(data);
            if (string.IsNullOrEmpty(eventLog.Name) || OAGlimpseTraceListener.metrics.ContainsKey(eventLog.Name) == false)
            {
                return;
            }

            if (string.Equals(eventLog.Name, Constants.CommandQuery) || string.Equals(eventLog.Name, Constants.CommandUpdate))
            {
                eventLog.Params = OAGlimpseTraceListener.GetParameterInformation(data);
                eventLog.Offset = this.TimerStrategy.Start();
            }
            else if (string.Equals(eventLog.Name, Constants.CloseReader))
            {
                OAEventLog command = OAGlimpseTraceListener.GetList().FirstOrDefault(x => x.StatementId == eventLog.StatementId);
                if (command != null)
                {
                    command.Duration = TimeSpan.FromTicks(eventLog.Timestamp.Ticks - command.Timestamp.Ticks);
                    var toPublish = new OACommandExecuted(eventLog.Timestamp, command.Duration, Constants.SqlQueryExecuted, command.DisplayName)
                        .AsTimedMessage(this.TimerStrategy.Stop(command.Offset));
                    toPublish.Duration = command.Duration;
                    this.MessageBroker.Publish(toPublish);
                }

                return;
            }

            eventLog.DisplayName = OAGlimpseTraceListener.Metrics[eventLog.Name].Name;
            eventLog.Type = OAGlimpseTraceListener.Metrics[eventLog.Name].Type;
            OAGlimpseTraceListener.GetList().Add(eventLog);
        }

        private static string GetParameterInformation(object data)
        {
            PropertyInfo propertyInfo = data.GetType().GetProperty("Params");
            if (propertyInfo == null)
            {
                return null;
            }

            MethodInfo methodInfo = propertyInfo.GetGetMethod();
            if (methodInfo == null)
            {
                return null;
            }

            object[] result = methodInfo.Invoke(data, null) as object[];
            if (result == null || result.Length != 1)
            {
                return null;
            }

            return result[0].ToString();
        }

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }

        public static IList<OAEventLog> GetList()
        {
            if (HttpContext.Current.Items.Contains(OAGlimpseTraceListener.ContextKey) == false)
            {
                IList<OAEventLog> result = new List<OAEventLog>();
                HttpContext.Current.Items[OAGlimpseTraceListener.ContextKey] = result;

                return result;
            }

            return (IList<OAEventLog>)HttpContext.Current.Items[OAGlimpseTraceListener.ContextKey];
        }
    }
}
