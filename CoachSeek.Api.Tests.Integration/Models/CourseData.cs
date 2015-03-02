using System.Collections.Generic;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class CourseData : SessionData
    {
        public List<SessionData> sessions { get; set; }
    }
}
