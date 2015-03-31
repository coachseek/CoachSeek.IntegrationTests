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

        private string BuildSearchUrl(string startDate, string endDate, Guid? coachId, Guid? locationId, Guid? serviceId)
        {
            var baseSearchUrl = string.Format("{0}/{1}?startDate={2}&endDate={3}", BaseUrl, RelativePath, startDate, endDate);

            if (coachId.HasValue)
                baseSearchUrl = string.Format("{0}&coachId={1}", baseSearchUrl, coachId.Value);

            if (locationId.HasValue)
                baseSearchUrl = string.Format("{0}&locationId={1}", baseSearchUrl, locationId.Value);

            if (serviceId.HasValue)
                baseSearchUrl = string.Format("{0}&serviceId={1}", baseSearchUrl, serviceId.Value);

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

        [Test]
        public void GivenInvalidServiceId_WhenSearch_ThenReturnInvalidServiceIdErrorResponse()
        {
            var criteria = GivenInvalidServiceId();
            var response = WhenSearch(criteria);
            ThenReturnInvalidServiceIdErrorResponse(response);
        }

        [Test]
        public void GivenValidServiceId_WhenSearch_ThenReturnSessionsForSessionResponse()
        {
            var criteria = GivenValidServiceId();
            var response = WhenSearch(criteria);
            ThenReturnSessionsForServiceResponse(response);
        }


        private Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidSearchPeriod()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("blah", "2015-02-30", null, null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenNoSessionInSearchPeriod()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", null, null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidCoachId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", Guid.NewGuid(), null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenValidCoachId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(), GetFormattedDateThreeWeeksOut(), AaronId, null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidLocationId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", null, Guid.NewGuid(), null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenValidLocationId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(), GetFormattedDateThreeWeeksOut(), null, OrakeiId, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidServiceId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", null, null, Guid.NewGuid());
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenValidServiceId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(), GetFormattedDateThreeWeeksOut(), null, null, MiniRedId);
        }


        private Response WhenSearch(Tuple<string, string, Guid?, Guid?, Guid?> criteria)
        {
            var url = BuildSearchUrl(criteria.Item1, criteria.Item2, criteria.Item3, criteria.Item4, criteria.Item5);

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

        private void ThenReturnInvalidLocationIdErrorResponse(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "Not a valid locationId.", "locationId" } });
        }

        private void ThenReturnInvalidServiceIdErrorResponse(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "Not a valid serviceId.", "serviceId" } });
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
            Assert.That(firstSession.coach.name, Is.EqualTo("Aaron Smith"));
            Assert.That(firstSession.timing.startTime, Is.EqualTo("9:00"));

            var secondSession = sessions[1];
            Assert.That(secondSession.id, Is.EqualTo(AaronOrakei16To17SessionId));
            Assert.That(secondSession.parentId, Is.Null);
            Assert.That(secondSession.coach.id, Is.EqualTo(AaronId));
            Assert.That(secondSession.coach.name, Is.EqualTo("Aaron Smith"));
            Assert.That(secondSession.timing.startTime, Is.EqualTo("16:00"));

            var thirdSession = sessions[2];
            Assert.That(thirdSession.parentId, Is.EqualTo(AaronRemuera9To10For8WeeksCourseId));
            Assert.That(thirdSession.coach.id, Is.EqualTo(AaronId));
            Assert.That(thirdSession.coach.name, Is.EqualTo("Aaron Smith"));
            Assert.That(thirdSession.timing.startTime, Is.EqualTo("9:00"));

            var fourthSession = sessions[3];
            Assert.That(fourthSession.parentId, Is.EqualTo(AaronRemuera9To10For8WeeksCourseId));
            Assert.That(fourthSession.coach.id, Is.EqualTo(AaronId));
            Assert.That(fourthSession.coach.name, Is.EqualTo("Aaron Smith"));
            Assert.That(fourthSession.timing.startTime, Is.EqualTo("9:00"));

            var fifthSession = sessions[4];
            Assert.That(fifthSession.id, Is.EqualTo(AaronOrakei14To15SessionId));
            Assert.That(fifthSession.parentId, Is.Null);
            Assert.That(fifthSession.coach.id, Is.EqualTo(AaronId));
            Assert.That(fifthSession.coach.name, Is.EqualTo("Aaron Smith"));
            Assert.That(fifthSession.timing.startTime, Is.EqualTo("14:00"));
            Assert.That(fifthSession.booking.bookings.Count, Is.EqualTo(2));
            var bookingOne = fifthSession.booking.bookings[0];
            Assert.That(bookingOne.id, Is.EqualTo(FredOnAaronOrakei14To15SessionId));
            Assert.That(bookingOne.customer.id, Is.EqualTo(FredId));
            Assert.That(bookingOne.customer.firstName, Is.EqualTo(FRED_FIRST_NAME));
            Assert.That(bookingOne.customer.lastName, Is.EqualTo(FLINTSTONE_LAST_NAME));
            Assert.That(bookingOne.customer.email, Is.EqualTo(FredEmail));
            Assert.That(bookingOne.customer.phone, Is.EqualTo(FredPhone.ToUpper()));
            var bookingTwo = fifthSession.booking.bookings[1];
            Assert.That(bookingTwo.id, Is.EqualTo(BarneyOnAaronOrakei14To15SessionId));
            Assert.That(bookingTwo.customer.id, Is.EqualTo(BarneyId));
            Assert.That(bookingTwo.customer.firstName, Is.EqualTo(BARNEY_FIRST_NAME));
            Assert.That(bookingTwo.customer.lastName, Is.EqualTo(RUBBLE_LAST_NAME));
            Assert.That(bookingTwo.customer.email, Is.Null);
            Assert.That(bookingTwo.customer.phone, Is.Null);
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

        private void ThenReturnSessionsForServiceResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Payload, Is.Not.Null);
            var sessions = (List<SessionData>)response.Payload;
            Assert.That(sessions.Count, Is.EqualTo(5));
            foreach (var session in sessions)
                Assert.That(session.service.id, Is.EqualTo(MiniRedId));
        }
    }
}
