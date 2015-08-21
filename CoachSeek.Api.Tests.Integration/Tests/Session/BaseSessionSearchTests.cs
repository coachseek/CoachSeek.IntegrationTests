using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    public abstract class BaseSessionSearchTests : ScheduleTests
    {
        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenNoSearchPeriod(SetupData setup)
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(null, null, setup.Aaron.Id, setup.Orakei.Id, setup.MiniRed.Id);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidSearchPeriod()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("blah", "2015-02-30", null, null, null);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenStartDateAfterEndDate()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-02", "2015-01-01", null, null, null);
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
                                                                  GetDateFormatNumberOfDaysOut(21),
                                                                  setup.Aaron.Id, null, null);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidLocationId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", null, Guid.NewGuid(), null);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenValidLocationId(SetupData setup)
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(),
                                                                  GetDateFormatNumberOfDaysOut(21),
                                                                  null, setup.Orakei.Id, null);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidServiceId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", null, null, Guid.NewGuid());
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenValidServiceId(SetupData setup)
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(),
                                                                  GetDateFormatNumberOfDaysOut(21),
                                                                  null, null, setup.MiniRed.Id);
        }


        protected void ThenReturnNoSearchPeriodError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { ErrorCodes.StartDateRequired, "The StartDate field is required.", null },
                                                    { ErrorCodes.EndDateRequired, "The EndDate field is required.", null } });
        }

        protected void ThenReturnInvalidSearchPeriodError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { ErrorCodes.StartDateInvalid, "'blah' is not a valid start date.", "blah" },
                                                    { ErrorCodes.EndDateInvalid, "'2015-02-30' is not a valid end date.", "2015-02-30" } });
        }

        protected void ThenReturnStartDateAfterEndDateError(ApiResponse response)
        {
            AssertSingleError(response,
                              ErrorCodes.StartDateAfterEndDate, 
                              "Start date '2015-01-02' is after end date '2015-01-01'",
                              "Start date: '2015-01-02', End date: '2015-01-01'");
        }

        protected void ThenReturnInvalidCoachIdError(ApiResponse response, Guid coachId)
        {
            AssertSingleError(response, ErrorCodes.CoachInvalid, "This coach does not exist.", coachId.ToString());
        }

        protected void ThenReturnInvalidLocationIdError(ApiResponse response, Guid locationId)
        {
            AssertSingleError(response, ErrorCodes.LocationInvalid, "This location does not exist.", locationId.ToString());
        }

        protected void ThenReturnInvalidServiceIdError(ApiResponse response, Guid serviceId)
        {
            AssertSingleError(response, ErrorCodes.ServiceInvalid, "This service does not exist.", serviceId.ToString());
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
