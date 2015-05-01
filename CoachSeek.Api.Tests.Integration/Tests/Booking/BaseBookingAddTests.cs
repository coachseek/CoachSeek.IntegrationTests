using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    public abstract class BaseBookingAddTests : BaseBookingTests
    {
        // Session
        protected Response WhenTryBookSession(ApiBookingSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);

            return WhenTryBookSession(json);
        }

        protected Response WhenTryBookSession(string json)
        {
            return Post<SingleSessionBookingData>(json);
        }

        protected Response WhenTryBookOnlineSession(ApiBookingSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);

            return WhenTryBookOnlineSession(json);
        }

        protected Response WhenTryBookOnlineSession(string json)
        {
            return PostForOnlineBooking<SingleSessionBookingData>(json);
        }


        // Course
        protected Response WhenTryBookCourse(ApiBookingSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);

            return WhenTryBookCourse(json);
        }

        protected Response WhenTryBookCourse(string json)
        {
            return Post<CourseBookingData>(json);
        }


        protected Response WhenTryBookOnlineCourse(ApiBookingSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);

            return WhenTryBookOnlineCourse(json);
        }

        protected Response WhenTryBookOnlineCourse(string json)
        {
            return PostForOnlineBooking<CourseBookingData>(json);
        }
    }
}
