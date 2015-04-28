using System;
using System.Collections.Generic;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Coach;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Coach
{
    [TestFixture]
    public class CoachGetTests : WebIntegrationTest
    {
        protected override string RelativePath
        {
            get { return "Coaches"; }
        }

        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            RegisterTestCoaches();
        }

        private void RegisterTestCoaches()
        {
            Steve = new CoachSteve();
            CoachRegistrar.RegisterCoach(Steve, Business);

            Aaron = new CoachAaron();
            CoachRegistrar.RegisterCoach(Aaron, Business);
            
            Bobby = new CoachBobby();
            CoachRegistrar.RegisterCoach(Bobby, Business);
        }


        [Test]
        public void WhenGetAll_ThenReturnAllCoachesResponse()
        {
            var response = WhenGetAll();
            ThenReturnAllCoachesResponse(response);
        }

        [Test]
        public void GivenInvalidCoachId_WhenGetById_ThenReturnNotFoundResponse()
        {
            var id = GivenInvalidCoachId();
            var response = WhenGetById(id);
            AssertNotFound(response);
        }

        [Test]
        public void GivenValidCoachId_WhenGetById_ThenReturnCoachResponse()
        {
            var id = GivenValidCoachId();
            var response = WhenGetById(id);
            ThenReturnCoachResponse(response);
        }



        private Guid GivenInvalidCoachId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidCoachId()
        {
            return Aaron.Id;
        }


        private Response WhenGetAll()
        {
            var url = BuildGetAllUrl();
            return AuthenticatedGet<List<CoachData>>(url);
        }

        private Response WhenGetById(Guid coachId)
        {
            var url = BuildGetByIdUrl(coachId);
            return AuthenticatedGet<CoachData>(url);
        }


        private void ThenReturnAllCoachesResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Payload, Is.Not.Null);
            var coaches = (List<CoachData>)response.Payload;
            Assert.That(coaches.Count, Is.EqualTo(3));

            var coachOne = coaches[0];
            Assert.That(coachOne.id, Is.EqualTo(Aaron.Id));
            Assert.That(coachOne.firstName, Is.EqualTo(Aaron.FirstName));
            Assert.That(coachOne.lastName, Is.EqualTo(Aaron.LastName));
            Assert.That(coachOne.email, Is.EqualTo(Aaron.Email));
            Assert.That(coachOne.phone, Is.EqualTo(Aaron.Phone.ToUpper()));
            
            var coachTwo = coaches[1];
            Assert.That(coachTwo.id, Is.EqualTo(Bobby.Id));
            Assert.That(coachTwo.firstName, Is.EqualTo(Bobby.FirstName));
            Assert.That(coachTwo.lastName, Is.EqualTo(Bobby.LastName));
            Assert.That(coachTwo.email, Is.EqualTo(Bobby.Email));
            Assert.That(coachTwo.phone, Is.EqualTo(Bobby.Phone.ToUpper()));

            var coachThree = coaches[2];
            Assert.That(coachThree.id, Is.EqualTo(Steve.Id));
            Assert.That(coachThree.firstName, Is.EqualTo(Steve.FirstName));
            Assert.That(coachThree.lastName, Is.EqualTo(Steve.LastName));
            Assert.That(coachThree.email, Is.EqualTo(Steve.Email));
            Assert.That(coachThree.phone, Is.EqualTo(Steve.Phone.ToUpper()));
        }

        private void ThenReturnCoachResponse(Response response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var coach = (CoachData)response.Payload;
            Assert.That(coach.id, Is.EqualTo(Aaron.Id));
            Assert.That(coach.firstName, Is.EqualTo(Aaron.FirstName));
            Assert.That(coach.lastName, Is.EqualTo(Aaron.LastName));
            Assert.That(coach.email, Is.EqualTo(Aaron.Email));
            Assert.That(coach.phone, Is.EqualTo(Aaron.Phone.ToUpperInvariant()));
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
