using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiServiceSaveCommand
    {
        public Guid? businessId { get; set; }
        public Guid? id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public ApiServiceTiming timing { get; set; }
        public ApiServiceBooking booking { get; set; }
        public ApiPresentation presentation { get; set; }
        public ApiServiceRepetition repetition { get; set; }
        public ApiPricing pricing { get; set; }
    }
}
