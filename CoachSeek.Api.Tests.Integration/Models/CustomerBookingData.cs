using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class CustomerBookingData
    {
        // Note: sessionId is not a property because instances of this class are held within a Session (or Course).
        public Guid id { get; set; }
        public Guid? parentId { get; set; }
        public CustomerData customer { get; set; }
        public bool? hasAttended { get; set; }
        public string paymentStatus { get; set; }
        public bool isOnlineBooking { get; set; }
    }
}
