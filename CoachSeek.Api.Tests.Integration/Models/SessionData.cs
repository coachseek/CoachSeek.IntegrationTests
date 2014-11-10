using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class SessionData
    {
        public Guid? businessId { get; set; }

        public Guid? serviceId { get; set; }
        public Guid? locationId { get; set; }
        public Guid? coachId { get; set; }

        public string startDate { get; set; }
        public string startTime { get; set; }
        public int duration { get; set; }
        public int studentCapacity { get; set; }
        public bool isOnlineBookable { get; set; } // eg. Is private or not
        public string colour { get; set; }
    }
}
