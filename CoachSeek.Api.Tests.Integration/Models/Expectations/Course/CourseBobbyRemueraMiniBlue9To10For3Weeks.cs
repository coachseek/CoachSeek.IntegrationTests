using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Course
{
    public class CourseBobbyRemueraMiniRed9To10For3Weeks : ExpectedCourse
    {
        public CourseBobbyRemueraMiniRed9To10For3Weeks(Guid coachId, 
                                                       Guid locationId, 
                                                       Guid serviceId, 
                                                       string startDate,
                                                       bool isOnlineBookable = true)
            : base(coachId, 
                   locationId, 
                   serviceId, 
                   3, 
                   "w", 
                   startDate, 
                   "09:00", 
                   60, 
                   3, 
                   isOnlineBookable, 
                   20, 
                   50, 
                   "red",
                   "Mini Red at Remuera Racquets Club with Bobby Smith on {0} at {1}")
        { }

        public override string Description { get { return string.Format("Mini Red at Remuera Racquets Club with Bobby Smith starting on {0} at {1} for 3 weeks", Timing.startDate, Timing.startTime); } }
    }
}
