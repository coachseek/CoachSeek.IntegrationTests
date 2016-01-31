using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class CustomFieldTemplateData
    {
        public Guid id { get; set; }
        public string type { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public bool isRequired { get; set; }
        public bool isActive { get; set; }
    }
}
