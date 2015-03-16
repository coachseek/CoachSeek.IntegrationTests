using System;
using System.Collections.Generic;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class SessionGetTests : ScheduleTests
    {
        [SetUp]
        public void Setup()
        {
            SetupFullTestBusiness();
        }


        [Test]
        public void GivenInvalidSessionId_WhenTryGetById_ThenReturnNotFound()
        {
            var id = GivenInvalidSessionId();
            var response = WhenTryGetById(id);
            ThenReturnNotFound(response);
        }

        [Test]
        public void GivenValidSessionWithoutBookings_WhenGetById_ThenReturnSessionWithoutBookings()
        {
            var id = GivenValidSessionWithoutBookings();
            var response = WhenTryGetById(id);
            ThenReturnSessionWithoutBookings(response);
        }

        [Test]
        public void GivenValidSessionWithBookings_WhenGetById_ThenReturnSessionWithBookings()
        {
            var id = GivenValidSessionWithBookings();
            var response = WhenTryGetById(id);
            ThenReturnSessionWithBookings(response);
        }


        private Guid GivenInvalidSessionId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidSessionWithoutBookings()
        {
            return AaronOrakei16To17SessionId;
        }

        private Guid GivenValidSessionWithBookings()
        {
            return AaronOrakei14To15SessionId;
        }


        private Response WhenGetAll()
        {
            var url = BuildGetAllUrl();
            return Get<List<CustomerData>>(url);
        }

        private Response WhenTryGetById(Guid sessionId)
        {
            var url = BuildGetByIdUrl(sessionId);
            return Get<SessionData>(url);
        }


        private void ThenReturnNotFound(Response response)
        {
            AssertNotFound(response);
        }

        private void ThenReturnSessionWithoutBookings(Response response)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.id, Is.EqualTo(AaronOrakei16To17SessionId));
            Assert.That(session.parentId, Is.Null);

            AssertSessionLocation(session.location, OrakeiId, "Orakei Tennis Club");
            AssertSessionCoach(session.coach, AaronId, "Aaron Smith");
            AssertSessionService(session.service, MiniRedId, "Mini Red");

            AssertSessionTiming(session.timing, GetFormattedDateOneWeekOut(), "16:00", 60);
            AssertSessionBooking(session.booking, 13, true);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 19.95m, null);
            AssertSessionPresentation(session.presentation, "red");
        }

        private void ThenReturnSessionWithBookings(Response response)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.id, Is.EqualTo(AaronOrakei14To15SessionId));
            Assert.That(session.parentId, Is.Null);

            AssertSessionLocation(session.location, OrakeiId, "Orakei Tennis Club");
            AssertSessionCoach(session.coach, AaronId, "Aaron Smith");
            AssertSessionService(session.service, MiniRedId, "Mini Red");

            AssertSessionTiming(session.timing, GetDateFormatNumberOfWeeksOut(3), "14:00", 60);
            AssertSessionBooking(session.booking, 13, true, 2);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 19.95m, null);
            AssertSessionPresentation(session.presentation, "red");

            var bookings = session.booking.bookings;
            var bookingOne = bookings[0];
            Assert.That(bookingOne.bookingId, Is.EqualTo(FredOnAaronOrakei14To15SessionId));
            Assert.That(bookingOne.customer.id, Is.EqualTo(FredId));
            Assert.That(bookingOne.customer.firstName, Is.EqualTo(FRED_FIRST_NAME));
            Assert.That(bookingOne.customer.lastName, Is.EqualTo(FLINTSTONE_LAST_NAME));
            Assert.That(bookingOne.customer.email, Is.EqualTo(FredEmail));
            Assert.That(bookingOne.customer.phone, Is.EqualTo(FredPhone.ToUpper()));

            var bookingTwo = bookings[1];
            Assert.That(bookingTwo.bookingId, Is.EqualTo(BarneyOnAaronOrakei14To15SessionId));
            Assert.That(bookingTwo.customer.id, Is.EqualTo(BarneyId));
            Assert.That(bookingTwo.customer.firstName, Is.EqualTo(BARNEY_FIRST_NAME));
            Assert.That(bookingTwo.customer.lastName, Is.EqualTo(RUBBLE_LAST_NAME));
            Assert.That(bookingTwo.customer.email, Is.Null);
            Assert.That(bookingTwo.customer.phone, Is.Null);
        }
    }
}
