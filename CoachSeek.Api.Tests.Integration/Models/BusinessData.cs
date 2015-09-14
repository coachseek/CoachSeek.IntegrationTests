using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class BusinessData
    {
        private DateTime _authorisedUntil;

        public Guid id { get; set; }
        public string name { get; set; }
        public string domain { get; set; }
        public string sport { get; set; }

        public DateTime authorisedUntil
        {
            get { return _authorisedUntil; }
            set { _authorisedUntil = new DateTime(value.Ticks, DateTimeKind.Utc); }
        }
        public BusinessPaymentData payment { get; set; }
    }
}
