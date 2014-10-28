using System;
using System.Collections.Generic;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class BusinessData
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string domain { get; set; }
        public BusinessAdminData admin { get; set; }
        public IList<LocationData> locations { get; set; }
        public IList<CoachData> coaches { get; set; }
    }
}
