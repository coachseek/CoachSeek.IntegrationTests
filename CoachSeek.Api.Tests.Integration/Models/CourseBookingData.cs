using System;
using System.Collections.Generic;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class CourseBookingData
    {
        public Guid id { get; set; }
        public string paymentStatus { get; set; }

        public SessionKeyData course { get; set; }
        public CustomerKeyData customer { get; set; }

        public List<SingleSessionBookingData> sessionBookings { get; set; }
    }
}
