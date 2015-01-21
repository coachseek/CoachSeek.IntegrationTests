using System;
using System.Collections.Generic;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Coach
{
    [TestFixture]
    public class CoachGetTests : WebIntegrationTest
    {
        private const string AARON_FIRST_NAME = "Aaron";
        private const string BOBBY_FIRST_NAME = "Bobby";
        private const string SMITH_LAST_NAME = "Smith";

        private Guid AaronId { get; set; }
        private Guid BobbyId { get; set; }
        private string AaronEmail { get; set; }
        private string AaronPhone { get; set; }

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
            RegisterAaronCoach();
            RegisterBobbyLocation();
        }

        private void RegisterAaronCoach()
        {
            AaronEmail = RandomEmail;
            AaronPhone = RandomString;
            var json = CreateNewCoachSaveCommand(AARON_FIRST_NAME, SMITH_LAST_NAME, AaronEmail, AaronPhone);
            var response = Post<CoachData>(json);
            AaronId = ((CoachData)response.Payload).id;
        }

        private void RegisterBobbyLocation()
        {
            var json = CreateNewCoachSaveCommand(BOBBY_FIRST_NAME, SMITH_LAST_NAME, RandomEmail, RandomString);
            var response = Post<CoachData>(json);
            BobbyId = ((CoachData)response.Payload).id;
        }

        private string CreateNewCoachSaveCommand(string firstName, string lastName, string email, string phone)
        {
            var coach = new ApiCoachSaveCommand
            {
                firstName = firstName,
                lastName = lastName,
                email = email,
                phone = phone,
                workingHours = SetupStandardWorkingHours()
            };

            return JsonConvert.SerializeObject(coach);
        }

        private ApiWeeklyWorkingHours SetupStandardWorkingHours()
        {
            return new ApiWeeklyWorkingHours
            {
                monday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                tuesday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                wednesday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                thursday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                friday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                saturday = new ApiDailyWorkingHours(),
                sunday = new ApiDailyWorkingHours()
            };
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
            ThenReturnNotFoundResponse(response);
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
            return AaronId;
        }


        private Response WhenGetAll()
        {
            var url = BuildGetAllUrl();
            return Get<List<CoachData>>(url);
        }

        private Response WhenGetById(Guid coachId)
        {
            var url = BuildGetByIdUrl(coachId);
            return Get<CoachData>(url);
        }


        private void ThenReturnAllCoachesResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Payload, Is.Not.Null);
            var coaches = (List<CoachData>)response.Payload;
            Assert.That(coaches.Count, Is.EqualTo(2));

            var coachOne = coaches[0];
            Assert.That(coachOne.id, Is.EqualTo(AaronId));
            Assert.That(coachOne.firstName, Is.EqualTo(AARON_FIRST_NAME));
            Assert.That(coachOne.lastName, Is.EqualTo(SMITH_LAST_NAME));
            
            var coachTwo = coaches[1];
            Assert.That(coachTwo.id, Is.EqualTo(BobbyId));
            Assert.That(coachTwo.firstName, Is.EqualTo(BOBBY_FIRST_NAME));
            Assert.That(coachTwo.lastName, Is.EqualTo(SMITH_LAST_NAME));
        }

        private void ThenReturnNotFoundResponse(Response response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        private void ThenReturnCoachResponse(Response response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var coach = (CoachData)response.Payload;
            Assert.That(coach.id, Is.EqualTo(AaronId));
            Assert.That(coach.firstName, Is.EqualTo(AARON_FIRST_NAME));
            Assert.That(coach.lastName, Is.EqualTo(SMITH_LAST_NAME));
            Assert.That(coach.email, Is.EqualTo(AaronEmail));
            Assert.That(coach.phone, Is.EqualTo(AaronPhone.ToUpperInvariant()));
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
