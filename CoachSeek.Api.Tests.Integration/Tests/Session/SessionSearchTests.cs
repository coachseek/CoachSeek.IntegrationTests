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

        private string BuildSearchUrl(string startDate, string endDate)
        {
            return string.Format("{0}/{1}?startDate={2}&endDate={3}", BaseUrl, RelativePath, startDate, endDate);
        }


        [Test]
        public void GivenInvalidSearchCriteria_WhenSearch_ThenReturnErrorResponse()
        {
            var criteria = GivenInvalidSearchCriteria();
            var response = WhenSearch(criteria);
            ThenReturnErrorResponse(response);
        }

        [Test]
        public void GivenNoSessionInSearchPeriod_WhenSearch_ThenReturnNoSessionResponse()
        {
            var criteria = GivenNoSessionInSearchPeriod();
            var response = WhenSearch(criteria);
            ThenReturnNoSessionResponse(response);
        }


        private Tuple<string, string> GivenInvalidSearchCriteria()
        {
            return new Tuple<string, string>("blah", "2015-02-30");
        }

        private Tuple<string, string> GivenNoSessionInSearchPeriod()
        {
            return new Tuple<string, string>("2015-01-01", "2015-01-02");
        }


        private Response WhenSearch(Tuple<string, string> criteria)
        {
            var url = BuildSearchUrl(criteria.Item1, criteria.Item2);

            return Get<List<SessionData>>(url);
        }


        private void ThenReturnErrorResponse(Response response)
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
    }
}
