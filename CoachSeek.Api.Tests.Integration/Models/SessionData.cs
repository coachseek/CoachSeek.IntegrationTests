using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class SessionData
    {
        public Guid id { get; set; }

        public LocationKeyData location { get; set; }
        public CoachKeyData coach { get; set; }
        public ServiceKeyData service { get; set; }

        public SessionTimingData timing { get; set; }
        public SessionBookingData booking { get; set; }
        public PricingData pricing { get; set; }
        public RepetitionData repetition { get; set; }
        public PresentationData presentation { get; set; }
    }
}
