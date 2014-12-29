using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class UserData
    {
        public Guid id { get; set; }

        public Guid? businessId { get; set; }
        public string businessName { get; set; } // Debug

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }

        public string username { get; set; }
        public string passwordHash { get; set; }
    }
}
