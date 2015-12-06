using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Session
{
    public class StandaloneAaronOrakeiMiniRed16To17 : ExpectedStandaloneSession
    {
        public StandaloneAaronOrakeiMiniRed16To17(Guid coachId, Guid locationId, Guid serviceId, string startDate)
            : base(coachId, 
                   locationId, 
                   serviceId, 
                   startDate, 
                   "16:00", 
                   60, 
                   13, 
                   false, 
                   19.95m, 
                   "red",
                   "Mini Red at Orakei Tennis Club with Aaron Smith on {0} at {1}")
        { }
    }
}
