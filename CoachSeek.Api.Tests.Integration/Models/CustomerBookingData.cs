using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class CustomerBookingData
    {
        public Guid bookingId { get; set; }
        public CustomerData customer { get; set; }
    }
}
