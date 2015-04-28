using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class BookingDeleteTests : ScheduleTests
    {
        [SetUp]
        public void Setup()
        {
            SetupFullTestBusiness();
        }

        protected override string RelativePath
        {
            get { return "Bookings"; }
        }


        [Test]
        public void GivenNonExistentBookingId_WhenTryDelete_ThenReturnNotFoundErrorResponse()
        {
            var id = GivenNonExistentBookingId();
            var response = WhenTryDelete(id);
            AssertNotFound(response);
        }

        [Test]
        public void GivenExistingSessionBookingId_WhenTryDelete_ThenSessionBookingIsDeleted()
        {
            var id = GivenExistingSessionBookingId();
            var response = WhenTryDelete(id);
            ThenSessionBookingIsDeleted(response);
        }

        [Test]
        public void GivenExistingCourseBookingId_WhenTryDelete_ThenCourseBookingIsDeleted()
        {
            var id = GivenExistingCourseBookingId();
            var response = WhenTryDelete(id);
            ThenCourseBookingIsDeleted(response);
        }


        private Guid GivenNonExistentBookingId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenExistingSessionBookingId()
        {
            // Ensure session booking is there BEFORE the deletion
            var getResponse = AuthenticatedGet<SessionBookingData>("Bookings", FredOnAaronOrakei14To15SessionId);
            AssertSuccessResponse<SessionBookingData>(getResponse);

            return FredOnAaronOrakei14To15SessionId;
        }

        private Guid GivenExistingCourseBookingId()
        {
            // Ensure course booking is there BEFORE the deletion
            var getResponse = AuthenticatedGet<CourseBookingData>("Bookings", FredOnAaronOrakeiMiniBlueFor2DaysCourseId);
            AssertSuccessResponse<CourseBookingData>(getResponse);

            return FredOnAaronOrakeiMiniBlueFor2DaysCourseId;
        }


        private Response WhenTryDelete(Guid id)
        {
            return Delete<BookingData>("Bookings", id);
        }


        private void ThenSessionBookingIsDeleted(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = AuthenticatedGet<BookingData>("Bookings", FredOnAaronOrakei14To15SessionId);
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
