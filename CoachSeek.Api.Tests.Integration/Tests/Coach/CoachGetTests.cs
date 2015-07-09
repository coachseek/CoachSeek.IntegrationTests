using System;
using System.Collections.Generic;
using System.Net;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Coach
{
    [TestFixture]
    public class CoachGetTests : CoachTests
    {
        [Test]
        public void WhenTryGetAllCoaches_ThenReturnAllCoaches()
        {
            var setup = RegisterBusiness();
            RegisterTestCoaches(setup);

            var response = WhenTryGetAllCoaches(setup);
            ThenReturnAllCoaches(response, setup);
        }

        [Test]
        public void GivenInvalidCoachId_WhenTryGetCoachById_ThenReturnNotFoundResponse()
        {
            var setup = RegisterBusiness();

            var id = GivenInvalidCoachId();
            var response = WhenTryGetCoachById(id, setup);
            AssertNotFound(response);
        }

        [Test]
        public void GivenValidCoachId_WhenTryGetCoachById_ThenReturnCoach()
        {
            var setup = RegisterBusiness();
            RegisterCoachAaron(setup);

            var id = GivenValidCoachId(setup);
            var response = WhenTryGetCoachById(id, setup);
            ThenReturnCoach(response, setup);
        }



        private Guid GivenInvalidCoachId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidCoachId(SetupData setup)
        {
            return setup.Aaron.Id;
        }


        private ApiResponse WhenTryGetAllCoaches(SetupData setup)
        {
            return new TestAuthenticatedApiClient().Get<List<CoachData>>(setup.Business.UserName,
                                                                         setup.Business.Password,
                                                                         RelativePath);
        }

        private ApiResponse WhenTryGetCoachById(Guid coachId, SetupData setup)
        {
            var url = string.Format("{0}/{1}", RelativePath, coachId);
            return new TestAuthenticatedApiClient().Get<CoachData>(setup.Business.UserName,
                                                                   setup.Business.Password,
                                                                   url);
        }


        private void ThenReturnAllCoaches(ApiResponse response, SetupData setup)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Payload, Is.Not.Null);
            var coaches = (List<CoachData>)response.Payload;
            Assert.That(coaches.Count, Is.EqualTo(2));

            var coachOne = coaches[0];
            Assert.That(coachOne.id, Is.EqualTo(setup.Aaron.Id));
            Assert.That(coachOne.firstName, Is.EqualTo(setup.Aaron.FirstName));
            Assert.That(coachOne.lastName, Is.EqualTo(setup.Aaron.LastName));
            Assert.That(coachOne.email, Is.EqualTo(setup.Aaron.Email));
            Assert.That(coachOne.phone, Is.EqualTo(setup.Aaron.Phone.ToUpper()));
            
            var coachTwo = coaches[1];
            Assert.That(coachTwo.id, Is.EqualTo(setup.Bobby.Id));
            Assert.That(coachTwo.firstName, Is.EqualTo(setup.Bobby.FirstName));
            Assert.That(coachTwo.lastName, Is.EqualTo(setup.Bobby.LastName));
            Assert.That(coachTwo.email, Is.EqualTo(setup.Bobby.Email));
            Assert.That(coachTwo.phone, Is.EqualTo(setup.Bobby.Phone.ToUpper()));
        }

        private void ThenReturnCoach(ApiResponse response, SetupData setup)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var coach = (CoachData)response.Payload;

            Assert.That(coach.id, Is.EqualTo(setup.Aaron.Id));
            Assert.That(coach.firstName, Is.EqualTo(setup.Aaron.FirstName));
            Assert.That(coach.lastName, Is.EqualTo(setup.Aaron.LastName));
            Assert.That(coach.email, Is.EqualTo(setup.Aaron.Email));
            Assert.That(coach.phone, Is.EqualTo(setup.Aaron.Phone.ToUpperInvariant()));
            AssertStandardWorkingHours(coach);
        }

        protected void AssertStandardWorkingHours(CoachData coach)
        {
            Assert.That(coach.workingHours, Is.Not.Null);
            AssertWorkingDay(coach.workingHours.monday);
            AssertWorkingDay(coach.workingHours.tuesday);
            AssertWorkingDay(coach.workingHours.wednesday);
            AssertWorkingDay(coach.workingHours.thursday);
            AssertWorkingDay(coach.workingHours.friday);
            AssertNonWorkingDay(coach.workingHours.saturday);
            AssertNonWorkingDay(coach.workingHours.sunday);
        }

        private void AssertWorkingDay(DailyWorkingHoursData day)
        {
            Assert.That(day.isAvailable, Is.True);
            Assert.That(day.startTime, Is.EqualTo("9:00"));
            Assert.That(day.finishTime, Is.EqualTo("17:00"));
        }

        private void AssertNonWorkingDay(DailyWorkingHoursData day)
        {
            Assert.That(day.isAvailable, Is.False);
            Assert.That(day.startTime, Is.Null);
            Assert.That(day.finishTime, Is.Null);
        }
    }
}
