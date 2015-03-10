using System;
using System.Collections.Generic;
using System.Net;
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
        public void GivenInvalidSessionId_WhenGetById_ThenReturnNotFoundResponse()
        {
            var id = GivenInvalidSessionId();
            var response = WhenGetById(id);
            ThenReturnNotFoundResponse(response);
        }

        [Test]
        public void GivenValidSessionId_WhenGetById_ThenReturnSessionResponse()
        {
            var id = GivenValidSessionId();
            var response = WhenGetById(id);
            ThenReturnSessionResponse(response);
        }


        private Guid GivenInvalidSessionId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidSessionId()
        {
            return AaronOrakei16To17SessionId;
        }


        private Response WhenGetAll()
        {
            var url = BuildGetAllUrl();
            return Get<List<CustomerData>>(url);
        }

        private Response WhenGetById(Guid sessionId)
        {
            var url = BuildGetByIdUrl(sessionId);
            return Get<SessionData>(url);
        }


        private void ThenReturnNotFoundResponse(Response response)
        {
            AssertNotFound(response);
        }

        private void ThenReturnSessionResponse(Response response)
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
    }
}
