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
        protected override string RelativePath
        {
            get { return "Services"; }
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
            var location = new ApiServiceSaveCommand
            {
                businessId = Guid.Empty,
                name = RandomString,
                description = RandomString
            };

            return JsonConvert.SerializeObject(location);
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
    }
}
