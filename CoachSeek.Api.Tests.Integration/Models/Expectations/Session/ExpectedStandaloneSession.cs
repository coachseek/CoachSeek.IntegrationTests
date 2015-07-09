using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Session
{
    public abstract class ExpectedStandaloneSession : ExpectedSingleSession
    {
        protected ExpectedStandaloneSession(Guid coachId, 
                                            Guid locationId, 
                                            Guid serviceId, 
                                            string date, 
                                            string startTime, 
                                            int duration, 
                                            int studentCapacity, 
                                            bool isOnlineBookable, 
                                            decimal price, 
                                            string colour)
            : base(coachId, 
                   locationId, 
                   serviceId, 
                   date, 
                   startTime, 
                   duration, 
                   studentCapacity, 
                   isOnlineBookable, 
                   price, 
                   colour)
        { }
    }
}
