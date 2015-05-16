using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Course
{
    public abstract class ExpectedCourse
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


        protected ExpectedCourse() 
        { }

        protected ExpectedCourse(Guid coachId, Guid locationId, Guid serviceId, int sessionCount, string repeatFrequency, string startDate, string startTime, int duration, int studentCapacity, bool isOnlineBookable, decimal price, string colour)
        {
            Coach = new ApiCoachKey { id = coachId };
            Location = new ApiLocationKey { id = locationId };
            Service = new ApiServiceKey { id = serviceId };
            Timing = new ApiSessionTiming { startDate = startDate, startTime = startTime, duration = duration };
            Booking = new ApiSessionBooking { studentCapacity = studentCapacity, isOnlineBookable = isOnlineBookable };
            Repetition = new ApiRepetition { sessionCount = sessionCount, repeatFrequency = repeatFrequency };
            Pricing = new ApiPricing { sessionPrice = price };
            Presentation = new ApiPresentation { colour = colour };
        }
    }
}
