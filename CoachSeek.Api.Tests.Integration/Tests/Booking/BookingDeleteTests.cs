using System;
using System.Net;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class BookingDeleteTests : BaseBookingTests
    {
        [Test]
        public void GivenNonExistentBookingId_WhenTryDelete_ThenReturnNotFoundErrorResponse()
        {
            var setup = RegisterBusiness();

            var id = GivenNonExistentBookingId();
            var response = WhenTryDelete(id, setup);
            AssertNotFound(response);
        }

        [Test]
        public void GivenExistingSessionBookingId_WhenTryDelete_ThenSessionBookingIsDeleted()
        {
            var setup = RegisterBusiness();
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            var id = GivenExistingSessionBookingId(setup);
            var response = WhenTryDelete(id, setup);
            ThenSessionBookingIsDeleted(response, setup);
        }

        //[Test]
        //public void GivenExistingCourseBookingId_WhenTryDelete_ThenCourseBookingIsDeleted()
        //{
        //    var setup = RegisterBusiness();
        //    RegisterFredOnAllCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days();

        //    var id = GivenExistingCourseBookingId();
        //    var response = WhenTryDelete(id, setup);
        //    ThenCourseBookingIsDeleted(response);
        //}


        private Guid GivenNonExistentBookingId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenExistingSessionBookingId(SetupData setup)
        {
            return setup.FredOnAaronOrakeiMiniRed14To15.Id;
        }

        //private Guid GivenExistingCourseBookingId()
        //{
        //    // Ensure course booking is there BEFORE the deletion
        //    var getResponse = AuthenticatedGet<CourseBookingData>("Bookings", FredOnAaronOrakeiMiniBlueFor2DaysCourseId);
        //    AssertSuccessResponse<CourseBookingData>(getResponse);

        //    return FredOnAaronOrakeiMiniBlueFor2DaysCourseId;
        //}


        private ApiResponse WhenTryDelete(Guid id, SetupData setup)
        {
            return Delete<BookingData>("Bookings", id, setup);
        }


        private void ThenSessionBookingIsDeleted(ApiResponse response, SetupData setup)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = AuthenticatedGet<BookingData>("Bookings", setup.FredOnAaronOrakeiMiniRed14To15.Id, setup);
            AssertNotFound(getResponse);
        }

        private void ThenCourseBookingIsDeleted(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = AuthenticatedGet<BookingData>("Bookings", FredOnAaronOrakeiMiniBlueFor2DaysCourseId);
            AssertNotFound(getResponse);
        }
    }
}
