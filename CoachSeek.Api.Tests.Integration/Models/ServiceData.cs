using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ServiceData
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public ServiceDefaults defaults { get; set; }

        public ServiceBooking booking { get; set; }
        public ServicePricing pricing { get; set; }
        public ServiceRepetition repetition { get; set; }
    }
}
