using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Course
{
    public class CourseBobbyRemueraMiniBlue12To13For25Weeks : ExpectedCourse
    {
        public CourseBobbyRemueraMiniBlue12To13For25Weeks(Guid coachId, 
                                                          Guid locationId, 
                                                          Guid serviceId, 
                                                          string startDate,
                                                          bool isOnlineBookable = true)
            : base(coachId, 
                   locationId, 
                   serviceId, 
                   25, 
                   "w", 
                   startDate, 
                   "12:00", 
                   60, 
                   1, 
                   isOnlineBookable, 
                   0, 
                   0, 
                   "blue",
                   "Mini Blue at Remuera Racquets Club with Bobby Smith on {0} at {1}")
        { }

        public override string Description { get { return string.Format("Mini Blue at Remuera Racquets Club with Bobby Smith starting on {0} at {1} for 25 weeks", Timing.startDate, Timing.startTime); } }
    }
}
