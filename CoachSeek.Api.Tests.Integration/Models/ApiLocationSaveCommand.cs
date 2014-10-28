using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiLocationSaveCommand
    {
        public Guid? businessId { get; set; }
        public Guid? id { get; set; }
        public string name { get; set; }
    }
}
