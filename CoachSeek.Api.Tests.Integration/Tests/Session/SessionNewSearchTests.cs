using System;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class SessionNewSearchTests : BaseSessionSearchTests
    {
        [SetUp]
        public void Setup()
        {
            FullySetupNewTestBusiness();
        }


        [Test]
        public void GivenInvalidSearchPeriod_WhenTrySearch_ThenReturnInvalidSearchPeriodError()
        {
            var criteria = GivenInvalidSearchPeriod();
            var response = WhenTrySearch(criteria);
            ThenReturnInvalidSearchPeriodError(response);
        }

        [Test]
        public void GivenNoSessionInSearchPeriod_WhenTrySearch_ThenReturnNoSessionOrCourses()
        {
            var criteria = GivenNoSessionInSearchPeriod();
            var response = WhenTrySearch(criteria);
            ThenReturnNoSessionOrCourses(response);
        }

        [Test]
        public void GivenInvalidCoachId_WhenTrySearch_ThenReturnInvalidCoachIdError()
        {
            var criteria = GivenInvalidCoachId();
            var response = WhenTrySearch(criteria);
            ThenReturnInvalidCoachIdError(response);
        }

        [Test]
        public void GivenValidCoachId_WhenTrySearch_ThenReturnSessionsAndCoursesForCoach()
        {
            var criteria = GivenValidCoachId();
            var response = WhenTrySearch(criteria);
            ThenReturnSessionsAndCoursesForCoach(response);
        }

        [Test]
        public void GivenInvalidLocationId_WhenTrySearch_ThenReturnInvalidLocationIdError()
        {
            var criteria = GivenInvalidLocationId();
            var response = WhenTrySearch(criteria);
            ThenReturnInvalidLocationIdError(response);
        }

        [Test]
        public void GivenValidLocationId_WhenTrySearch_ThenReturnSessionsAndCoursesForLocation()
        {
            var criteria = GivenValidLocationId();
            var response = WhenTrySearch(criteria);
            ThenReturnSessionsAndCoursesForLocation(response);
        }

        [Test]
        public void GivenInvalidServiceId_WhenTrySearch_ThenReturnInvalidServiceIdError()
        {
            var criteria = GivenInvalidServiceId();
            var response = WhenTrySearch(criteria);
            ThenReturnInvalidServiceIdError(response);
        }

        [Test]
        public void GivenValidServiceId_WhenTrySearch_ThenReturnSessionsAndCoursesForService()
        {
            var criteria = GivenValidServiceId();
            var response = WhenTrySearch(criteria);
            ThenReturnSessionsAndCoursesForService(response);
        }


        private Response WhenTrySearch(Tuple<string, string, Guid?, Guid?, Guid?> criteria)
        {
            var url = BuildSearchUrl(criteria.Item1, criteria.Item2, criteria.Item3, criteria.Item4, criteria.Item5);
            url = string.Format("{0}&useNewSearch=true", url);

            return AuthenticatedGet<SessionSearchData>(url);
        }


        private void ThenReturnSessionsAndCoursesForCoach(Response response)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            // Standalone sessions
            var standalones = searchResult.Sessions;
            Assert.That(standalones.Count, Is.EqualTo(2));

            var firstStandalone = standalones[0];
            Assert.That(firstStandalone.id, Is.EqualTo(AaronOrakei16To17.Id));
            Assert.That(firstStandalone.parentId, Is.Null);
            Assert.That(firstStandalone.coach.id, Is.EqualTo(Aaron.Id));
            Assert.That(firstStandalone.coach.name, Is.EqualTo(Aaron.Name));
            Assert.That(firstStandalone.timing.startTime, Is.EqualTo("16:00"));

            var secondStandalone = standalones[1];
            Assert.That(secondStandalone.id, Is.EqualTo(AaronOrakei14To15.Id));
            Assert.That(secondStandalone.parentId, Is.Null);
            Assert.That(secondStandalone.coach.id, Is.EqualTo(Aaron.Id));
            Assert.That(secondStandalone.coach.name, Is.EqualTo(Aaron.Name));
            Assert.That(secondStandalone.timing.startTime, Is.EqualTo("14:00"));
            Assert.That(secondStandalone.booking.bookings.Count, Is.EqualTo(2));
            var bookingOne = secondStandalone.booking.bookings[0];
            Assert.That(bookingOne.id, Is.EqualTo(FredOnAaronOrakei14To15SessionId));
            Assert.That(bookingOne.customer.id, Is.EqualTo(Fred.Id));
            Assert.That(bookingOne.customer.firstName, Is.EqualTo(Fred.FirstName));
            Assert.That(bookingOne.customer.lastName, Is.EqualTo(Fred.LastName));
            Assert.That(bookingOne.customer.email, Is.EqualTo(Fred.Email));
            Assert.That(bookingOne.customer.phone, Is.EqualTo(Fred.Phone.ToUpper()));
            var bookingTwo = secondStandalone.booking.bookings[1];
            Assert.That(bookingTwo.id, Is.EqualTo(BarneyOnAaronOrakei14To15SessionId));
            Assert.That(bookingTwo.customer.id, Is.EqualTo(Barney.Id));
            Assert.That(bookingTwo.customer.firstName, Is.EqualTo(Barney.FirstName));
            Assert.That(bookingTwo.customer.lastName, Is.EqualTo(Barney.LastName));
            Assert.That(bookingTwo.customer.email, Is.Null);
            Assert.That(bookingTwo.customer.phone, Is.Null);

            // Courses
            var courses = searchResult.Courses;
            Assert.That(courses.Count, Is.EqualTo(2));

            var firstCourse = courses[0];
            Assert.That(firstCourse.id, Is.EqualTo(AaronOrakeiMiniBlueFor2DaysCourseId));
            Assert.That(firstCourse.parentId, Is.Null);
            Assert.That(firstCourse.coach.id, Is.EqualTo(Aaron.Id));
            Assert.That(firstCourse.coach.name, Is.EqualTo(Aaron.Name));
            Assert.That(firstCourse.timing.startDate, Is.EqualTo(GetDateFormatNumberOfDaysOut(1)));
            Assert.That(firstCourse.timing.startTime, Is.EqualTo("9:00"));
            Assert.That(firstCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(firstCourse.booking.bookings.Count, Is.EqualTo(1));
            Assert.That(firstCourse.sessions.Count, Is.EqualTo(2));

            var firstSessionFirstCourse = firstCourse.sessions[0];
            Assert.That(firstSessionFirstCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(firstSessionFirstCourse.booking.bookings.Count, Is.EqualTo(1));

            var secondSessionFirstCourse = firstCourse.sessions[1];
            Assert.That(secondSessionFirstCourse.booking.bookingCount, Is.EqualTo(2));
            Assert.That(secondSessionFirstCourse.booking.bookings.Count, Is.EqualTo(2));

            var secondCourse = courses[1];
            Assert.That(secondCourse.id, Is.EqualTo(AaronRemuera9To10For4WeeksCourseId));
            Assert.That(secondCourse.parentId, Is.Null);
            Assert.That(secondCourse.coach.id, Is.EqualTo(Aaron.Id));
            Assert.That(secondCourse.coach.name, Is.EqualTo(Aaron.Name));
            Assert.That(firstCourse.timing.startDate, Is.EqualTo(GetDateFormatNumberOfDaysOut(1)));
            Assert.That(secondCourse.timing.startTime, Is.EqualTo("9:00"));
            Assert.That(secondCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(secondCourse.booking.bookings.Count, Is.EqualTo(0));
            Assert.That(secondCourse.sessions.Count, Is.EqualTo(4));

            var firstSessionSecondCourse = secondCourse.sessions[0];
            Assert.That(firstSessionSecondCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(firstSessionSecondCourse.booking.bookings.Count, Is.EqualTo(0));

            var secondSessionSecondCourse = secondCourse.sessions[1];
            Assert.That(secondSessionSecondCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(secondSessionSecondCourse.booking.bookings.Count, Is.EqualTo(0));

            var thirdSessionSecondCourse = secondCourse.sessions[2];
            Assert.That(thirdSessionSecondCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(thirdSessionSecondCourse.booking.bookings.Count, Is.EqualTo(0));

            var fourthSessionSecondCourse = secondCourse.sessions[3];
            Assert.That(fourthSessionSecondCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(fourthSessionSecondCourse.booking.bookings.Count, Is.EqualTo(0));
        }

        private void ThenReturnSessionsAndCoursesForLocation(Response response)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            // Standalone sessions
            var standalones = searchResult.Sessions;
            Assert.That(standalones.Count, Is.EqualTo(2));

            var firstStandalone = standalones[0];
            Assert.That(firstStandalone.id, Is.EqualTo(AaronOrakei16To17.Id));
            Assert.That(firstStandalone.parentId, Is.Null);
            Assert.That(firstStandalone.location.id, Is.EqualTo(Orakei.Id));
            Assert.That(firstStandalone.location.name, Is.EqualTo(Orakei.Name));
            Assert.That(firstStandalone.timing.startTime, Is.EqualTo("16:00"));

            var secondStandalone = standalones[1];
            Assert.That(secondStandalone.id, Is.EqualTo(AaronOrakei14To15.Id));
            Assert.That(secondStandalone.parentId, Is.Null);
            Assert.That(secondStandalone.location.id, Is.EqualTo(Orakei.Id));
            Assert.That(secondStandalone.location.name, Is.EqualTo(Orakei.Name));
            Assert.That(secondStandalone.timing.startTime, Is.EqualTo("14:00"));
            Assert.That(secondStandalone.booking.bookings.Count, Is.EqualTo(2));

            // Courses
            var courses = searchResult.Courses;
            Assert.That(courses.Count, Is.EqualTo(1));

            var firstCourse = courses[0];
            Assert.That(firstCourse.id, Is.EqualTo(AaronOrakeiMiniBlueFor2DaysCourseId));
            Assert.That(firstCourse.parentId, Is.Null);
            Assert.That(firstCourse.location.id, Is.EqualTo(Orakei.Id));
            Assert.That(firstCourse.location.name, Is.EqualTo(Orakei.Name));
            Assert.That(firstCourse.timing.startDate, Is.EqualTo(GetDateFormatNumberOfDaysOut(1)));
            Assert.That(firstCourse.timing.startTime, Is.EqualTo("9:00"));
            Assert.That(firstCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(firstCourse.booking.bookings.Count, Is.EqualTo(1));
            Assert.That(firstCourse.sessions.Count, Is.EqualTo(2));

            var firstSessionFirstCourse = firstCourse.sessions[0];
            Assert.That(firstSessionFirstCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(firstSessionFirstCourse.booking.bookings.Count, Is.EqualTo(1));

            var secondSessionFirstCourse = firstCourse.sessions[1];
            Assert.That(secondSessionFirstCourse.booking.bookingCount, Is.EqualTo(2));
            Assert.That(secondSessionFirstCourse.booking.bookings.Count, Is.EqualTo(2));
        }
    }
}
