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
        public void GivenExistingBookingId_WhenTryDelete_ThenBookingIsDeleted()
        {
            var id = GivenExistingBookingId();
            var response = WhenTryDelete(id);
            ThenBookingIsDeleted(response);
        }


        private Guid GivenNonExistentBookingId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenExistingBookingId()
        {
            return FredOnAaronOrakei14To15SessionId;
        }


        private Response WhenTryDelete(Guid id)
        {
            return Delete<BookingData>("Bookings", id);
        }


        private void ThenBookingIsDeleted(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            var getResponse = Get<BookingData>("Bookings", FredOnAaronOrakei14To15SessionId);
            AssertNotFound(getResponse);
        }
    }
}
