﻿using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    public abstract class BaseBookingAddTests : BaseBookingTests
    {
        // Session
        protected ApiResponse WhenTryBookSession(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonConvert.SerializeObject(command);
            return WhenTryBookSession(json, setup);
        }

        protected ApiResponse WhenTryBookSessionAnonymously(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonConvert.SerializeObject(command);
            return BusinessAnonymousPost<BookingData>(json, RelativePath, setup);
        }

        protected ApiResponse WhenTryBookSession(string json, SetupData setup)
        {
            return AuthenticatedPost<SingleSessionBookingData>(json, RelativePath, setup);
        }

        protected ApiResponse WhenTryBookCourse(string json, SetupData setup)
        {
            return AuthenticatedPost<CourseBookingData>(json, RelativePath, setup);
        }

        protected ApiResponse WhenTryOnlineBookSession(string json, SetupData setup)
        {
            return BusinessAnonymousPost<SingleSessionBookingData>(json, "OnlineBooking/Bookings", setup);
        }

        protected ApiResponse WhenTryOnlineBookCourse(string json, SetupData setup)
        {
            return BusinessAnonymousPost<CourseBookingData>(json, "OnlineBooking/Bookings", setup);
        }
    }
}
