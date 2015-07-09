using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Course
{
    public class CourseBobbyRemueraMiniRed9To10For3Weeks : ExpectedCourse
    {
        public CourseBobbyRemueraMiniRed9To10For3Weeks(Guid coachId, 
                                                       Guid locationId, 
                                                       Guid serviceId, 
                                                       string startDate)
            : base(coachId, locationId, serviceId, 3, "w", startDate, "9:00", 60, 3, true, 20, 50, "red")
        { }
    }
}
