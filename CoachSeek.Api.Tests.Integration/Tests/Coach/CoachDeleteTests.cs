using System;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Coach
{
    [TestFixture]
    public class CoachDeleteTests : WebIntegrationTest
    {
        private const string AARON_FIRST_NAME = "Aaron";
        private const string BOBBY_FIRST_NAME = "Bobby";
        private const string STEVE_FIRST_NAME = "Steve";
        private const string FERGUSSON_LAST_NAME = "Fergusson";
        private const string SMITH_LAST_NAME = "Smith";

        private Guid AaronId { get; set; }
        private Guid BobbyId { get; set; }
        private Guid SteveId { get; set; }
        private string AaronEmail { get; set; }
        private string AaronPhone { get; set; }
        private string BobbyEmail { get; set; }
        private string BobbyPhone { get; set; }
        private string SteveEmail { get; set; }
        private string StevePhone { get; set; }

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
            RegisterSteveFergussonCoach();
            RegisterAaronSmithCoach();
            RegisterBobbySmithCoach();
        }

        private void RegisterSteveFergussonCoach()
        {
            SteveEmail = Random.RandomEmail;
            StevePhone = Random.RandomString;
            var json = CreateNewCoachSaveCommand(STEVE_FIRST_NAME, FERGUSSON_LAST_NAME, SteveEmail, StevePhone);
            var response = Post<CoachData>(json);
            SteveId = ((CoachData)response.Payload).id;
        }

        private void RegisterAaronSmithCoach()
        {
            AaronEmail = Random.RandomEmail;
            AaronPhone = Random.RandomString;
            var json = CreateNewCoachSaveCommand(AARON_FIRST_NAME, SMITH_LAST_NAME, AaronEmail, AaronPhone);
            var response = Post<CoachData>(json);
            AaronId = ((CoachData)response.Payload).id;
        }

        private void RegisterBobbySmithCoach()
        {
            BobbyEmail = Random.RandomEmail;
            BobbyPhone = Random.RandomString;
            var json = CreateNewCoachSaveCommand(BOBBY_FIRST_NAME, SMITH_LAST_NAME, BobbyEmail, BobbyPhone);
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
        public void GivenNonExistentCoachId_WhenTryDelete_ThenReturnNotFoundErrorResponse()
        {
            var id = GivenNonExistentCoachId();
            var response = WhenTryDelete(id);
            AssertNotFound(response);
        }


        private Guid GivenNonExistentCoachId()
        {
            return Guid.NewGuid();
        }


        private Response WhenTryDelete(Guid id)
        {
            return Delete<CoachData>("Coaches", id);
        }
    }
}
