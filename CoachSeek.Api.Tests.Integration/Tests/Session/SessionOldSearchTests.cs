using System.Collections.Generic;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class SessionOldSearchTests : BaseSessionSearchTests
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
        public void GivenNoSessionInSearchPeriod_WhenTrySearch_ThenReturnNoSession()
        {
            var criteria = GivenNoSessionInSearchPeriod();
            var response = WhenTrySearch(criteria);
            ThenReturnNoSession(response);
        }

        [Test]
        public void GivenInvalidCoachId_WhenTrySearch_ThenReturnInvalidCoachIdError()
        {
            var criteria = GivenInvalidCoachId();
            var response = WhenTrySearch(criteria);
            ThenReturnInvalidCoachIdError(response);
        }

        [Test]
        public void GivenValidCoachId_WhenTrySearch_ThenReturnSessionsForCoach()
        {
            var criteria = GivenValidCoachId();
            var response = WhenTrySearch(criteria);
            ThenReturnSessionsForCoach(response);
        }

        [Test]
        public void GivenInvalidLocationId_WhenTrySearch_ThenReturnInvalidLocationIdError()
        {
            var criteria = GivenInvalidLocationId();
            var response = WhenTrySearch(criteria);
            ThenReturnInvalidLocationIdError(response);
        }

        [Test]
        public void GivenValidLocationId_WhenTrySearch_ThenReturnSessionsForLocation()
        {
            var criteria = GivenValidLocationId();
            var response = WhenTrySearch(criteria);
            ThenReturnSessionsForLocation(response);
        }

        [Test]
        public void GivenInvalidServiceId_WhenTrySearch_ThenReturnInvalidServiceIdError()
        {
            var criteria = GivenInvalidServiceId();
            var response = WhenTrySearch(criteria);
            ThenReturnInvalidServiceIdError(response);
        }

        [Test]
        public void GivenValidServiceId_WhenTrySearch_ThenReturnSessionsForService()
        {
            var criteria = GivenValidServiceId();
            var response = WhenTrySearch(criteria);
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
            Assert.That(thirdSession.parentId, Is.EqualTo(AaronRemuera9To10For4WeeksCourseId));
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
            Assert.That(fifthSession.parentId, Is.EqualTo(AaronRemuera9To10For4WeeksCourseId));
            Assert.That(fifthSession.coach.id, Is.EqualTo(Aaron.Id));
            Assert.That(fifthSession.coach.name, Is.EqualTo(Aaron.Name));
            Assert.That(fifthSession.timing.startTime, Is.EqualTo("9:00"));

            var sixthSession = sessions[5];
            Assert.That(sixthSession.parentId, Is.EqualTo(AaronRemuera9To10For4WeeksCourseId));
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

}
