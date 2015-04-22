using System.Collections.Generic;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class SessionBookingData
    {
        public int studentCapacity { get; set; }
        public bool isOnlineBookable { get; set; }
        public int bookingCount { get; set; }
        public IList<CustomerBookingData> bookings { get; set; }
    }
}
