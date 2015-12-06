using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Session
{
    public class ExpectedCourseSession : ExpectedSingleSession
    {
        public Guid ParentId { get; set; }


        public ExpectedCourseSession(Guid coachId,
                                        Guid locationId,
                                        Guid serviceId,
                                        string date,
                                        string startTime,
                                        int duration,
                                        int studentCapacity,
                                        bool isOnlineBookable,
                                        decimal? price,
                                        string colour,
                                        string name)
            : base(coachId,
                locationId,
                serviceId,
                date,
                startTime,
                duration,
                studentCapacity,
                isOnlineBookable,
                price,
                colour,
                name)
        { }
    }
}
