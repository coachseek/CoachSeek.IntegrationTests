using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    [TestFixture]
    public class CoachTests : WebIntegrationTest
    {
        private const string AARON_FIRST_NAME = "Aaron";
        private const string BOBBY_FIRST_NAME = "Bobby";
        private const string SMITH_LAST_NAME = "Smith";

        private Guid AaronId { get; set; }
        private Guid BobbyId { get; set; }


        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            RegisterTestCoaches();

            //NewLocationName = string.Empty;
        }

        private void RegisterTestCoaches()
        {
            RegisterAaronCoach();
            RegisterBobbyLocation();
        }

        private void RegisterAaronCoach()
        {
            var json = CreateNewCoachSaveCommand(AARON_FIRST_NAME, SMITH_LAST_NAME);
            var response = Post<CoachData>(json);
            AaronId = ((CoachData)response.Payload).id;
        }

        private void RegisterBobbyLocation()
        {
            var json = CreateNewCoachSaveCommand(BOBBY_FIRST_NAME, SMITH_LAST_NAME);
            var response = Post<CoachData>(json);
            BobbyId = ((CoachData)response.Payload).id;
        }

        private string CreateNewCoachSaveCommand(string firstName, string lastName)
        {
            var coach = new ApiCoachSaveCommand
            {
                businessId = BusinessId,
                firstName = firstName,
                lastName = lastName,
                email = RandomEmail,
                phone = RandomString,
                workingHours = SetupStandardWorkingHours()
            };

            return JsonConvert.SerializeObject(coach);
        }

        protected override string RelativePath
        {
            get { return "Coaches"; }
        }


        [Test]
        public void GivenNoCoachSaveCommand_WhenPost_ThenReturnNoDataErrorResponse()
        {
            var command = GivenNoCoachSaveCommand();
            var response = WhenPost(command);
            ThenReturnNoDataErrorResponse(response);
        }

        [Test]
        public void GivenEmptyCoachSaveCommand_WhenPost_ThenReturnRootRequiredErrorResponse()
        {
            var command = GivenEmptyCoachSaveCommand();
            var response = WhenPost(command);
            ThenReturnRootRequiredErrorResponse(response);
        }

        [Test]
        public void GivenNonExistentBusinessId_WhenPost_ThenReturnInvalidBusinessIdErrorResponse()
        {
            var command = GivenNonExistentBusinessId();
            var response = WhenPost(command);
            ThenReturnInvalidBusinessIdErrorResponse(response);
        }

        [Test]
        public void GivenMissingWorkingHoursProperties_WhenPost_ThenReturnMissingWorkingHoursPropertyErrorResponse()
        {
            var command = GivenMissingWorkingHoursProperties();
            var response = WhenPost(command);
            ThenReturnMissingWorkingHoursPropertyErrorResponse(response);
        }

        [Test]
        public void GivenInvalidWorkingHoursProperties_WhenPost_ThenReturnInvalidWorkingHoursPropertyErrorResponse()
        {
            var command = GivenInvalidWorkingHoursProperties();
            var response = WhenPost(command);
            ThenReturnInvalidWorkingHoursPropertyErrorResponse(response);
        }

        [Test]
        public void GivenNonExistentCoachId_WhenPost_ThenReturnInvalidCoachIdErrorResponse()
        {
            var command = GivenNonExistentCoachId();
            var response = WhenPost(command);
            ThenReturnInvalidCoachIdErrorResponse(response);
        }

        [Test]
        public void GivenNewCoachWithAnAlreadyExistingCoachName_WhenPost_ThenReturnDuplicateCoachErrorResponse()
        {
            var command = GivenNewCoachWithAnAlreadyExistingCoachName();
            var response = WhenPost(command);
            ThenReturnDuplicateCoachErrorResponse(response);
        }


        private string GivenNoCoachSaveCommand()
        {
            return "";
        }

        private string GivenEmptyCoachSaveCommand()
        {
            return "{}";
        }

        private string GivenNonExistentBusinessId()
        {
            var coach = new ApiCoachSaveCommand
            {
                businessId = Guid.Empty,
                firstName = RandomString,
                lastName = RandomString,
                email = RandomEmail,
                phone = RandomString,
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

        private string GivenMissingWorkingHoursProperties()
        {
            var coach = new ApiCoachSaveCommand
            {
                businessId = BusinessId,
                firstName = RandomString,
                lastName = RandomString,
                email = RandomEmail,
                phone = RandomString,
                workingHours = new ApiWeeklyWorkingHours
                {
                    monday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                    thursday = new ApiDailyWorkingHours(),
                    friday = new ApiDailyWorkingHours(true, "10:00", "18:00"),
                    saturday = new ApiDailyWorkingHours()
                }
            };

            return JsonConvert.SerializeObject(coach);
        }

        private string GivenInvalidWorkingHoursProperties()
        {
            var coach = new ApiCoachSaveCommand
            {
                businessId = BusinessId,
                firstName = RandomString,
                lastName = RandomString,
                email = RandomEmail,
                phone = RandomString,
                workingHours = new ApiWeeklyWorkingHours
                {
                    monday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                    tuesday = new ApiDailyWorkingHours(),
                    wednesday = new ApiDailyWorkingHours(true, "-4:17", "23:77"),
                    thursday = new ApiDailyWorkingHours(),
                    friday = new ApiDailyWorkingHours(true, "hello", "world"),
                    saturday = new ApiDailyWorkingHours(),
                    sunday = new ApiDailyWorkingHours()
                }
            };

            return JsonConvert.SerializeObject(coach);
        }

        private string GivenNonExistentCoachId()
        {
            var coach = new ApiCoachSaveCommand
            {
                businessId = BusinessId,
                id = Guid.Empty,
                firstName = RandomString,
                lastName = RandomString,
                email = RandomEmail,
                phone = RandomString,
                workingHours = SetupStandardWorkingHours()
            };

            return JsonConvert.SerializeObject(coach);
        }

        private string GivenNewCoachWithAnAlreadyExistingCoachName()
        {
            var coach = new ApiCoachSaveCommand
            {
                businessId = BusinessId,
                firstName = AARON_FIRST_NAME,
                lastName = SMITH_LAST_NAME,
                email = RandomEmail,
                phone = RandomString,
                workingHours = SetupStandardWorkingHours()
            };

            return JsonConvert.SerializeObject(coach);
        }


        private Response WhenPost(string json)
        {
            return Post<CoachData>(json);
        }


        private void ThenReturnNoDataErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], null, "Please post us some data!");
        }

        private void ThenReturnRootRequiredErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(6));
            AssertApplicationError(errors[0], "coach.businessId", "The businessId field is required.");
            AssertApplicationError(errors[1], "coach.firstName", "The firstName field is required.");
            AssertApplicationError(errors[2], "coach.lastName", "The lastName field is required.");
            AssertApplicationError(errors[3], "coach.email", "The email field is required.");
            AssertApplicationError(errors[4], "coach.phone", "The phone field is required.");
            AssertApplicationError(errors[5], "coach.workingHours", "The workingHours field is required.");
        }

        private void ThenReturnInvalidBusinessIdErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "coach.businessId", "This business does not exist.");
        }

        private void ThenReturnMissingWorkingHoursPropertyErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;

            Assert.That(errors.GetLength(0), Is.EqualTo(3));
            AssertApplicationError(errors[0], "coach.workingHours.tuesday", "The tuesday field is required.");
            AssertApplicationError(errors[1], "coach.workingHours.wednesday", "The wednesday field is required.");
            AssertApplicationError(errors[2], "coach.workingHours.sunday", "The sunday field is required.");
        }

        private void ThenReturnInvalidWorkingHoursPropertyErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;

            Assert.That(errors.GetLength(0), Is.EqualTo(2));
            AssertApplicationError(errors[0], "coach.workingHours.wednesday", "The wednesday working hours are not valid.");
            AssertApplicationError(errors[1], "coach.workingHours.friday", "The friday working hours are not valid.");
        }

        private void ThenReturnInvalidCoachIdErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "coach.id", "This coach does not exist.");
        }

        private void ThenReturnDuplicateCoachErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], null, "This coach already exists.");
        }
    }
}
