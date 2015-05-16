using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Session
{
    public abstract class ExpectedStandaloneSession
    {
        public Guid Id { get; set; }

        public ApiLocationKey Location { get; private set; }
        public ApiCoachKey Coach { get; private set; }
        public ApiServiceKey Service { get; private set; }


        public ApiSessionTiming Timing { get; private set; }
        public ApiSessionBooking Booking { get; private set; }
        public ApiRepetition Repetition { get; private set; }
        public ApiPricing Pricing { get; private set; }
        public ApiPresentation Presentation { get; private set; }

        public string Description { get { return string.Format("Mini Red at Orakei Tennis Club with Aaron Smith on {0} at {1}", Timing.startDate, Timing.startTime); } }


        protected ExpectedStandaloneSession() 
        { }

        protected ExpectedStandaloneSession(Guid coachId, Guid locationId, Guid serviceId, string date, string startTime, int duration, int studentCapacity, bool isOnlineBookable, decimal price, string colour)
        {
            Coach = new ApiCoachKey { id = coachId };
            Location = new ApiLocationKey { id = locationId };
            Service = new ApiServiceKey { id = serviceId };
            Timing = new ApiSessionTiming { startDate = date, startTime = startTime, duration = duration };
            Booking = new ApiSessionBooking { studentCapacity = studentCapacity, isOnlineBookable = isOnlineBookable };
            Repetition = new ApiRepetition { sessionCount = 1 };
            Pricing = new ApiPricing { sessionPrice = price };
            Presentation = new ApiPresentation { colour = colour };
        }
    }
}
