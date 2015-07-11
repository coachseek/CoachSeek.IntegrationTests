using System;
using NUnit.Framework;

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


        public void Assert(SessionData actualSession)
        {
            NUnit.Framework.Assert.That(actualSession.id, Is.EqualTo(Id));
            NUnit.Framework.Assert.That(actualSession.parentId, Is.Null);
            NUnit.Framework.Assert.That(actualSession.location.id, Is.EqualTo(Location.id));

            NUnit.Framework.Assert.That(actualSession.timing.duration, Is.EqualTo(Timing.duration));
            NUnit.Framework.Assert.That(actualSession.timing.startDate, Is.EqualTo(Timing.startDate));
            NUnit.Framework.Assert.That(actualSession.timing.startTime, Is.EqualTo(Timing.startTime));

            NUnit.Framework.Assert.That(actualSession.booking.studentCapacity, Is.EqualTo(Booking.studentCapacity));
            NUnit.Framework.Assert.That(actualSession.booking.isOnlineBookable, Is.EqualTo(Booking.isOnlineBookable));

            NUnit.Framework.Assert.That(actualSession.repetition.sessionCount, Is.EqualTo(1));
            NUnit.Framework.Assert.That(actualSession.repetition.repeatFrequency, Is.Null);

            NUnit.Framework.Assert.That(actualSession.pricing.sessionPrice, Is.EqualTo(Pricing.sessionPrice));
            NUnit.Framework.Assert.That(actualSession.pricing.coursePrice, Is.Null);

            NUnit.Framework.Assert.That(actualSession.presentation.colour, Is.EqualTo(Presentation.colour.ToLowerInvariant()));
        }
    }
}
