using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class BookingData
    {
        public Guid id { get; set; }

        public SessionKeyData session { get; set; }
        public CustomerKeyData customer { get; set; }
    }
}
