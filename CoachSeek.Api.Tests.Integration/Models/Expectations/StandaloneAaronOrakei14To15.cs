using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class StandaloneAaronOrakei14To15 : ExpectedStandaloneSession
    {
        public StandaloneAaronOrakei14To15(Guid coachId, Guid locationId, Guid serviceId, string startDate)
            : base(coachId, locationId, serviceId, startDate, "14:00", 60, 13, true, 19.95m, "red")
        { }
    }
}
