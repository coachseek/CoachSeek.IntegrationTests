using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class CustomerData
    {
        public Guid id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string dateOfBirth { get; set; }
    }
}
