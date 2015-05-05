using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Service
{
    public abstract class ExpectedService
    {
        public Guid Id { get; set; }

        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public ApiPresentation Presentation { get; protected set; }
        public ApiServiceRepetition Repetition { get; protected set; }
        public ApiServiceTiming Timing { get; protected set; }
        public ApiPricing Pricing { get; protected set; }
        public ApiServiceBooking Booking { get; protected set; }


        protected ExpectedService(string colour, 
                                  int sessionCount = 1, 
                                  string repeatFrequency = null, 
                                  int? duration = null,
                                  decimal? sessionPrice = null,
                                  decimal? coursePrice = null, 
                                  int? studentCapacity = null, 
                                  bool? isOnlineBookable = null)
        {
            colour = colour.ToLower().Capitalise();

            Name = string.Format("Mini {0}", colour);
            Description = string.Format("Mini {0} Service", colour);
            Presentation = new ApiPresentation { colour = colour };

            Repetition = new ApiServiceRepetition { sessionCount = sessionCount, repeatFrequency = repeatFrequency };

            if (duration != null)
                Timing = new ApiServiceTiming { duration = duration };
            if (sessionPrice != null || coursePrice != null)
                Pricing = new ApiPricing { sessionPrice = sessionPrice, coursePrice = coursePrice };
            if (studentCapacity != null || isOnlineBookable != null)
                Booking = new ApiServiceBooking { studentCapacity = studentCapacity, isOnlineBookable = isOnlineBookable };
        }

        protected ExpectedService(string name, 
                                  string colour,
                                  int sessionCount = 1,
                                  string repeatFrequency = null,
                                  int? duration = null,
                                  decimal? sessionPrice = null,
                                  decimal? coursePrice = null,
                                  int? studentCapacity = null,
                                  bool? isOnlineBookable = null)
        {
            Name = name;
            Description = string.Format("{0} Service", name);
            Presentation = new ApiPresentation { colour = colour.ToLower().Capitalise() };

            Repetition = new ApiServiceRepetition { sessionCount = sessionCount, repeatFrequency = repeatFrequency };

            if (duration != null)
                Timing = new ApiServiceTiming { duration = duration };
            if (sessionPrice != null || coursePrice != null)
                Pricing = new ApiPricing { sessionPrice = sessionPrice, coursePrice = coursePrice };
            if (studentCapacity != null || isOnlineBookable != null)
                Booking = new ApiServiceBooking { studentCapacity = studentCapacity, isOnlineBookable = isOnlineBookable };
        }
    }
}
