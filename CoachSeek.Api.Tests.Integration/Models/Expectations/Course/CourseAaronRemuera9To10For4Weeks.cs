using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Course
{
    public class CourseAaronRemuera9To10For4Weeks : ExpectedCourse
    {
        public CourseAaronRemuera9To10For4Weeks(Guid coachId, Guid locationId, Guid serviceId, string startDate)
            : base(coachId, locationId, serviceId, 4, "w", startDate, "9:00", 60, 2, true, 19.95m, "red")
        { }
    }
}
