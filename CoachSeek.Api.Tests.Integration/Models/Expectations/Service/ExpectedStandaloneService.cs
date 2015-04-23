using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Service
{
    public abstract class ExpectedStandaloneService
    {
        public Guid Id { get; set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public ApiPresentation Presentation { get; private set; }
        public ApiServiceRepetition Repetition { get; private set; }
        public ApiServiceTiming Timing { get; private set; }
        public ApiPricing Pricing { get; private set; }
        public ApiServiceBooking Booking { get; private set; }


        protected ExpectedStandaloneService(string colour, int? duration = null, decimal? price = null, int? studentCapacity = null, bool? isOnlineBookable = null)
        {
            colour = colour.ToLower().Capitalise();

            Name = string.Format("Mini {0}", colour);
            Description = string.Format("Mini {0} Service", colour);
            Presentation = new ApiPresentation { colour = colour };
            Repetition = new ApiServiceRepetition { sessionCount = 1 };

            if (duration != null)
                Timing = new ApiServiceTiming { duration = duration };
            if (price != null)
                Pricing = new ApiPricing { sessionPrice = price };
            if (studentCapacity != null || isOnlineBookable != null)
                Booking = new ApiServiceBooking { studentCapacity = studentCapacity, isOnlineBookable = isOnlineBookable };
        }
    }
}
