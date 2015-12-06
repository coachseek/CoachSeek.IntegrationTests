using System;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Session
{
    public abstract class ExpectedSingleSession
    {
        private string Name { get; set; }

        public Guid Id { get; set; }

        public ApiLocationKey Location { get; private set; }
        public ApiCoachKey Coach { get; private set; }
        public ApiServiceKey Service { get; private set; }

        public ApiSessionTiming Timing { get; private set; }
        public ApiSessionBooking Booking { get; private set; }
        public ApiRepetition Repetition { get; private set; }
        public ApiPricing Pricing { get; private set; }
        public ApiPresentation Presentation { get; private set; }

        public string Description { get { return string.Format(Name, Timing.startDate, Timing.startTime); } }


        protected ExpectedSingleSession() 
        { }

        protected ExpectedSingleSession(Guid coachId, 
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
        {
            Coach = new ApiCoachKey { id = coachId };
            Location = new ApiLocationKey { id = locationId };
            Service = new ApiServiceKey { id = serviceId };
            Timing = new ApiSessionTiming { startDate = date, startTime = startTime, duration = duration };
            Booking = new ApiSessionBooking { studentCapacity = studentCapacity, isOnlineBookable = isOnlineBookable };
            Repetition = new ApiRepetition { sessionCount = 1 };
            Pricing = new ApiPricing { sessionPrice = price };
            Presentation = new ApiPresentation { colour = colour };
            Name = name;
        }

        
        public void Assert(SessionKeyData actualSession)
        {
            NUnit.Framework.Assert.That(actualSession.id, Is.EqualTo(Id));
            NUnit.Framework.Assert.That(actualSession.name, Is.EqualTo(Description));
        }
    }
}
