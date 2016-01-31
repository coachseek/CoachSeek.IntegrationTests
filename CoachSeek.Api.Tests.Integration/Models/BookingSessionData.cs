using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class BookingSessionData
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public int studentCapacity { get; set; }
        public int bookingCount { get; set; }
        public string date { get; set; }
        public string startTime { get; set; }
    }
}
