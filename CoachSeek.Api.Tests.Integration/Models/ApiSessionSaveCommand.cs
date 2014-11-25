using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiSessionSaveCommand
    {
        public Guid? businessId { get; set; }
        public Guid? id { get; set; }

        public ApiServiceKey service { get; set; }
        public ApiLocationKey location { get; set; }
        public ApiCoachKey coach { get; set; }

        public ApiSessionTiming timing { get; set; }
        public ApiSessionBooking booking { get; set; }
        public ApiPricing pricing { get; set; }
        public ApiRepetition repetition { get; set; }
        public ApiPresentation presentation { get; set; }
    }
}
