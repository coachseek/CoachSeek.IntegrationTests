using System;
using System.Collections.Generic;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    public class SessionGetTests : ScheduleTests
    {
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
            public void GivenInvalidSessionId_WhenTryGetSessionById_ThenReturnNotFound()
            {
                var setup = RegisterBusiness();
                RegisterStandaloneAaronOrakeiMiniRed14To15(setup);

                var id = GivenInvalidSessionId();
                var response = WhenTryGetSessionById(id, setup);
                ThenReturnNotFound(response);
            }

            [Test]
            public void GivenValidSessionWithoutBookings_WhenTryGetSessionById_ThenReturnSessionWithoutBookings()
            {
                var setup = RegisterBusiness();

                var id = GivenValidSessionWithoutBookings(setup);
                var response = WhenTryGetSessionById(id, setup);
                ThenReturnSessionWithoutBookings(response, setup);
            }

            [Test]
            public void GivenValidSessionWithBookings_WhenGetById_ThenReturnSessionWithBookings()
            {
                var setup = RegisterBusiness();

                var id = GivenValidSessionWithBookings(setup);
                var response = WhenTryGetSessionById(id, setup);
                ThenReturnSessionWithBookings(response, setup);
            }
        }


        private Guid GivenInvalidSessionId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidSessionWithoutBookings(SetupData setup)
        {
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);

            return setup.AaronOrakeiMiniRed14To15.Id;
        }

        private Guid GivenValidSessionWithBookings(SetupData setup)
        {
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            return setup.AaronOrakeiMiniRed14To15.Id;
        }


        private ApiResponse WhenTryGetSessionById(Guid sessionId, SetupData setup)
        {
            return AuthenticatedGet<SessionData>(RelativePath, sessionId, setup);
        }


        private void ThenReturnSessionWithoutBookings(ApiResponse response, SetupData setup)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            setup.AaronOrakeiMiniRed14To15.Assert(session);
            Assert.That(session.booking.bookingCount, Is.EqualTo(0));
            Assert.That(session.booking.bookings.Count, Is.EqualTo(0));
        }

        private void ThenReturnSessionWithBookings(ApiResponse response, SetupData setup)
        {
            var session = AssertSuccessResponse<SessionData>(response);

            setup.AaronOrakeiMiniRed14To15.Assert(session);
            Assert.That(session.booking.bookingCount, Is.EqualTo(1));
            Assert.That(session.booking.bookings.Count, Is.EqualTo(1));
            setup.FredOnAaronOrakeiMiniRed14To15.Assert(session.booking.bookings[0], session.id);
            setup.Fred.Assert(session.booking.bookings[0].customer);
        }
    }
}
