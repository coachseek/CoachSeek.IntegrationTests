using System;
using System.Collections.Generic;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    public abstract class BaseSessionSearchTests : ScheduleTests
    {
        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidSearchPeriod()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("blah", "2015-02-30", null, null, null);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenNoSessionInSearchPeriod()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", null, null, null);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidCoachId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", Guid.NewGuid(), null, null);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenValidCoachId(SetupData setup)
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(), 
                                                                  GetFormattedDateThreeWeeksOut(),
                                                                  setup.Aaron.Id, null, null);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidLocationId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", null, Guid.NewGuid(), null);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenValidLocationId(SetupData setup)
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(), 
                                                                  GetFormattedDateThreeWeeksOut(),
                                                                  null, setup.Orakei.Id, null);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidServiceId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", null, null, Guid.NewGuid());
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenValidServiceId(SetupData setup)
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(), 
                                                                  GetFormattedDateThreeWeeksOut(), 
                                                                  null, null, setup.MiniRed.Id);
        }


        protected Response WhenTryOldSearch(Tuple<string, string, Guid?, Guid?, Guid?> criteria)
        {
            var url = BuildSearchUrl(criteria.Item1, criteria.Item2, criteria.Item3, criteria.Item4, criteria.Item5);

            return AuthenticatedGet<List<SessionData>>(url);
        }


        protected void ThenReturnInvalidSearchPeriodError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "The startDate is not a valid date.", "startDate" },
                                                    { "The endDate is not a valid date.", "endDate" } });
        }

        protected void ThenReturnInvalidCoachIdError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "Not a valid coachId.", "coachId" } });
        }

        protected void ThenReturnInvalidLocationIdError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "Not a valid locationId.", "locationId" } });
        }

        protected void ThenReturnInvalidServiceIdError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "Not a valid serviceId.", "serviceId" } });
        }

        protected void ThenReturnNoSessionOrCourses(ApiResponse response)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            var sessions = searchResult.Sessions;
            Assert.That(sessions.Count, Is.EqualTo(0));

            var courses = searchResult.Courses;
            Assert.That(courses.Count, Is.EqualTo(0));
        }


        protected string BuildSearchUrl(string startDate, string endDate, Guid? coachId, Guid? locationId, Guid? serviceId)
        {
            var searchUrl = string.Format("{0}?startDate={1}&endDate={2}", RelativePath, startDate, endDate);
            if (coachId.HasValue)
                searchUrl = string.Format("{0}&coachId={1}", searchUrl, coachId.Value);
            if (locationId.HasValue)
                searchUrl = string.Format("{0}&locationId={1}", searchUrl, locationId.Value);
            if (serviceId.HasValue)
                searchUrl = string.Format("{0}&serviceId={1}", searchUrl, serviceId.Value);
            return searchUrl;
        }
    }
}
