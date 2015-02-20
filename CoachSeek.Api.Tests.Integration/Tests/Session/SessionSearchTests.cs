using System;
using System.Collections.Generic;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class SessionSearchTests : ScheduleTests
    {
        [SetUp]
        public void Setup()
        {
            SetupFullTestBusiness();
        }

        protected override string RelativePath
        {
            get { return "Sessions"; }
        }

        private string BuildSearchUrl(string startDate, string endDate, Guid? coachId, Guid? locationId)
        {
            var baseSearchUrl = string.Format("{0}/{1}?startDate={2}&endDate={3}", BaseUrl, RelativePath, startDate, endDate);

            if (coachId.HasValue)
                baseSearchUrl = string.Format("{0}&coachId={1}", baseSearchUrl, coachId.Value);

            if (locationId.HasValue)
                baseSearchUrl = string.Format("{0}&locationId={1}", baseSearchUrl, locationId.Value);

            return baseSearchUrl;
        }


        [Test]
        public void GivenInvalidSearchPeriod_WhenSearch_ThenReturnInvalidSearchPeriodErrorResponse()
        {
            var criteria = GivenInvalidSearchPeriod();
            var response = WhenSearch(criteria);
            ThenReturnInvalidSearchPeriodErrorResponse(response);
        }

        [Test]
        public void GivenNoSessionInSearchPeriod_WhenSearch_ThenReturnNoSessionResponse()
        {
            var criteria = GivenNoSessionInSearchPeriod();
            var response = WhenSearch(criteria);
            ThenReturnNoSessionResponse(response);
        }

        [Test]
        public void GivenInvalidCoachId_WhenSearch_ThenReturnInvalidCoachIdErrorResponse()
        {
            var criteria = GivenInvalidCoachId();
            var response = WhenSearch(criteria);
            ThenReturnInvalidCoachIdErrorResponse(response);
        }

        [Test]
        public void GivenValidCoachId_WhenSearch_ThenReturnSessionsForCoachResponse()
        {
            var criteria = GivenValidCoachId();
            var response = WhenSearch(criteria);
            ThenReturnSessionsForCoachResponse(response);
        }

        [Test]
        public void GivenInvalidLocationId_WhenSearch_ThenReturnInvalidLocationIdErrorResponse()
        {
            var criteria = GivenInvalidLocationId();
            var response = WhenSearch(criteria);
            ThenReturnInvalidLocationIdErrorResponse(response);
        }

        [Test]
        public void GivenValidLocationId_WhenSearch_ThenReturnSessionsForLocationResponse()
        {
            var criteria = GivenValidLocationId();
            var response = WhenSearch(criteria);
            ThenReturnSessionsForLocationResponse(response);
        }


        private Tuple<string, string, Guid?, Guid?> GivenInvalidSearchPeriod()
        {
            return new Tuple<string, string, Guid?, Guid?>("blah", "2015-02-30", null, null);
        }

        private Tuple<string, string, Guid?, Guid?> GivenNoSessionInSearchPeriod()
        {
            return new Tuple<string, string, Guid?, Guid?>("2015-01-01", "2015-01-02", null, null);
        }

        private Tuple<string, string, Guid?, Guid?> GivenInvalidCoachId()
        {
            return new Tuple<string, string, Guid?, Guid?>("2015-01-01", "2015-01-02", Guid.NewGuid(), null);
        }

        private Tuple<string, string, Guid?, Guid?> GivenValidCoachId()
        {
            return new Tuple<string, string, Guid?, Guid?>(GetFormattedDateToday(), GetFormattedDateThreeWeeksOut(), AaronId, null);
        }

        private Tuple<string, string, Guid?, Guid?> GivenInvalidLocationId()
        {
            return new Tuple<string, string, Guid?, Guid?>("2015-01-01", "2015-01-02", null, Guid.NewGuid());
        }

        private Tuple<string, string, Guid?, Guid?> GivenValidLocationId()
        {
            return new Tuple<string, string, Guid?, Guid?>(GetFormattedDateToday(), GetFormattedDateThreeWeeksOut(), null, OrakeiId);
        }


        private Response WhenSearch(Tuple<string, string, Guid?, Guid?> criteria)
        {
            var url = BuildSearchUrl(criteria.Item1, criteria.Item2, criteria.Item3, criteria.Item4);

            return Get<List<SessionData>>(url);
        }
         

        private void ThenReturnInvalidSearchPeriodErrorResponse(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "The startDate is not a valid date.", "startDate" },
                                                    { "The endDate is not a valid date.", "endDate" } });
        }

        private void ThenReturnNoSessionResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Payload, Is.Not.Null);
            var sessions = (List<SessionData>)response.Payload;
            Assert.That(sessions.Count, Is.EqualTo(0));
        }

        private void ThenReturnInvalidCoachIdErrorResponse(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "Not a valid coachId.", "coachId" } });
        }

        private void ThenReturnSessionsForCoachResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Payload, Is.Not.Null);
            var sessions = (List<SessionData>)response.Payload;
            Assert.That(sessions.Count, Is.EqualTo(5));

            var firstSession = sessions[0];
            Assert.That(firstSession.parentId, Is.EqualTo(AaronRemuera9To10For8WeeksCourseId));
            Assert.That(firstSession.coach.id, Is.EqualTo(AaronId));
            Assert.That(firstSession.timing.startTime, Is.EqualTo("9:00"));

            var secondSession = sessions[1];
            Assert.That(secondSession.id, Is.EqualTo(AaronOrakei16To17SessionId));
            Assert.That(secondSession.parentId, Is.Null);
            Assert.That(secondSession.coach.id, Is.EqualTo(AaronId));
            Assert.That(secondSession.timing.startTime, Is.EqualTo("16:00"));

            var thirdSession = sessions[2];
            Assert.That(thirdSession.parentId, Is.EqualTo(AaronRemuera9To10For8WeeksCourseId));
            Assert.That(thirdSession.coach.id, Is.EqualTo(AaronId));
            Assert.That(thirdSession.timing.startTime, Is.EqualTo("9:00"));

            var fourthSession = sessions[3];
            Assert.That(fourthSession.parentId, Is.EqualTo(AaronRemuera9To10For8WeeksCourseId));
            Assert.That(fourthSession.coach.id, Is.EqualTo(AaronId));
            Assert.That(fourthSession.timing.startTime, Is.EqualTo("9:00"));

            var fifthSession = sessions[4];
            Assert.That(fifthSession.id, Is.EqualTo(AaronOrakei14To15SessionId));
            Assert.That(fifthSession.parentId, Is.Null);
            Assert.That(fifthSession.coach.id, Is.EqualTo(AaronId));
            Assert.That(fifthSession.timing.startTime, Is.EqualTo("14:00"));
        }

        private void ThenReturnInvalidLocationIdErrorResponse(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "Not a valid locationId.", "locationId" } });
        }

        private void ThenReturnSessionsForLocationResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Payload, Is.Not.Null);
            var sessions = (List<SessionData>)response.Payload;
            Assert.That(sessions.Count, Is.EqualTo(2));
            foreach (var session in sessions)
                Assert.That(session.location.id, Is.EqualTo(OrakeiId));
        }
    }
}
