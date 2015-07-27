using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Course
{
    public class CourseAaronOrakeiHolidayCamp9To15For3Days : ExpectedCourse
    {
        public CourseAaronOrakeiHolidayCamp9To15For3Days(Guid coachId, Guid locationId, Guid serviceId, string startDate, int studentCapacity = 3)
            : base(coachId, locationId, serviceId, 3, "d", startDate, "9:00", 360, studentCapacity, true, 50, 120, "yellow")
        { }

        public override string Description { get { return string.Format("Holiday Camp at Orakei Tennis Club with Aaron Smith starting on {0} at 09:00 for 3 days", Timing.startDate, Timing.startTime); } }
    }
}
