using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Course
{
    public class CourseAaronOrakeiHolidayCamp9To15For3Days : ExpectedCourse
    {
        public CourseAaronOrakeiHolidayCamp9To15For3Days(Guid coachId, Guid locationId, Guid serviceId, string startDate)
            : base(coachId, locationId, serviceId, 3, "d", startDate, "9:00", 360, 3, true, 50, 120, "yellow")
        { }
    }
}
