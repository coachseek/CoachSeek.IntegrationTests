using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class StandaloneAaronOrakei16To17 : ExpectedStandaloneSession
    {
        public StandaloneAaronOrakei16To17(Guid coachId, Guid locationId, Guid serviceId, string startDate)
            : base(coachId, locationId, serviceId, startDate, "16:00", 60, 13, false, 19.95m, "red")
        { }
    }
}
