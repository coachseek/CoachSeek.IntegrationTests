using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class SingleSessionBookingData
    {
        public Guid id { get; set; }
        public Guid? parentId { get; set; }
        public string paymentStatus { get; set; }
        public bool hasAttended { get; set; }
        public SessionKeyData session { get; set; }
        public CustomerKeyData customer { get; set; }
    }
}
