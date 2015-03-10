using System.Collections;
using System.Collections.Generic;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class SessionBookingData
    {
        public int? studentCapacity { get; set; }
        public bool isOnlineBookable { get; set; } // eg. Is private or not
        public IList<CustomerBookingData> bookings { get; set; }
    }
}
