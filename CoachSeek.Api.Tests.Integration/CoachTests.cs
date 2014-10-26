using System;
using System.Net;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration
{
    [TestFixture]
    public class CoachTests : WebIntegrationTest
    {
        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            //RegisterTestCoaches();

            //NewLocationName = string.Empty;
        }

        //private void RegisterTestCoaches()
        //{
        //    RegisterOrakeiLocation();
        //    RegisterRemueraLocation();
        //}

        //private void RegisterOrakeiLocation()
        //{
        //    var json = CreateNewLocationSaveCommand(ORAKEI_NAME);
        //    var response = Post<LocationData>(json);
        //    OrakeiId = ((LocationData)response.Payload).id;
        //}

        //private void RegisterRemueraLocation()
        //{
        //    var json = CreateNewLocationSaveCommand(REMUERA_NAME);
        //    var response = Post<LocationData>(json);
        //    RemueraId = ((LocationData)response.Payload).id;
        //}

        private string CreateNewCoachSaveCommand(string firstName, string lastName, string email, string phone)
        {
            var coach = new ApiCoachSaveCommand
            {
                businessId = BusinessId,
                firstName = firstName,
                lastName = lastName,
                email = email,
                phone = phone,
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

        //private string GivenMissingWorkingHoursProperties()
        //{
        //    var registration = new ApiCoachSaveCommand
        //    {
        //        businessName = BusinessName,
        //        registrant = new ApiBusinessRegistrant()
        //    };

        //    return JsonConvert.SerializeObject(registration);
        //}


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


        public class ApiCoachSaveCommand
        {
            public Guid? businessId { get; set; }
            public Guid? id { get; set; }

            public string firstName { get; set; }
            public string lastName { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
            public ApiWeeklyWorkingHours workingHours { get; set; }
        }

        public class ApiWeeklyWorkingHours
        {
            public ApiDailyWorkingHours monday { get; set; }
            public ApiDailyWorkingHours tuesday { get; set; }
            public ApiDailyWorkingHours wednesday { get; set; }
            public ApiDailyWorkingHours thursday { get; set; }
            public ApiDailyWorkingHours friday { get; set; }
            public ApiDailyWorkingHours saturday { get; set; }
            public ApiDailyWorkingHours sunday { get; set; }
        }

        public class ApiDailyWorkingHours
        {
            public bool isAvailable { get; set; }
            public string startTime { get; set; }
            public string finishTime { get; set; }

            public ApiDailyWorkingHours()
            { }

            public ApiDailyWorkingHours(bool isAvailable, string startTime, string finishTime)
            {
                this.isAvailable = isAvailable;
                this.startTime = startTime;
                this.finishTime = finishTime;
            }
        }
    }
}
