using System;

namespace OpenAccess.Glimpse.Model
{
    public class OAEventLog
    {
        public int ConnectionId
        {
            get;
            set;
        }

        public int StatementId
        {
            get;
            set;
        }

        public string StackTrace
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public string FetchGroup
        {
            get;
            set;
        }

        public string Query
        {
            get;
            set;
        }

        public DateTime Timestamp
        {
            get;
            set;
        }

        public TimeSpan Duration
        {
            get;
            set;
        }

        public string Information
        {
            get;
            set;
        }

        public int BatchedStatements
        {
            get;
            set;
        }

        public string Params
        {
            get;
            set;
        }

        public MetricsType Type
        {
            get;
            set;
        }

        public TimeSpan Offset
        {
            get;
            set;
        }
    }
}
