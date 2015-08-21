using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class SessionNewSearchTests : BaseSessionSearchTests
    {
        [Test]
        public void GivenNoSearchPeriod_WhenTrySearch_ThenReturnNoSearchPeriodError()
        {
            var setup = RegisterBusiness();
            RegisterCoachAaron(setup);
            RegisterLocationOrakei(setup);
            RegisterServiceMiniRed(setup);

            var criteria = GivenNoSearchPeriod(setup);
            var response = WhenTrySearch(criteria, setup);
            ThenReturnNoSearchPeriodError(response);
        }

        [Test]
        public void GivenInvalidSearchPeriod_WhenTrySearch_ThenReturnInvalidSearchPeriodError()
        {
            var setup = RegisterBusiness();

            var criteria = GivenInvalidSearchPeriod();
            var response = WhenTrySearch(criteria, setup);
            ThenReturnInvalidSearchPeriodError(response);
        }

        [Test]
        public void GivenStartDateAfterEndDate_WhenTrySearch_ThenReturnStartDateAfterEndDateError()
        {
            var setup = RegisterBusiness();
            RegisterCoachAaron(setup);
            RegisterLocationOrakei(setup);
            RegisterServiceMiniRed(setup);

            var criteria = GivenStartDateAfterEndDate();
            var response = WhenTrySearch(criteria, setup);
            ThenReturnStartDateAfterEndDateError(response);
        }

        [Test]
        public void GivenNoSessionInSearchPeriod_WhenTrySearch_ThenReturnNoSessionOrCourses()
        {
            var setup = RegisterBusiness();

            var criteria = GivenNoSessionInSearchPeriod();
            var response = WhenTrySearch(criteria, setup);
            ThenReturnNoSessionOrCourses(response);
        }

        [Test]
        public void GivenInvalidCoachId_WhenTrySearch_ThenReturnInvalidCoachIdError()
        {
            var setup = RegisterBusiness();

            var criteria = GivenInvalidCoachId();
            var response = WhenTrySearch(criteria, setup);
            ThenReturnInvalidCoachIdError(response, criteria.Item3.Value);
        }

        [Test]
        public void GivenValidCoachId_WhenTrySearch_ThenReturnSessionsAndCoursesForCoach()
        {
            var setup = RegisterBusiness();
            RegisterTestSessions(setup);
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterTestCourses(setup);
            RegisterFredOnTwoCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(setup);

            var criteria = GivenValidCoachId(setup);
            var response = WhenTrySearch(criteria, setup);
            ThenReturnSessionsAndCoursesForCoach(response, setup);
        }

        [Test]
        public void GivenInvalidLocationId_WhenTrySearch_ThenReturnInvalidLocationIdError()
        {
            var setup = RegisterBusiness();

            var criteria = GivenInvalidLocationId();
            var response = WhenTrySearch(criteria, setup);
            ThenReturnInvalidLocationIdError(response, criteria.Item4.Value);
        }

        [Test]
        public void GivenValidLocationId_WhenTrySearch_ThenReturnSessionsAndCoursesForLocation()
        {
            var setup = RegisterBusiness();
            RegisterTestSessions(setup);
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterTestCourses(setup);
            RegisterFredOnTwoCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(setup);

            var criteria = GivenValidLocationId(setup);
            var response = WhenTrySearch(criteria, setup);
            ThenReturnSessionsAndCoursesForLocation(response, setup);
        }

        [Test]
        public void GivenInvalidServiceId_WhenTrySearch_ThenReturnInvalidServiceIdError()
        {
            var setup = RegisterBusiness();

            var criteria = GivenInvalidServiceId();
            var response = WhenTrySearch(criteria, setup);
            ThenReturnInvalidServiceIdError(response, criteria.Item5.Value);
        }

        [Test]
        public void GivenValidServiceId_WhenTrySearch_ThenReturnSessionsAndCoursesForService()
        {
            var setup = RegisterBusiness();
            RegisterTestSessions(setup);
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterTestCourses(setup);
            RegisterFredOnTwoCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(setup);

            var criteria = GivenValidServiceId(setup);
            var response = WhenTrySearch(criteria, setup);
            ThenReturnSessionsAndCoursesForService(response, setup);
        }


        private ApiResponse WhenTrySearch(Tuple<string, string, Guid?, Guid?, Guid?> criteria, SetupData setup)
        {
            var url = BuildSearchUrl(criteria.Item1, criteria.Item2, criteria.Item3, criteria.Item4, criteria.Item5);
            return new TestAuthenticatedApiClient().Get<SessionSearchData>(setup.Business.UserName,
                                                                           setup.Business.Password,
                                                                           url);
        }


        private void ThenReturnSessionsAndCoursesForCoach(ApiResponse response, SetupData setup)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            // Standalone sessions
            var standalones = searchResult.Sessions;
            Assert.That(standalones.Count, Is.EqualTo(2));

            var firstStandalone = standalones[0];
            Assert.That(firstStandalone.id, Is.EqualTo(setup.AaronOrakeiMiniRed16To17.Id));
            Assert.That(firstStandalone.parentId, Is.Null);
            Assert.That(firstStandalone.coach.id, Is.EqualTo(setup.Aaron.Id));
            Assert.That(firstStandalone.coach.name, Is.EqualTo(setup.Aaron.Name));
            Assert.That(firstStandalone.timing.startTime, Is.EqualTo("16:00"));

            var secondStandalone = standalones[1];
            Assert.That(secondStandalone.id, Is.EqualTo(setup.AaronOrakeiMiniRed14To15.Id));
            Assert.That(secondStandalone.parentId, Is.Null);
            Assert.That(secondStandalone.coach.id, Is.EqualTo(setup.Aaron.Id));
            Assert.That(secondStandalone.coach.name, Is.EqualTo(setup.Aaron.Name));
            Assert.That(secondStandalone.timing.startTime, Is.EqualTo("14:00"));
            Assert.That(secondStandalone.booking.bookings.Count, Is.EqualTo(1));
            var bookingOne = secondStandalone.booking.bookings[0];
            Assert.That(bookingOne.id, Is.EqualTo(setup.FredOnAaronOrakeiMiniRed14To15.Id));
            Assert.That(bookingOne.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(bookingOne.customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(bookingOne.customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(bookingOne.customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(bookingOne.customer.phone, Is.EqualTo(setup.Fred.Phone.ToUpper()));

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
            Assert.That(course.booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions.Count, Is.EqualTo(3));

            var firstSessionCourse = course.sessions[0];
            Assert.That(firstSessionCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(firstSessionCourse.booking.bookings.Count, Is.EqualTo(1));

            var secondSessionCourse = course.sessions[1];
            Assert.That(secondSessionCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(secondSessionCourse.booking.bookings.Count, Is.EqualTo(0));

            var thirdSessionCourse = course.sessions[2];
            Assert.That(thirdSessionCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(thirdSessionCourse.booking.bookings.Count, Is.EqualTo(1));
        }

        private void ThenReturnSessionsAndCoursesForLocation(ApiResponse response, SetupData setup)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            // Standalone sessions
            var standalones = searchResult.Sessions;
            Assert.That(standalones.Count, Is.EqualTo(2));

            var firstStandalone = standalones[0];
            Assert.That(firstStandalone.id, Is.EqualTo(setup.AaronOrakeiMiniRed16To17.Id));
            Assert.That(firstStandalone.parentId, Is.Null);
            Assert.That(firstStandalone.coach.id, Is.EqualTo(setup.Aaron.Id));
            Assert.That(firstStandalone.coach.name, Is.EqualTo(setup.Aaron.Name));
            Assert.That(firstStandalone.timing.startTime, Is.EqualTo("16:00"));

            var secondStandalone = standalones[1];
            Assert.That(secondStandalone.id, Is.EqualTo(setup.AaronOrakeiMiniRed14To15.Id));
            Assert.That(secondStandalone.parentId, Is.Null);
            Assert.That(secondStandalone.coach.id, Is.EqualTo(setup.Aaron.Id));
            Assert.That(secondStandalone.coach.name, Is.EqualTo(setup.Aaron.Name));
            Assert.That(secondStandalone.timing.startTime, Is.EqualTo("14:00"));
            Assert.That(secondStandalone.booking.bookings.Count, Is.EqualTo(1));
            var bookingOne = secondStandalone.booking.bookings[0];
            Assert.That(bookingOne.id, Is.EqualTo(setup.FredOnAaronOrakeiMiniRed14To15.Id));
            Assert.That(bookingOne.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(bookingOne.customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(bookingOne.customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(bookingOne.customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(bookingOne.customer.phone, Is.EqualTo(setup.Fred.Phone.ToUpper()));

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
            Assert.That(course.booking.bookings.Count, Is.EqualTo(1));
            Assert.That(course.sessions.Count, Is.EqualTo(3));

            var firstSessionCourse = course.sessions[0];
            Assert.That(firstSessionCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(firstSessionCourse.booking.bookings.Count, Is.EqualTo(1));

            var secondSessionCourse = course.sessions[1];
            Assert.That(secondSessionCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(secondSessionCourse.booking.bookings.Count, Is.EqualTo(0));

            var thirdSessionCourse = course.sessions[2];
            Assert.That(thirdSessionCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(thirdSessionCourse.booking.bookings.Count, Is.EqualTo(1));
        }

        protected void ThenReturnSessionsAndCoursesForService(ApiResponse response, SetupData setup)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            // Standalone sessions
            var standalones = searchResult.Sessions;
            Assert.That(standalones.Count, Is.EqualTo(2));

            var firstStandalone = standalones[0];
            Assert.That(firstStandalone.id, Is.EqualTo(setup.AaronOrakeiMiniRed16To17.Id));
            Assert.That(firstStandalone.parentId, Is.Null);
            Assert.That(firstStandalone.coach.id, Is.EqualTo(setup.Aaron.Id));
            Assert.That(firstStandalone.coach.name, Is.EqualTo(setup.Aaron.Name));
            Assert.That(firstStandalone.timing.startTime, Is.EqualTo("16:00"));

            var secondStandalone = standalones[1];
            Assert.That(secondStandalone.id, Is.EqualTo(setup.AaronOrakeiMiniRed14To15.Id));
            Assert.That(secondStandalone.parentId, Is.Null);
            Assert.That(secondStandalone.coach.id, Is.EqualTo(setup.Aaron.Id));
            Assert.That(secondStandalone.coach.name, Is.EqualTo(setup.Aaron.Name));
            Assert.That(secondStandalone.timing.startTime, Is.EqualTo("14:00"));
            Assert.That(secondStandalone.booking.bookings.Count, Is.EqualTo(1));
            var bookingOne = secondStandalone.booking.bookings[0];
            Assert.That(bookingOne.id, Is.EqualTo(setup.FredOnAaronOrakeiMiniRed14To15.Id));
            Assert.That(bookingOne.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(bookingOne.customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(bookingOne.customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(bookingOne.customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(bookingOne.customer.phone, Is.EqualTo(setup.Fred.Phone.ToUpper()));

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
            Assert.That(course.booking.bookings.Count, Is.EqualTo(0));
            Assert.That(course.sessions.Count, Is.EqualTo(3));

            var firstSessionCourse = course.sessions[0];
            Assert.That(firstSessionCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(firstSessionCourse.booking.bookings.Count, Is.EqualTo(0));

            var secondSessionCourse = course.sessions[1];
            Assert.That(secondSessionCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(secondSessionCourse.booking.bookings.Count, Is.EqualTo(0));

            var thirdSessionCourse = course.sessions[2];
            Assert.That(thirdSessionCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(thirdSessionCourse.booking.bookings.Count, Is.EqualTo(0));
        }
    }
}
