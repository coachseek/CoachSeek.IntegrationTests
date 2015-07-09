using System;
using System.Collections.Generic;
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

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenValidCoachId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(), GetFormattedDateThreeWeeksOut(), Aaron.Id, null, null);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidLocationId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", null, Guid.NewGuid(), null);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenValidLocationId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(), GetFormattedDateThreeWeeksOut(), null, Orakei.Id, null);
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidServiceId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", null, null, Guid.NewGuid());
        }

        protected Tuple<string, string, Guid?, Guid?, Guid?> GivenValidServiceId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(), GetFormattedDateThreeWeeksOut(), null, null, MiniRed.Id);
        }


        protected Response WhenTryOldSearch(Tuple<string, string, Guid?, Guid?, Guid?> criteria)
        {
            var url = BuildSearchUrl(criteria.Item1, criteria.Item2, criteria.Item3, criteria.Item4, criteria.Item5);

            return AuthenticatedGet<List<SessionData>>(url);
        }


        protected void ThenReturnInvalidSearchPeriodError(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "The startDate is not a valid date.", "startDate" },
                                                    { "The endDate is not a valid date.", "endDate" } });
        }

        protected void ThenReturnInvalidCoachIdError(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "Not a valid coachId.", "coachId" } });
        }

        protected void ThenReturnInvalidLocationIdError(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "Not a valid locationId.", "locationId" } });
        }

        protected void ThenReturnInvalidServiceIdError(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "Not a valid serviceId.", "serviceId" } });
        }

        protected void ThenReturnNoSessionOrCourses(Response response)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            var sessions = searchResult.Sessions;
            Assert.That(sessions.Count, Is.EqualTo(0));

            var courses = searchResult.Courses;
            Assert.That(courses.Count, Is.EqualTo(0));
        }

        protected void ThenReturnSessionsAndCoursesForService(Response response)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            // Standalone sessions
            var standalones = searchResult.Sessions;
            Assert.That(standalones.Count, Is.EqualTo(2));

            var firstStandalone = standalones[0];
            Assert.That(firstStandalone.id, Is.EqualTo(AaronOrakeiMiniRed16To17.Id));
            Assert.That(firstStandalone.parentId, Is.Null);
            Assert.That(firstStandalone.location.id, Is.EqualTo(Orakei.Id));
            Assert.That(firstStandalone.location.name, Is.EqualTo(Orakei.Name));
            Assert.That(firstStandalone.timing.startTime, Is.EqualTo("16:00"));

            var secondStandalone = standalones[1];
            Assert.That(secondStandalone.id, Is.EqualTo(AaronOrakeiMiniRed14To15.Id));
            Assert.That(secondStandalone.parentId, Is.Null);
            Assert.That(secondStandalone.location.id, Is.EqualTo(Orakei.Id));
            Assert.That(secondStandalone.location.name, Is.EqualTo(Orakei.Name));
            Assert.That(secondStandalone.timing.startTime, Is.EqualTo("14:00"));
            Assert.That(secondStandalone.booking.bookings.Count, Is.EqualTo(2));

            // Courses
            var courses = searchResult.Courses;
            Assert.That(courses.Count, Is.EqualTo(1));

            var firstCourse = courses[0];
            Assert.That(firstCourse.id, Is.EqualTo(AaronRemuera9To10For4WeeksCourseId));
            Assert.That(firstCourse.parentId, Is.Null);
            Assert.That(firstCourse.coach.id, Is.EqualTo(Aaron.Id));
            Assert.That(firstCourse.coach.name, Is.EqualTo(Aaron.Name));
            Assert.That(firstCourse.timing.startDate, Is.EqualTo(GetDateFormatNumberOfWeeksOut(1)));
            Assert.That(firstCourse.timing.startTime, Is.EqualTo("9:00"));
            Assert.That(firstCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(firstCourse.booking.bookings.Count, Is.EqualTo(0));
            Assert.That(firstCourse.sessions.Count, Is.EqualTo(4));

            var firstSessionFirstCourse = firstCourse.sessions[0];
            Assert.That(firstSessionFirstCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(firstSessionFirstCourse.booking.bookings.Count, Is.EqualTo(0));

            var secondSessionFirstCourse = firstCourse.sessions[1];
            Assert.That(secondSessionFirstCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(secondSessionFirstCourse.booking.bookings.Count, Is.EqualTo(0));

            var thirdSessionFirstCourse = firstCourse.sessions[2];
            Assert.That(thirdSessionFirstCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(thirdSessionFirstCourse.booking.bookings.Count, Is.EqualTo(0));

            var fourthSessionFirstCourse = firstCourse.sessions[3];
            Assert.That(fourthSessionFirstCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(fourthSessionFirstCourse.booking.bookings.Count, Is.EqualTo(0));
        }


        protected string BuildSearchUrl(string startDate, string endDate, Guid? coachId, Guid? locationId, Guid? serviceId)
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
    }
}
