using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    [TestFixture]
    public class ServiceTests : WebIntegrationTest
    {
        private const string MINI_RED_NAME = "Mini Red";
        private const string MINI_BLUE_NAME = "Mini Blue";

        private Guid MiniRedId { get; set; }
        private Guid MiniBlueId { get; set; }
        private string NewServiceName { get; set; }
        private int? Duration { get; set; }
        private decimal? Price { get; set; }
        private int? StudentCapacity { get; set; }
        private bool? IsOnlineBookable { get; set; }
        private string Colour { get; set; }


        protected override string RelativePath
        {
            get { return "Services"; }
        }

        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            RegisterTestServices();

            NewServiceName = string.Empty;
        }

        private void RegisterTestServices()
        {
            RegisterMiniRedService();
            RegisterMiniBlueService();
        }

        private void RegisterMiniRedService()
        {
            var json = CreateNewServiceSaveCommand(MINI_RED_NAME, "Mini Red Description");
            var response = Post<ServiceData>(json);
            MiniRedId = ((ServiceData)response.Payload).id;
        }

        private void RegisterMiniBlueService()
        {
            var json = CreateNewServiceSaveCommand(MINI_BLUE_NAME, "Mini Blue Description");
            var response = Post<ServiceData>(json);
            MiniBlueId = ((ServiceData)response.Payload).id;
        }

        private string CreateNewServiceSaveCommand(string name, string description)
        {
            var service = new ApiServiceSaveCommand
            {
                businessId = BusinessId,
                name = name,
                description = description,
                defaults = new ApiServiceDefaults
                {
                    duration = 45,
                    price = 50,
                    studentCapacity = 6,
                    isOnlineBookable = null,
                    colour = "orange"
                }

            };

            return JsonConvert.SerializeObject(service);
        }


        [Test]
        public void GivenNoServiceSaveCommand_WhenPost_ThenReturnNoDataErrorResponse()
        {
            var command = GivenNoServiceSaveCommand();
            var response = WhenPost(command);
            ThenReturnNoDataErrorResponse(response);
        }

        [Test]
        public void GivenEmptyServiceSaveCommand_WhenPost_ThenReturnRootRequiredErrorResponse()
        {
            var command = GivenEmptyServiceSaveCommand();
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
        public void GivenNonExistentServiceId_WhenPost_ThenReturnInvalidServiceIdErrorResponse()
        {
            var command = GivenNonExistentServiceId();
            var response = WhenPost(command);
            ThenReturnInvalidServiceIdErrorResponse(response);
        }

        [Test]
        public void GivenNewServiceWithAnAlreadyExistingServiceName_WhenPost_ThenReturnDuplicateServiceErrorResponse()
        {
            var command = GivenNewServiceWithAnAlreadyExistingServiceName();
            var response = WhenPost(command);
            ThenReturnDuplicateServiceErrorResponse(response);
        }

        [Test]
        public void GivenExistingServiceAndChangeToAnAlreadyExistingServiceName_WhenPost_ThenReturnDuplicateServiceErrorResponse()
        {
            var command = GivenExistingServiceAndChangeToAnAlreadyExistingServiceName();
            var response = WhenPost(command);
            ThenReturnDuplicateServiceErrorResponse(response);
        }

        [Test]
        public void GivenNewUniqueService_WhenPost_ThenReturnNewServiceSuccessResponse()
        {
            var command = GivenNewUniqueService();
            var response = WhenPost(command);
            ThenReturnNewServiceSuccessResponse(response);
        }

        [Test]
        public void GivenExistingServiceAndChangeToUniqueServiceName_WhenPost_ThenReturnExistingServiceSuccessResponse()
        {
            var command = GivenExistingServiceAndChangeToUniqueServiceName();
            var response = WhenPost(command);
            ThenReturnExistingServiceSuccessResponse(response);
        }

        [Test]
        public void GivenExistingServiceAndKeepServiceNameSame_WhenPost_ThenReturnExistingServiceSuccessResponse()
        {
            var command = GivenExistingServiceAndKeepServiceNameSame();
            var response = WhenPost(command);
            ThenReturnExistingServiceSuccessResponse(response);
        }

        [Test]
        public void GivenNewServiceWithDefaults_WhenPost_ThenReturnNewServiceWithDefaultsSuccessResponse()
        {
            var command = GivenNewServiceWithDefaults();
            var response = WhenPost(command);
            ThenReturnNewServiceWithDefaultsSuccessResponse(response);
        }

        [Test]
        public void GivenNewServiceWithInvalidDefaults_WhenPost_ThenReturnServiceDefaultsErrorResponse()
        {
            var command = GivenNewServiceWithInvalidDefaults();
            var response = WhenPost(command);
            ThenReturnServiceDefaultsErrorResponse(response);
        }

        [Test]
        public void GivenExistingServiceWithDefaults_WhenPost_ThenReturnExistingServiceWithDefaultsSuccessResponse()
        {
            var command = GivenExistingServiceWithDefaults();
            var response = WhenPost(command);
            ThenReturnExistingServiceWithDefaultsSuccessResponse(response);
        }


        private string GivenNoServiceSaveCommand()
        {
            return "";
        }

        private string GivenEmptyServiceSaveCommand()
        {
            return "{}";
        }

        private string GivenNonExistentBusinessId()
        {
            var service = new ApiServiceSaveCommand
            {
                businessId = Guid.Empty,
                name = RandomString,
                description = RandomString
            };

            return JsonConvert.SerializeObject(service);
        }

        private string GivenNonExistentServiceId()
        {
            var service = new ApiServiceSaveCommand
            {
                businessId = BusinessId,
                id = Guid.Empty,
                name = RandomString,
                description = RandomString
            };

            return JsonConvert.SerializeObject(service);
        }

        private string GivenNewServiceWithAnAlreadyExistingServiceName()
        {
            var service = new ApiServiceSaveCommand
            {
                businessId = BusinessId,
                name = MINI_RED_NAME,
                description = RandomString
            };

            return JsonConvert.SerializeObject(service);
        }

        private string GivenExistingServiceAndChangeToAnAlreadyExistingServiceName()
        {
            var service = new ApiServiceSaveCommand
            {
                businessId = BusinessId,
                id = MiniRedId,
                name = MINI_BLUE_NAME,
                description = RandomString
            };

            return JsonConvert.SerializeObject(service);
        }

        private string GivenNewUniqueService()
        {
            var service = new ApiServiceSaveCommand
            {
                businessId = BusinessId,
                name = "Mini Orange",
                description = "Tennis for 5-7 year olds of intermediate skill level."
            };

            return JsonConvert.SerializeObject(service);
        }

        private string GivenExistingServiceAndChangeToUniqueServiceName()
        {
            NewServiceName = "Mini Red #3";

            var service = new ApiServiceSaveCommand
            {
                businessId = BusinessId,
                id = MiniRedId,
                name = NewServiceName,
                description = "Tennis for 6-8 year olds of low skill level."
            };

            return JsonConvert.SerializeObject(service);
        }

        private string GivenExistingServiceAndKeepServiceNameSame()
        {
            NewServiceName = MINI_RED_NAME;

            var service = new ApiServiceSaveCommand
            {
                businessId = BusinessId,
                id = MiniRedId,
                name = NewServiceName,
                description = "Tennis for 6-8 year olds of low skill level."
            };

            return JsonConvert.SerializeObject(service);
        }

        private string GivenNewServiceWithDefaults()
        {
            Duration = 60;
            Price = 45;
            StudentCapacity = 8;
            IsOnlineBookable = true;
            Colour = "orange";

            var service = new ApiServiceSaveCommand
            {
                businessId = BusinessId,
                name = "Mini Orange",
                description = "Mini Orange Service",
                defaults = new ApiServiceDefaults
                {
                    duration = Duration,
                    price = Price,
                    studentCapacity = StudentCapacity,
                    isOnlineBookable = IsOnlineBookable,
                    colour = Colour
                }
            };

            return JsonConvert.SerializeObject(service);
        }

        private string GivenNewServiceWithInvalidDefaults()
        {
            Duration = 67;
            Price = 78.904m;
            StudentCapacity = -8;
            IsOnlineBookable = true;
            Colour = "mandarin";

            var service = new ApiServiceSaveCommand
            {
                businessId = BusinessId,
                name = "Mini Orange",
                description = "Mini Orange Service",
                defaults = new ApiServiceDefaults
                {
                    duration = Duration,
                    price = Price,
                    studentCapacity = StudentCapacity,
                    isOnlineBookable = IsOnlineBookable,
                    colour = Colour
                }
            };

            return JsonConvert.SerializeObject(service);
        }

        private string GivenExistingServiceWithDefaults()
        {
            Duration = 60;
            Price = 75;
            StudentCapacity = 8;
            IsOnlineBookable = true;
            Colour = "red";

            var service = new ApiServiceSaveCommand
            {
                businessId = BusinessId,
                id = MiniRedId,
                name = "Mini Red",
                description = "Mini Red Service",
                defaults = new ApiServiceDefaults
                {
                    duration = Duration,
                    price = Price,
                    studentCapacity = StudentCapacity,
                    isOnlineBookable = IsOnlineBookable,
                    colour = Colour
                }
            };

            return JsonConvert.SerializeObject(service);
        }

        private Response WhenPost(string json)
        {
            return Post<ServiceData>(json);
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
            Assert.That(errors.GetLength(0), Is.EqualTo(2));
            AssertApplicationError(errors[0], "service.businessId", "The businessId field is required.");
            AssertApplicationError(errors[1], "service.name", "The name field is required.");
        }

        private void ThenReturnInvalidBusinessIdErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "service.businessId", "This business does not exist.");
        }

        private void ThenReturnInvalidServiceIdErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "service.id", "This service does not exist.");
        }

        private void ThenReturnDuplicateServiceErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "service.name", "This service already exists.");
        }

        private void ThenReturnNewServiceSuccessResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<ServiceData>());
            var service = (ServiceData)response.Payload;
            Assert.That(service.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(service.name, Is.EqualTo("Mini Orange"));
            Assert.That(service.description, Is.EqualTo("Tennis for 5-7 year olds of intermediate skill level."));
        }

        private void ThenReturnExistingServiceSuccessResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<ServiceData>());
            var service = (ServiceData)response.Payload;
            Assert.That(service.id, Is.EqualTo(MiniRedId));
            Assert.That(service.name, Is.EqualTo(NewServiceName));
            Assert.That(service.description, Is.EqualTo("Tennis for 6-8 year olds of low skill level."));
        }

        private void ThenReturnNewServiceWithDefaultsSuccessResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<ServiceData>());
            var service = (ServiceData)response.Payload;
            Assert.That(service.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(service.name, Is.EqualTo("Mini Orange"));
            Assert.That(service.description, Is.EqualTo("Mini Orange Service"));
            var defaults = service.defaults;
            Assert.That(defaults.duration, Is.EqualTo(Duration));
            Assert.That(defaults.price, Is.EqualTo(Price));
            Assert.That(defaults.studentCapacity, Is.EqualTo(StudentCapacity));
            Assert.That(defaults.isOnlineBookable, Is.EqualTo(IsOnlineBookable));
            Assert.That(defaults.colour, Is.EqualTo(Colour));
        }

        private void ThenReturnServiceDefaultsErrorResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;

            Assert.That(errors.GetLength(0), Is.EqualTo(4));
            AssertApplicationError(errors[0], "service.duration", "The duration is not valid.");
            AssertApplicationError(errors[1], "service.price", "The price is not valid.");
            AssertApplicationError(errors[2], "service.studentCapacity", "The studentCapacity is not valid.");
            AssertApplicationError(errors[3], "service.colour", "The colour is not valid.");
        }

        private void ThenReturnExistingServiceWithDefaultsSuccessResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<ServiceData>());
            var service = (ServiceData)response.Payload;
            Assert.That(service.id, Is.EqualTo(MiniRedId));
            Assert.That(service.name, Is.EqualTo("Mini Red"));
            Assert.That(service.description, Is.EqualTo("Mini Red Service"));
            var defaults = service.defaults;
            Assert.That(defaults.duration, Is.EqualTo(Duration));
            Assert.That(defaults.price, Is.EqualTo(Price));
            Assert.That(defaults.studentCapacity, Is.EqualTo(StudentCapacity));
            Assert.That(defaults.isOnlineBookable, Is.EqualTo(IsOnlineBookable));
            Assert.That(defaults.colour, Is.EqualTo(Colour));
        }
    }
}
