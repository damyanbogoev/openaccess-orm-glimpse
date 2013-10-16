using System;
using Glimpse.Core.Message;

namespace OpenAccess.Glimpse.Model
{
    public class OACommandExecuted : ITimelineMessage
    {
        private readonly Guid id;

        public OACommandExecuted(DateTime startTime, TimeSpan? duration, string eventName, string subEventText)
        {
            this.id = Guid.NewGuid();
            this.EventName = eventName;
            this.StartTime = startTime;
            this.Duration = duration.GetValueOrDefault();
            this.EventCategory = new TimelineCategoryItem("OpenAccess ORM", "#FF0F2B", "#FF0F2B");
            this.EventSubText = subEventText;
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public TimeSpan Offset
        {
            get;
            set;
        }

        public TimeSpan Duration
        {
            get;
            set;
        }

        public DateTime StartTime
        {
            get;
            set;
        }

        public string EventName
        {
            get;
            set;
        }

        public TimelineCategoryItem EventCategory
        {
            get;
            set;
        }

        public string EventSubText
        {
            get;
            set;
        }
    }
}
