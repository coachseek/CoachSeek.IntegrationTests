using System;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class OnlineBookingSessionSearchTests : BaseSessionSearchTests
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
        public void GivenValidCoachId_WhenTrySearch_ThenReturnOnlineBookableSessionsAndCoursesForCoach()
        {
            var criteria = GivenValidCoachId();
            var response = WhenTrySearch(criteria);
            ThenReturnOnlineBookableSessionsAndCoursesForCoach(response);
        }

        [Test]
        public void GivenInvalidLocationId_WhenTrySearch_ThenReturnInvalidLocationIdError()
        {
            var criteria = GivenInvalidLocationId();
            var response = WhenTrySearch(criteria);
            ThenReturnInvalidLocationIdError(response);
        }

        [Test]
        public void GivenValidLocationId_WhenTrySearch_ThenReturnOnlineBookableSessionsAndCoursesForLocation()
        {
            var criteria = GivenValidLocationId();
            var response = WhenTrySearch(criteria);
            ThenReturnOnlineBookableSessionsAndCoursesForLocation(response);
        }

        [Test]
        public void GivenInvalidServiceId_WhenTrySearch_ThenReturnInvalidServiceIdError()
        {
            var criteria = GivenInvalidServiceId();
            var response = WhenTrySearch(criteria);
            ThenReturnInvalidServiceIdError(response);
        }

        [Test]
        public void GivenValidServiceId_WhenTrySearch_ThenReturnOnlineBookableSessionsAndCoursesForService()
        {
            var criteria = GivenValidServiceId();
            var response = WhenTrySearch(criteria);
            ThenReturnOnlineBookableSessionsAndCoursesForService(response);
        }


        private Response WhenTrySearch(Tuple<string, string, Guid?, Guid?, Guid?> criteria)
        {
            var url = BuildOnlineBookingSearchUrl(criteria.Item1, criteria.Item2, criteria.Item3, criteria.Item4, criteria.Item5);

            return GetAnonymously<SessionSearchData>(url);
        }


        private void ThenReturnOnlineBookableSessionsAndCoursesForCoach(Response response)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            // Standalone sessions
            var standalones = searchResult.Sessions;
            Assert.That(standalones.Count, Is.EqualTo(1));

            var firstStandalone = standalones[0];
            Assert.That(firstStandalone.id, Is.EqualTo(AaronOrakei14To15.Id));
            Assert.That(firstStandalone.parentId, Is.Null);
            Assert.That(firstStandalone.coach.id, Is.EqualTo(Aaron.Id));
            Assert.That(firstStandalone.coach.name, Is.EqualTo(Aaron.Name));
            Assert.That(firstStandalone.timing.startTime, Is.EqualTo("14:00"));
            Assert.That(firstStandalone.booking.bookingCount, Is.EqualTo(2));
            Assert.That(firstStandalone.booking.bookings, Is.Null);     // Ensure booking details are gone

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
            Assert.That(firstCourse.booking.bookings, Is.Null);     // Ensure booking details are gone
            Assert.That(firstCourse.sessions.Count, Is.EqualTo(2));

            var firstSessionFirstCourse = firstCourse.sessions[0];
            Assert.That(firstSessionFirstCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(firstSessionFirstCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var secondSessionFirstCourse = firstCourse.sessions[1];
            Assert.That(secondSessionFirstCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(secondSessionFirstCourse.booking.bookings, Is.Null);    // Ensure booking details are gone

            var secondCourse = courses[1];
            Assert.That(secondCourse.id, Is.EqualTo(AaronRemuera9To10For4WeeksCourseId));
            Assert.That(secondCourse.parentId, Is.Null);
            Assert.That(secondCourse.coach.id, Is.EqualTo(Aaron.Id));
            Assert.That(secondCourse.coach.name, Is.EqualTo(Aaron.Name));
            Assert.That(firstCourse.timing.startDate, Is.EqualTo(GetDateFormatNumberOfDaysOut(1)));
            Assert.That(secondCourse.timing.startTime, Is.EqualTo("9:00"));
            Assert.That(secondCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(secondCourse.booking.bookings, Is.Null);     // Ensure booking details are gone
            Assert.That(secondCourse.sessions.Count, Is.EqualTo(4));

            var firstSessionSecondCourse = secondCourse.sessions[0];
            Assert.That(firstSessionSecondCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(firstSessionSecondCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var secondSessionSecondCourse = secondCourse.sessions[1];
            Assert.That(secondSessionSecondCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(secondSessionSecondCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var thirdSessionSecondCourse = secondCourse.sessions[2];
            Assert.That(thirdSessionSecondCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(thirdSessionSecondCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var fourthSessionSecondCourse = secondCourse.sessions[3];
            Assert.That(fourthSessionSecondCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(fourthSessionSecondCourse.booking.bookings, Is.Null);     // Ensure booking details are gone
        }


        private void ThenReturnOnlineBookableSessionsAndCoursesForLocation(Response response)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            // Standalone sessions
            var standalones = searchResult.Sessions;
            Assert.That(standalones.Count, Is.EqualTo(1));

            var firstStandalone = standalones[0];
            Assert.That(firstStandalone.id, Is.EqualTo(AaronOrakei14To15.Id));
            Assert.That(firstStandalone.parentId, Is.Null);
            Assert.That(firstStandalone.location.id, Is.EqualTo(Orakei.Id));
            Assert.That(firstStandalone.location.name, Is.EqualTo(Orakei.Name));
            Assert.That(firstStandalone.timing.startTime, Is.EqualTo("14:00"));
            Assert.That(firstStandalone.booking.bookingCount, Is.EqualTo(2));
            Assert.That(firstStandalone.booking.bookings, Is.Null); // Ensure booking details are gone

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
            Assert.That(firstCourse.booking.bookings, Is.Null);     // Ensure booking details are gone
            Assert.That(firstCourse.sessions.Count, Is.EqualTo(2));

            var firstSessionFirstCourse = firstCourse.sessions[0];
            Assert.That(firstSessionFirstCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(firstSessionFirstCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var secondSessionFirstCourse = firstCourse.sessions[1];
            Assert.That(secondSessionFirstCourse.booking.bookingCount, Is.EqualTo(1));
            Assert.That(secondSessionFirstCourse.booking.bookings, Is.Null);    // Ensure booking details are gone
        }

        private void ThenReturnOnlineBookableSessionsAndCoursesForService(Response response)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            // Standalone sessions
            var standalones = searchResult.Sessions;
            Assert.That(standalones.Count, Is.EqualTo(1));

            var firstStandalone = standalones[0];
            Assert.That(firstStandalone.id, Is.EqualTo(AaronOrakei14To15.Id));
            Assert.That(firstStandalone.parentId, Is.Null);
            Assert.That(firstStandalone.location.id, Is.EqualTo(Orakei.Id));
            Assert.That(firstStandalone.location.name, Is.EqualTo(Orakei.Name));
            Assert.That(firstStandalone.timing.startTime, Is.EqualTo("14:00"));
            Assert.That(firstStandalone.booking.bookingCount, Is.EqualTo(2));
            Assert.That(firstStandalone.booking.bookings, Is.Null); // Ensure booking details are gone

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
            Assert.That(firstCourse.booking.bookings, Is.Null);     // Ensure booking details are gone
            Assert.That(firstCourse.sessions.Count, Is.EqualTo(4));

            var firstSessionFirstCourse = firstCourse.sessions[0];
            Assert.That(firstSessionFirstCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(firstSessionFirstCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var secondSessionFirstCourse = firstCourse.sessions[1];
            Assert.That(secondSessionFirstCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(secondSessionFirstCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var thirdSessionFirstCourse = firstCourse.sessions[2];
            Assert.That(thirdSessionFirstCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(thirdSessionFirstCourse.booking.bookings, Is.Null);     // Ensure booking details are gone

            var fourthSessionFirstCourse = firstCourse.sessions[3];
            Assert.That(fourthSessionFirstCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(fourthSessionFirstCourse.booking.bookings, Is.Null);     // Ensure booking details are gone
        }


        private string BuildOnlineBookingSearchUrl(string startDate, string endDate, Guid? coachId, Guid? locationId, Guid? serviceId)
        {
            var baseSearchUrl = string.Format("{0}/OnlineBooking/Sessions?startDate={1}&endDate={2}", BaseUrl, startDate, endDate);
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
