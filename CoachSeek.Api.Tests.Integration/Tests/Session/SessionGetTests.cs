using System;
using System.Collections.Generic;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    public class SessionGetTests : ScheduleTests
    {
        [SetUp]
        public void Setup()
        {
            SetupFullTestBusiness();
        }


        [TestFixture]
        public class AnonymousSessionGetTests : SessionGetTests
        {
            //[Test]
            //public void GivenNoBusinessDomain_WhenTryGetById_ThenReturnNotAuthorised()
            //{
            //    var id = GivenInvalidSessionId();
            //    var response = WhenTryGetById(id);
            //    ThenReturnNotFound(response);
            //}

            //[Test]
            //public void GivenInvalidBusinessDomain_WhenTryGetById_ThenReturnNotAuthorised()
            //{
            //    var id = GivenInvalidSessionId();
            //    var response = WhenTryGetById(id);
            //    ThenReturnNotFound(response);
            //}

        
        }

        [TestFixture]
        public class AuthenticatedSessionGetTests : SessionGetTests
        {
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
        }


        private Guid GivenInvalidSessionId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidSessionWithoutBookings()
        {
            return AaronOrakei16To17.Id;
        }

        private Guid GivenValidSessionWithBookings()
        {
            return AaronOrakei14To15.Id;
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
            Assert.That(session.id, Is.EqualTo(AaronOrakei16To17.Id));
            Assert.That(session.parentId, Is.Null);

            AssertSessionLocation(session.location, Orakei.Id, Orakei.Name);
            AssertSessionCoach(session.coach, Aaron.Id, Aaron.Name);
            AssertSessionService(session.service, MiniRedId, "Mini Red");

            AssertSessionTiming(session.timing, GetFormattedDateOneWeekOut(), "16:00", 60);
            AssertSessionBooking(session.booking, 13, false);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 19.95m, null);
            AssertSessionPresentation(session.presentation, "red");
        }

        private void ThenReturnSessionWithBookings(Response response)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            Assert.That(session, Is.Not.Null);
            Assert.That(session.id, Is.EqualTo(AaronOrakei14To15.Id));
            Assert.That(session.parentId, Is.Null);

            AssertSessionLocation(session.location, Orakei.Id, Orakei.Name);
            AssertSessionCoach(session.coach, Aaron.Id, Aaron.Name);
            AssertSessionService(session.service, MiniRedId, "Mini Red");

            AssertSessionTiming(session.timing, GetDateFormatNumberOfWeeksOut(3), "14:00", 60);
            AssertSessionBooking(session.booking, 13, true, 2);
            AssertSessionRepetition(session.repetition, 1, null);
            AssertSessionPricing(session.pricing, 19.95m, null);
            AssertSessionPresentation(session.presentation, "red");

            var bookings = session.booking.bookings;
            var bookingOne = bookings[0];
            Assert.That(bookingOne.id, Is.EqualTo(FredOnAaronOrakei14To15SessionId));
            Assert.That(bookingOne.customer.id, Is.EqualTo(Fred.Id));
            Assert.That(bookingOne.customer.firstName, Is.EqualTo(Fred.FirstName));
            Assert.That(bookingOne.customer.lastName, Is.EqualTo(Fred.LastName));
            Assert.That(bookingOne.customer.email, Is.EqualTo(Fred.Email));
            Assert.That(bookingOne.customer.phone, Is.EqualTo(Fred.Phone.ToUpper()));

            var bookingTwo = bookings[1];
            Assert.That(bookingTwo.id, Is.EqualTo(BarneyOnAaronOrakei14To15SessionId));
            Assert.That(bookingTwo.customer.id, Is.EqualTo(Barney.Id));
            Assert.That(bookingTwo.customer.firstName, Is.EqualTo(Barney.FirstName));
            Assert.That(bookingTwo.customer.lastName, Is.EqualTo(Barney.LastName));
            Assert.That(bookingTwo.customer.email, Is.EqualTo(Barney.Email));
            Assert.That(bookingTwo.customer.phone, Is.EqualTo(Barney.Phone));
        }
    }
}
