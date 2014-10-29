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

        protected override string RelativePath
        {
            get { return "Services"; }
        }

        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            RegisterTestServices();
        }

        private void RegisterTestServices()
        {
            RegisterMiniRedService();
            RegisterMiniBlueService();
        }

        private void RegisterMiniRedService()
        {
            var json = CreateNewServiceSaveCommand(MINI_RED_NAME);
            var response = Post<ServiceData>(json);
            MiniRedId = ((ServiceData)response.Payload).id;
        }

        private void RegisterMiniBlueService()
        {
            var json = CreateNewServiceSaveCommand(MINI_BLUE_NAME);
            var response = Post<ServiceData>(json);
            MiniBlueId = ((ServiceData)response.Payload).id;
        }

        private string CreateNewServiceSaveCommand(string name)
        {
            var service = new ApiServiceSaveCommand
            {
                businessId = BusinessId,
                name = name,
                description = RandomString
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
    }
}
