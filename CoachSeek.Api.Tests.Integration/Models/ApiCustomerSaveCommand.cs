using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiCustomerSaveCommand
    {
        public Guid? id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string dateOfBirth { get; set; }
    }
}
