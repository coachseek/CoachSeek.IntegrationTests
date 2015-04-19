using System;
using System.Collections.Generic;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class SessionSearchData
    {
        public List<SessionData> Sessions { get; set; }
        public List<CourseData> Courses { get; set; }
    }
}
