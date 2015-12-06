using System;
using System.Collections.Generic;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Session;
using CoachSeek.Common.Extensions;
using NUnit.Framework;

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

        public IList<ExpectedCourseSession> Sessions { get; private set; }

        public abstract string Description { get; }


        protected ExpectedCourse()
        {
            Sessions = new List<ExpectedCourseSession>();
        }

        protected ExpectedCourse(Guid coachId, 
                                 Guid locationId, 
                                 Guid serviceId, 
                                 int sessionCount, 
                                 string repeatFrequency, 
                                 string startDate, 
                                 string startTime, 
                                 int duration, 
                                 int studentCapacity, 
                                 bool isOnlineBookable,
                                 decimal? sessionPrice,
                                 decimal? coursePrice,
                                 string colour,
                                 string sessionName)
        {
            Coach = new ApiCoachKey { id = coachId };
            Location = new ApiLocationKey { id = locationId };
            Service = new ApiServiceKey { id = serviceId };
            Timing = new ApiSessionTiming { startDate = startDate, startTime = startTime, duration = duration };
            Booking = new ApiSessionBooking { studentCapacity = studentCapacity, isOnlineBookable = isOnlineBookable };
            Repetition = new ApiRepetition { sessionCount = sessionCount, repeatFrequency = repeatFrequency };
            Pricing = new ApiPricing { sessionPrice = sessionPrice, coursePrice = coursePrice };
            Presentation = new ApiPresentation { colour = colour };

            Sessions = new List<ExpectedCourseSession>();
            for (var i = 0; i < sessionCount; i++)
            {
                var date = FormatDate(startDate, repeatFrequency, i);
                var session = new ExpectedCourseSession(coachId, 
                                                        locationId, 
                                                        serviceId, 
                                                        date, 
                                                        startTime, 
                                                        duration,
                                                        studentCapacity, 
                                                        isOnlineBookable, 
                                                        sessionPrice, 
                                                        colour,
                                                        sessionName);
                Sessions.Add(session);
            }
        }

        private string FormatDate(string startDate, string repeatFrequency, int sessionIndex)
        {
            var startingDate = startDate.Parse<DateTime>();
            var date = DateTime.Today;
            if (repeatFrequency == "d")
                date = startingDate.AddDays(sessionIndex);
            if (repeatFrequency == "w")
                date = startingDate.AddDays(sessionIndex * 7);
            return date.ToString("yyyy-MM-dd");
        }


        public void Assert(SessionKeyData actualCourse)
        {
            NUnit.Framework.Assert.That(actualCourse.id, Is.EqualTo(Id));
            NUnit.Framework.Assert.That(actualCourse.name, Is.EqualTo(Description));
        }
    }
}
