using System;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Course;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Session;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiSessionSaveCommand
    {
        public Guid? id { get; set; }
        public ApiServiceKey service { get; set; }
        public ApiLocationKey location { get; set; }
        public ApiCoachKey coach { get; set; }
        public ApiSessionTiming timing { get; set; }
        public ApiSessionBooking booking { get; set; }
        public ApiPricing pricing { get; set; }
        public ApiRepetition repetition { get; set; }
        public ApiPresentation presentation { get; set; }


        public ApiSessionSaveCommand() { }

        public ApiSessionSaveCommand(ExpectedSingleSession session)
        {
            service = new ApiServiceKey(session.Service.id);
            location = new ApiLocationKey(session.Location.id);
            coach = new ApiCoachKey(session.Coach.id);
            timing = new ApiSessionTiming
            {
                duration = session.Timing.duration,
                startDate = session.Timing.startDate,
                startTime = session.Timing.startTime,
            };
            booking = new ApiSessionBooking
            {
                studentCapacity = session.Booking.studentCapacity,
                isOnlineBookable = session.Booking.isOnlineBookable
            };
            pricing = new ApiPricing
            {
                sessionPrice = session.Pricing.sessionPrice,
                coursePrice = session.Pricing.coursePrice
            };
            presentation = new ApiPresentation(session.Presentation.colour);
            repetition = new ApiRepetition();
        }

        public ApiSessionSaveCommand(ExpectedCourse course)
        {
            service = new ApiServiceKey(course.Service.id);
            location = new ApiLocationKey(course.Location.id);
            coach = new ApiCoachKey(course.Coach.id);
            timing = new ApiSessionTiming
            {
                duration = course.Timing.duration,
                startDate = course.Timing.startDate,
                startTime = course.Timing.startTime,
            };
            booking = new ApiSessionBooking
            {
                studentCapacity = course.Booking.studentCapacity,
                isOnlineBookable = course.Booking.isOnlineBookable
            };
            pricing = new ApiPricing
            {
                sessionPrice = course.Pricing.sessionPrice,
                coursePrice = course.Pricing.coursePrice
            };
            presentation = new ApiPresentation(course.Presentation.colour);
            repetition = new ApiRepetition(course.Repetition.sessionCount, course.Repetition.repeatFrequency);
        }
    }
}
