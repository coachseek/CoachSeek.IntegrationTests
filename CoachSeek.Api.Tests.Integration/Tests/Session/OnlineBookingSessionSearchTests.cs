using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class OnlineBookingSessionSearchTests : BaseSessionSearchTests
    {
        [Test]
        public void GivenInvalidSearchPeriod_WhenTrySearchForOnlineBookableSessions_ThenReturnInvalidSearchPeriodError()
        {
            var setup = RegisterBusiness();

            var criteria = GivenInvalidSearchPeriod();
            var response = WhenTrySearchForOnlineBookableSessions(criteria, setup);
            ThenReturnInvalidSearchPeriodError(response);
        }

        [Test]
        public void GivenNoSessionInSearchPeriod_WhenTrySearchForOnlineBookableSessions_ThenReturnNoSessionOrCourses()
        {
            var setup = RegisterBusiness();

            var criteria = GivenNoSessionInSearchPeriod();
            var response = WhenTrySearchForOnlineBookableSessions(criteria, setup);
            ThenReturnNoSessionOrCourses(response);
        }

        [Test]
        public void GivenInvalidCoachId_WhenTrySearchForOnlineBookableSessions_ThenReturnInvalidCoachIdError()
        {
            var setup = RegisterBusiness();

            var criteria = GivenInvalidCoachId();
            var response = WhenTrySearchForOnlineBookableSessions(criteria, setup);
            ThenReturnInvalidCoachIdError(response);
        }

        [Test]
        public void GivenValidCoachId_WhenTrySearchForOnlineBookableSessions_ThenReturnOnlineBookableSessionsAndCoursesForCoach()
        {
            var setup = RegisterBusiness();
            RegisterTestSessions(setup);
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterTestCourses(setup);
            RegisterFredOnTwoCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(setup);

            var criteria = GivenValidCoachId(setup);
            var response = WhenTrySearchForOnlineBookableSessions(criteria, setup);
            ThenReturnOnlineBookableSessionsAndCoursesForCoach(response, setup);
        }

        [Test]
        public void GivenInvalidLocationId_WhenTrySearchForOnlineBookableSessions_ThenReturnInvalidLocationIdError()
        {
            var setup = RegisterBusiness();

            var criteria = GivenInvalidLocationId();
            var response = WhenTrySearchForOnlineBookableSessions(criteria, setup);
            ThenReturnInvalidLocationIdError(response);
        }

        [Test]
        public void GivenValidLocationId_WhenTrySearchForOnlineBookableSessions_ThenReturnOnlineBookableSessionsAndCoursesForLocation()
        {
            var setup = RegisterBusiness();
            RegisterTestSessions(setup);
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterTestCourses(setup);
            RegisterFredOnTwoCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(setup);

            var criteria = GivenValidLocationId(setup);
            var response = WhenTrySearchForOnlineBookableSessions(criteria, setup);
            ThenReturnOnlineBookableSessionsAndCoursesForLocation(response, setup);
        }

        [Test]
        public void GivenInvalidServiceId_WhenTrySearchForOnlineBookableSessions_ThenReturnInvalidServiceIdError()
        {
            var setup = RegisterBusiness();

            var criteria = GivenInvalidServiceId();
            var response = WhenTrySearchForOnlineBookableSessions(criteria, setup);
            ThenReturnInvalidServiceIdError(response);
        }

        [Test]
        public void GivenValidServiceId_WhenTrySearchForOnlineBookableSessions_ThenReturnOnlineBookableSessionsAndCoursesForService()
        {
            var setup = RegisterBusiness();
            RegisterTestSessions(setup);
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterTestCourses(setup);
            RegisterFredOnTwoCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(setup);

            var criteria = GivenValidServiceId(setup);
            var response = WhenTrySearchForOnlineBookableSessions(criteria, setup);
            ThenReturnOnlineBookableSessionsAndCoursesForService(response, setup);
        }


        private ApiResponse WhenTrySearchForOnlineBookableSessions(Tuple<string, string, Guid?, Guid?, Guid?> criteria, SetupData setup)
        {
            var url = BuildOnlineBookingSearchUrl(criteria.Item1, criteria.Item2, criteria.Item3, criteria.Item4, criteria.Item5);
            return new TestBusinessAnonymousApiClient().Get<SessionSearchData>(setup.Business.Domain, url);
        }


        private void ThenReturnOnlineBookableSessionsAndCoursesForCoach(ApiResponse response, SetupData setup)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            // Standalone sessions
            var standalones = searchResult.Sessions;
            Assert.That(standalones.Count, Is.EqualTo(1));

            var firstStandalone = standalones[0];
            Assert.That(firstStandalone.id, Is.EqualTo(setup.AaronOrakeiMiniRed14To15.Id));
            Assert.That(firstStandalone.parentId, Is.Null);
            Assert.That(firstStandalone.coach.id, Is.EqualTo(setup.Aaron.Id));
            Assert.That(firstStandalone.coach.name, Is.EqualTo(setup.Aaron.Name));
            Assert.That(firstStandalone.timing.startTime, Is.EqualTo("14:00"));
            Assert.That(firstStandalone.booking.bookingCount, Is.EqualTo(1));
            Assert.That(firstStandalone.booking.bookings, Is.Null);     // Ensure booking details are gone

            // Courses
            var courses = searchResult.Courses;
            Assert.That(courses.Count, Is.EqualTo(1));

            var course = courses[0];
            Assert.That(course.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));
            Assert.That(course.parentId, Is.Null);
            Assert.That(course.coach.id, Is.EqualTo(setup.Aaron.Id));
            Assert.That(course.coach.name, Is.EqualTo(setup.Aaron.Name));
            Assert.That(course.timing.startDate, Is.EqualTo(GetDateFormatNumberOfDaysOut(14)));
            Assert.That(course.timing.startTime, Is.EqualTo("9:00"));
            Assert.That(course.booking.bookingCount, Is.EqualTo(1));
            Assert.That(course.booking.bookings, Is.Null);     // Ensure booking details are gone

            var firstSessionCourse = course.sessions[0];
            Assert.That(firstSessionCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(firstSessionCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var secondSessionCourse = course.sessions[1];
            Assert.That(secondSessionCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(secondSessionCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var thirdSessionCourse = course.sessions[2];
            Assert.That(thirdSessionCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(thirdSessionCourse.booking.bookings, Is.Null);     // Ensure booking details are gone
        }


        private void ThenReturnOnlineBookableSessionsAndCoursesForLocation(ApiResponse response, SetupData setup)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            // Standalone sessions
            var standalones = searchResult.Sessions;
            Assert.That(standalones.Count, Is.EqualTo(1));

            var firstStandalone = standalones[0];
            Assert.That(firstStandalone.id, Is.EqualTo(setup.AaronOrakeiMiniRed14To15.Id));
            Assert.That(firstStandalone.parentId, Is.Null);
            Assert.That(firstStandalone.coach.id, Is.EqualTo(setup.Aaron.Id));
            Assert.That(firstStandalone.coach.name, Is.EqualTo(setup.Aaron.Name));
            Assert.That(firstStandalone.timing.startTime, Is.EqualTo("14:00"));
            Assert.That(firstStandalone.booking.bookingCount, Is.EqualTo(1));
            Assert.That(firstStandalone.booking.bookings, Is.Null);     // Ensure booking details are gone

            // Courses
            var courses = searchResult.Courses;
            Assert.That(courses.Count, Is.EqualTo(1));

            var course = courses[0];
            Assert.That(course.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));
            Assert.That(course.parentId, Is.Null);
            Assert.That(course.coach.id, Is.EqualTo(setup.Aaron.Id));
            Assert.That(course.coach.name, Is.EqualTo(setup.Aaron.Name));
            Assert.That(course.timing.startDate, Is.EqualTo(GetDateFormatNumberOfDaysOut(14)));
            Assert.That(course.timing.startTime, Is.EqualTo("9:00"));
            Assert.That(course.booking.bookingCount, Is.EqualTo(1));
            Assert.That(course.booking.bookings, Is.Null);     // Ensure booking details are gone

            var firstSessionCourse = course.sessions[0];
            Assert.That(firstSessionCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(firstSessionCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var secondSessionCourse = course.sessions[1];
            Assert.That(secondSessionCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(secondSessionCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var thirdSessionCourse = course.sessions[2];
            Assert.That(thirdSessionCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(thirdSessionCourse.booking.bookings, Is.Null);     // Ensure booking details are gone
        }

        private void ThenReturnOnlineBookableSessionsAndCoursesForService(ApiResponse response, SetupData setup)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            // Standalone sessions
            var standalones = searchResult.Sessions;
            Assert.That(standalones.Count, Is.EqualTo(1));

            var firstStandalone = standalones[0];
            Assert.That(firstStandalone.id, Is.EqualTo(setup.AaronOrakeiMiniRed14To15.Id));
            Assert.That(firstStandalone.parentId, Is.Null);
            Assert.That(firstStandalone.coach.id, Is.EqualTo(setup.Aaron.Id));
            Assert.That(firstStandalone.coach.name, Is.EqualTo(setup.Aaron.Name));
            Assert.That(firstStandalone.timing.startTime, Is.EqualTo("14:00"));
            Assert.That(firstStandalone.booking.bookingCount, Is.EqualTo(1));
            Assert.That(firstStandalone.booking.bookings, Is.Null);     // Ensure booking details are gone

            // Courses
            var courses = searchResult.Courses;
            Assert.That(courses.Count, Is.EqualTo(1));

            var course = courses[0];
            Assert.That(course.id, Is.EqualTo(setup.BobbyRemueraMiniRed9To10For3Weeks.Id));
            Assert.That(course.parentId, Is.Null);
            Assert.That(course.coach.id, Is.EqualTo(setup.Bobby.Id));
            Assert.That(course.coach.name, Is.EqualTo(setup.Bobby.Name));
            Assert.That(course.timing.startDate, Is.EqualTo(GetDateFormatNumberOfDaysOut(10)));
            Assert.That(course.timing.startTime, Is.EqualTo("9:00"));
            Assert.That(course.booking.bookingCount, Is.EqualTo(0));
            Assert.That(course.booking.bookings, Is.Null);     // Ensure booking details are gone

            var firstSessionCourse = course.sessions[0];
            Assert.That(firstSessionCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(firstSessionCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var secondSessionCourse = course.sessions[1];
            Assert.That(secondSessionCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(secondSessionCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var thirdSessionCourse = course.sessions[2];
            Assert.That(thirdSessionCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(thirdSessionCourse.booking.bookings, Is.Null);     // Ensure booking details are gone
        }


        private string BuildOnlineBookingSearchUrl(string startDate, string endDate, Guid? coachId, Guid? locationId, Guid? serviceId)
        {
            var searchUrl = string.Format("OnlineBooking/Sessions?startDate={0}&endDate={1}", startDate, endDate);
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
