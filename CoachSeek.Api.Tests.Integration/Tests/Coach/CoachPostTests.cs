using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Coach
{
    [TestFixture]
    public class CoachPostTests : WebIntegrationTest
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

        [Test]
        public void GivenValidNewCoach_WhenPost_ThenReturnNewCoachResponse()
        {
            var command = GivenValidNewCoach();
            var response = WhenPost(command);
            ThenReturnNewCoachResponse(response);
        }


        private string GivenNoCoachSaveCommand()
        {
            return "";
        }

        private string GivenEmptyCoachSaveCommand()
        {
            return "{}";
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
                firstName = AARON_FIRST_NAME,
                lastName = SMITH_LAST_NAME,
                email = RandomEmail,
                phone = RandomString,
                workingHours = SetupStandardWorkingHours()
            };

            return JsonConvert.SerializeObject(coach);
        }

        private string GivenValidNewCoach()
        {
            var coach = new ApiCoachSaveCommand
            {
                firstName = "Carl",
                lastName = "Carson",
                email = "Carl@CoachMaster.com",
                phone = "021 69 69 69",
                workingHours = SetupStandardWorkingHours()
            };

            coach.workingHours.sunday = new ApiDailyWorkingHours(false, "10:30", "15:45");

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
            Assert.That(errors.GetLength(0), Is.EqualTo(5));
            AssertApplicationError(errors[0], "coach.firstName", "The firstName field is required.");
            AssertApplicationError(errors[1], "coach.lastName", "The lastName field is required.");
            AssertApplicationError(errors[2], "coach.email", "The email field is required.");
            AssertApplicationError(errors[3], "coach.phone", "The phone field is required.");
            AssertApplicationError(errors[4], "coach.workingHours", "The workingHours field is required.");
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

        private void ThenReturnNewCoachResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<CoachData>());
            var coach = (CoachData)response.Payload;

            Assert.That(coach.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(coach.firstName, Is.EqualTo("Carl"));
            Assert.That(coach.lastName, Is.EqualTo("Carson"));
            Assert.That(coach.email, Is.EqualTo("carl@coachmaster.com"));
            Assert.That(coach.phone, Is.EqualTo("021 69 69 69"));

            Assert.That(coach.workingHours, Is.Not.Null);
            AssertStandardWorkingDay(coach.workingHours.monday);
            AssertStandardWorkingDay(coach.workingHours.tuesday);
            AssertStandardWorkingDay(coach.workingHours.wednesday);
            AssertStandardWorkingDay(coach.workingHours.thursday);
            AssertStandardWorkingDay(coach.workingHours.friday);
            AssertStandardWeekendDay(coach.workingHours.saturday);
            AssertWorkingHours(coach.workingHours.sunday, false, "10:30", "15:45");
        }

        private void AssertStandardWorkingDay(DailyWorkingHoursData workingDay)
        {
            Assert.That(workingDay, Is.Not.Null);
            Assert.That(workingDay.isAvailable, Is.True);
            Assert.That(workingDay.startTime, Is.EqualTo("9:00"));
            Assert.That(workingDay.finishTime, Is.EqualTo("17:00"));
        }

        private void AssertStandardWeekendDay(DailyWorkingHoursData workingDay)
        {
            Assert.That(workingDay, Is.Not.Null);
            Assert.That(workingDay.isAvailable, Is.False);
            Assert.That(workingDay.startTime, Is.Null);
            Assert.That(workingDay.finishTime, Is.Null);
        }

        private void AssertWorkingHours(DailyWorkingHoursData workingDay, bool isAvailable, string startTime, string finishTime)
        {
            Assert.That(workingDay, Is.Not.Null);
            Assert.That(workingDay.isAvailable, Is.EqualTo(isAvailable));
            Assert.That(workingDay.startTime, Is.EqualTo(startTime));
            Assert.That(workingDay.finishTime, Is.EqualTo(finishTime));
        }
    }
}
