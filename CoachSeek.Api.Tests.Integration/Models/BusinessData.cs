using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class BusinessData : BasicBusinessData
    {
        private DateTime _authorisedUntil;

        public string subscriptionPlan { get; set; }

        public DateTime authorisedUntil
        {
            get { return _authorisedUntil; }
            set { _authorisedUntil = new DateTime(value.Ticks, DateTimeKind.Utc); }
        }
        public BusinessStatisticsData statistics { get; set; }
    }
}
