using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    public abstract class BaseBookingAddTests : BaseBookingTests
    {
        // Session
        protected ApiResponse WhenTryBookSession(ApiBookingSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);
            return WhenTryBookSession(json);
        }

        protected ApiResponse WhenTryBookSession(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonConvert.SerializeObject(command);
            return WhenTryBookSession(json, setup);
        }


        protected ApiResponse WhenTryBookSession(string json)
        {
            return new TestAuthenticatedApiClient().Post<SingleSessionBookingData>(json, 
                                                                                   Business.UserName, 
                                                                                   Business.Password, 
                                                                                   RelativePath);
        }


        protected ApiResponse WhenTryBookSession(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<SingleSessionBookingData>(json, 
                                                                                   setup.Business.UserName,
                                                                                   setup.Business.Password, 
                                                                                   RelativePath);
        }

        protected ApiResponse WhenTryOnlineBookSession(string json, SetupData setup)
        {
            return new TestBusinessAnonymousApiClient().Post<SingleSessionBookingData>(json, 
                                                                                       setup.Business.Domain,
                                                                                       "OnlineBooking/Bookings");
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


        protected ApiResponse WhenTryBookCourse(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<CourseBookingData>(json,
                                                                            setup.Business.UserName,
                                                                            setup.Business.Password,
                                                                            RelativePath);
        }

        protected ApiResponse WhenTryOnlineBookCourse(string json, SetupData setup)
        {
            return new TestBusinessAnonymousApiClient().Post<CourseBookingData>(json,
                                                                                setup.Business.Domain,
                                                                                "OnlineBooking/Bookings");
        }
    }
}
