using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Session
{
    public class StandaloneBobbyRemuera12To13 : ExpectedStandaloneSession
    {
        public StandaloneBobbyRemuera12To13(Guid coachId, Guid locationId, Guid serviceId, string startDate)
            : base(coachId, 
                   locationId, 
                   serviceId, 
                   startDate, 
                   "12:00", 
                   45, 
                   2, 
                   true, 
                   35, 
                   "blue",
                   "Mini Blue at Remuera Raquets Club with Bobby Smith on {0} at {1}")
        { }
    }
}
