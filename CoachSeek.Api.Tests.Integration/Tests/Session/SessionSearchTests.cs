using System;
using System.Collections.Generic;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
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

        private void ThenReturnNoSessionOrCourses(Response response)
        {
            var searchResult = AssertSuccessResponse<SessionSearchData>(response);

            var sessions = searchResult.Sessions;
            Assert.That(sessions.Count, Is.EqualTo(0));

            var courses = searchResult.Courses;
            Assert.That(courses.Count, Is.EqualTo(0));
        }


        [TestFixture]
        public class SessionOldSearchTests : SessionSearchTests
        {

            [Test]
            public void GivenInvalidSearchPeriod_WhenSearch_ThenReturnInvalidSearchPeriodError()
            {
                var criteria = GivenInvalidSearchPeriod();
                var response = WhenSearch(criteria);
                ThenReturnInvalidSearchPeriodError(response);
            }

            [Test]
            public void GivenNoSessionInSearchPeriod_WhenSearch_ThenReturnNoSession()
            {
                var criteria = GivenNoSessionInSearchPeriod();
                var response = WhenSearch(criteria);
                ThenReturnNoSession(response);
            }

            [Test]
            public void GivenInvalidCoachId_WhenSearch_ThenReturnInvalidCoachIdError()
            {
                var criteria = GivenInvalidCoachId();
                var response = WhenSearch(criteria);
                ThenReturnInvalidCoachIdError(response);
            }

            [Test]
            public void GivenValidCoachId_WhenSearch_ThenReturnSessionsForCoach()
            {
                var criteria = GivenValidCoachId();
                var response = WhenSearch(criteria);
                ThenReturnSessionsForCoach(response);
            }

            [Test]
            public void GivenInvalidLocationId_WhenSearch_ThenReturnInvalidLocationIdError()
            {
                var criteria = GivenInvalidLocationId();
                var response = WhenSearch(criteria);
                ThenReturnInvalidLocationIdError(response);
            }

            [Test]
            public void GivenValidLocationId_WhenSearch_ThenReturnSessionsForLocation()
            {
                var criteria = GivenValidLocationId();
                var response = WhenSearch(criteria);
                ThenReturnSessionsForLocation(response);
            }

            [Test]
            public void GivenInvalidServiceId_WhenSearch_ThenReturnInvalidServiceIdError()
            {
                var criteria = GivenInvalidServiceId();
                var response = WhenSearch(criteria);
                ThenReturnInvalidServiceIdError(response);
            }

            [Test]
            public void GivenValidServiceId_WhenSearch_ThenReturnSessionsForService()
            {
                var criteria = GivenValidServiceId();
                var response = WhenSearch(criteria);
                ThenReturnSessionsForService(response);
            }


            private void ThenReturnNoSession(Response response)
            {
                var sessions = AssertSuccessResponse<List<SessionData>>(response);

                Assert.That(sessions.Count, Is.EqualTo(0));
            }

            private void ThenReturnSessionsForCoach(Response response)
            {
                var sessions = AssertSuccessResponse<List<SessionData>>(response);

                Assert.That(sessions.Count, Is.EqualTo(7));

                var firstSession = sessions[0];
                Assert.That(firstSession.parentId, Is.EqualTo(AaronOrakeiMiniBlueFor2DaysCourseId));
                Assert.That(firstSession.coach.id, Is.EqualTo(Aaron.Id));
                Assert.That(firstSession.coach.name, Is.EqualTo(Aaron.Name));
                Assert.That(firstSession.timing.startTime, Is.EqualTo("9:00"));

                var secondSession = sessions[1];
                Assert.That(secondSession.parentId, Is.EqualTo(AaronOrakeiMiniBlueFor2DaysCourseId));
                Assert.That(secondSession.coach.id, Is.EqualTo(Aaron.Id));
                Assert.That(secondSession.coach.name, Is.EqualTo(Aaron.Name));
                Assert.That(secondSession.timing.startTime, Is.EqualTo("9:00"));

                var thirdSession = sessions[2];
                Assert.That(thirdSession.parentId, Is.EqualTo(AaronRemuera9To10For5WeeksCourseId));
                Assert.That(thirdSession.coach.id, Is.EqualTo(Aaron.Id));
                Assert.That(thirdSession.coach.name, Is.EqualTo(Aaron.Name));
                Assert.That(thirdSession.timing.startTime, Is.EqualTo("9:00"));

                var fourthSession = sessions[3];
                Assert.That(fourthSession.id, Is.EqualTo(AaronOrakei16To17.Id));
                Assert.That(fourthSession.parentId, Is.Null);
                Assert.That(fourthSession.coach.id, Is.EqualTo(Aaron.Id));
                Assert.That(fourthSession.coach.name, Is.EqualTo(Aaron.Name));
                Assert.That(fourthSession.timing.startTime, Is.EqualTo("16:00"));

                var fifthSession = sessions[4];
                Assert.That(fifthSession.parentId, Is.EqualTo(AaronRemuera9To10For5WeeksCourseId));
                Assert.That(fifthSession.coach.id, Is.EqualTo(Aaron.Id));
                Assert.That(fifthSession.coach.name, Is.EqualTo(Aaron.Name));
                Assert.That(fifthSession.timing.startTime, Is.EqualTo("9:00"));

                var sixthSession = sessions[5];
                Assert.That(sixthSession.parentId, Is.EqualTo(AaronRemuera9To10For5WeeksCourseId));
                Assert.That(sixthSession.coach.id, Is.EqualTo(Aaron.Id));
                Assert.That(sixthSession.coach.name, Is.EqualTo(Aaron.Name));
                Assert.That(sixthSession.timing.startTime, Is.EqualTo("9:00"));

                var seventhSession = sessions[6];
                Assert.That(seventhSession.id, Is.EqualTo(AaronOrakei14To15.Id));
                Assert.That(seventhSession.parentId, Is.Null);
                Assert.That(seventhSession.coach.id, Is.EqualTo(Aaron.Id));
                Assert.That(seventhSession.coach.name, Is.EqualTo(Aaron.Name));
                Assert.That(seventhSession.timing.startTime, Is.EqualTo("14:00"));
                Assert.That(seventhSession.booking.bookings.Count, Is.EqualTo(2));
                var bookingOne = seventhSession.booking.bookings[0];
                Assert.That(bookingOne.id, Is.EqualTo(FredOnAaronOrakei14To15SessionId));
                Assert.That(bookingOne.customer.id, Is.EqualTo(Fred.Id));
                Assert.That(bookingOne.customer.firstName, Is.EqualTo(Fred.FirstName));
                Assert.That(bookingOne.customer.lastName, Is.EqualTo(Fred.LastName));
                Assert.That(bookingOne.customer.email, Is.EqualTo(Fred.Email));
                Assert.That(bookingOne.customer.phone, Is.EqualTo(Fred.Phone.ToUpper()));
                var bookingTwo = seventhSession.booking.bookings[1];
                Assert.That(bookingTwo.id, Is.EqualTo(BarneyOnAaronOrakei14To15SessionId));
                Assert.That(bookingTwo.customer.id, Is.EqualTo(Barney.Id));
                Assert.That(bookingTwo.customer.firstName, Is.EqualTo(Barney.FirstName));
                Assert.That(bookingTwo.customer.lastName, Is.EqualTo(Barney.LastName));
                Assert.That(bookingTwo.customer.email, Is.Null);
                Assert.That(bookingTwo.customer.phone, Is.Null);
            }

            private void ThenReturnSessionsForLocation(Response response)
            {
                var sessions = AssertSuccessResponse<List<SessionData>>(response);

                Assert.That(sessions.Count, Is.EqualTo(4));
                foreach (var session in sessions)
                    Assert.That(session.location.id, Is.EqualTo(Orakei.Id));
            }

            private void ThenReturnSessionsForService(Response response)
            {
                var sessions = AssertSuccessResponse<List<SessionData>>(response);

                Assert.That(sessions.Count, Is.EqualTo(5));
                foreach (var session in sessions)
                    Assert.That(session.service.id, Is.EqualTo(MiniRed.Id));
            }
        }


        [TestFixture]
        public class OnlineBookingSessionSearchTests : SessionSearchTests
        {
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
                Assert.That(secondCourse.id, Is.EqualTo(AaronRemuera9To10For5WeeksCourseId));
                Assert.That(secondCourse.parentId, Is.Null);
                Assert.That(secondCourse.coach.id, Is.EqualTo(Aaron.Id));
                Assert.That(secondCourse.coach.name, Is.EqualTo(Aaron.Name));
                Assert.That(firstCourse.timing.startDate, Is.EqualTo(GetDateFormatNumberOfDaysOut(1)));
                Assert.That(secondCourse.timing.startTime, Is.EqualTo("9:00"));
                Assert.That(secondCourse.booking.bookingCount, Is.EqualTo(0));
                Assert.That(secondCourse.booking.bookings, Is.Null);     // Ensure booking details are gone
                Assert.That(secondCourse.sessions.Count, Is.EqualTo(5));

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

                var fifthSessionSecondCourse = secondCourse.sessions[4];
                Assert.That(fifthSessionSecondCourse.booking.bookingCount, Is.EqualTo(0));
                Assert.That(fifthSessionSecondCourse.booking.bookings, Is.Null);     // Ensure booking details are gone
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
                Assert.That(firstCourse.id, Is.EqualTo(AaronRemuera9To10For5WeeksCourseId));
                Assert.That(firstCourse.parentId, Is.Null);
                Assert.That(firstCourse.coach.id, Is.EqualTo(Aaron.Id));
                Assert.That(firstCourse.coach.name, Is.EqualTo(Aaron.Name));
                Assert.That(firstCourse.timing.startDate, Is.EqualTo(GetDateFormatNumberOfWeeksOut(1)));
                Assert.That(firstCourse.timing.startTime, Is.EqualTo("9:00"));
                Assert.That(firstCourse.booking.bookingCount, Is.EqualTo(0));
                Assert.That(firstCourse.booking.bookings, Is.Null);     // Ensure booking details are gone
                Assert.That(firstCourse.sessions.Count, Is.EqualTo(5));

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

                var fifthSessionFirstCourse = firstCourse.sessions[4];
                Assert.That(fifthSessionFirstCourse.booking.bookingCount, Is.EqualTo(0));
                Assert.That(fifthSessionFirstCourse.booking.bookings, Is.Null);     // Ensure booking details are gone
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


        [TestFixture]
        public class SessionNewSearchTests : SessionSearchTests
        {
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

                return Get<SessionSearchData>(url);
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
                Assert.That(secondSessionFirstCourse.booking.bookingCount, Is.EqualTo(1));
                Assert.That(secondSessionFirstCourse.booking.bookings.Count, Is.EqualTo(1));

                var secondCourse = courses[1];
                Assert.That(secondCourse.id, Is.EqualTo(AaronRemuera9To10For5WeeksCourseId));
                Assert.That(secondCourse.parentId, Is.Null);
                Assert.That(secondCourse.coach.id, Is.EqualTo(Aaron.Id));
                Assert.That(secondCourse.coach.name, Is.EqualTo(Aaron.Name));
                Assert.That(firstCourse.timing.startDate, Is.EqualTo(GetDateFormatNumberOfDaysOut(1)));
                Assert.That(secondCourse.timing.startTime, Is.EqualTo("9:00"));
                Assert.That(secondCourse.booking.bookingCount, Is.EqualTo(0));
                Assert.That(secondCourse.booking.bookings.Count, Is.EqualTo(0));
                Assert.That(secondCourse.sessions.Count, Is.EqualTo(5));

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

                var fifthSessionSecondCourse = secondCourse.sessions[4];
                Assert.That(fifthSessionSecondCourse.booking.bookingCount, Is.EqualTo(0));
                Assert.That(fifthSessionSecondCourse.booking.bookings.Count, Is.EqualTo(0));
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
                Assert.That(secondSessionFirstCourse.booking.bookingCount, Is.EqualTo(1));
                Assert.That(secondSessionFirstCourse.booking.bookings.Count, Is.EqualTo(1));
            }
        }

        private void ThenReturnSessionsAndCoursesForService(Response response)
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
            Assert.That(firstCourse.id, Is.EqualTo(AaronRemuera9To10For5WeeksCourseId));
            Assert.That(firstCourse.parentId, Is.Null);
            Assert.That(firstCourse.coach.id, Is.EqualTo(Aaron.Id));
            Assert.That(firstCourse.coach.name, Is.EqualTo(Aaron.Name));
            Assert.That(firstCourse.timing.startDate, Is.EqualTo(GetDateFormatNumberOfWeeksOut(1)));
            Assert.That(firstCourse.timing.startTime, Is.EqualTo("9:00"));
            Assert.That(firstCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(firstCourse.booking.bookings.Count, Is.EqualTo(0));
            Assert.That(firstCourse.sessions.Count, Is.EqualTo(5));

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

            var fifthSessionFirstCourse = firstCourse.sessions[4];
            Assert.That(fifthSessionFirstCourse.booking.bookingCount, Is.EqualTo(0));
            Assert.That(fifthSessionFirstCourse.booking.bookings.Count, Is.EqualTo(0));
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
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(), GetFormattedDateThreeWeeksOut(), Aaron.Id, null, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidLocationId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", null, Guid.NewGuid(), null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenValidLocationId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(), GetFormattedDateThreeWeeksOut(), null, Orakei.Id, null);
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenInvalidServiceId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>("2015-01-01", "2015-01-02", null, null, Guid.NewGuid());
        }

        private Tuple<string, string, Guid?, Guid?, Guid?> GivenValidServiceId()
        {
            return new Tuple<string, string, Guid?, Guid?, Guid?>(GetFormattedDateToday(), GetFormattedDateThreeWeeksOut(), null, null, MiniRed.Id);
        }


        private Response WhenSearch(Tuple<string, string, Guid?, Guid?, Guid?> criteria)
        {
            var url = BuildSearchUrl(criteria.Item1, criteria.Item2, criteria.Item3, criteria.Item4, criteria.Item5);

            return Get<List<SessionData>>(url);
        }
         

        private void ThenReturnInvalidSearchPeriodError(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "The startDate is not a valid date.", "startDate" },
                                                    { "The endDate is not a valid date.", "endDate" } });
        }

        private void ThenReturnInvalidCoachIdError(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "Not a valid coachId.", "coachId" } });
        }

        private void ThenReturnInvalidLocationIdError(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "Not a valid locationId.", "locationId" } });
        }

        private void ThenReturnInvalidServiceIdError(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "Not a valid serviceId.", "serviceId" } });
        }
    }
}
