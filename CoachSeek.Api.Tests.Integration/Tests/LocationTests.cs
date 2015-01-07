using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    [TestFixture]
    public class LocationTests : WebIntegrationTest
    {
        private const string ORAKEI_NAME = "Orakei Tennis Club";
        private const string REMUERA_NAME = "Remuera Racquets Club";

        private Guid OrakeiId { get; set; }
        private Guid RemueraId { get; set; }
        private string NewLocationName { get; set; }

        protected override string RelativePath
        {
            get { return "Locations"; }
        }

        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            RegisterTestLocations();

            NewLocationName = string.Empty;
        }

        private void RegisterTestLocations()
        {
            RegisterOrakeiLocation();
            RegisterRemueraLocation();
        }

        private void RegisterOrakeiLocation()
        {
            var json = CreateNewLocationSaveCommand(ORAKEI_NAME);
            var response = Post<LocationData>(json);
            OrakeiId = ((LocationData)response.Payload).id;
        }

        private void RegisterRemueraLocation()
        {
            var json = CreateNewLocationSaveCommand(REMUERA_NAME);
            var response = Post<LocationData>(json);
            RemueraId = ((LocationData)response.Payload).id;
        }

        private string CreateNewLocationSaveCommand(string name)
        {
            var location = new ApiLocationSaveCommand
            {
                name = name
            };

            return JsonConvert.SerializeObject(location);
        }


        [Test]
        public void GivenNoLocationSaveCommand_WhenPost_ThenReturnNoDataErrorResponse()
        {
            var command = GivenNoLocationSaveCommand();
            var response = WhenPost(command);
            ThenReturnNoDataErrorResponse(response);
        }

        [Test]
        public void GivenEmptyLocationSaveCommand_WhenPost_ThenReturnRootRequiredErrorResponse()
        {
            var command = GivenEmptyLocationSaveCommand();
            var response = WhenPost(command);
            ThenReturnRootRequiredErrorResponse(response);
        }

        [Test]
        public void GivenNonExistentLocationId_WhenPost_ThenReturnInvalidLocationIdErrorResponse()
        {
            var command = GivenNonExistentLocationId();
            var response = WhenPost(command);
            ThenReturnInvalidLocationIdErrorResponse(response);
        }

        [Test]
        public void GivenNewLocationWithAnAlreadyExistingLocationName_WhenPost_ThenReturnDuplicateLocationErrorResponse()
        {
            var command = GivenNewLocationWithAnAlreadyExistingLocationName();
            var response = WhenPost(command);
            ThenReturnDuplicateLocationErrorResponse(response);
        }

        [Test]
        public void GivenExistingLocationAndChangeToAnAlreadyExistingLocationName_WhenPost_ThenReturnDuplicateLocationErrorResponse()
        {
            var command = GivenExistingLocationAndChangeToAnAlreadyExistingLocationName();
            var response = WhenPost(command);
            ThenReturnDuplicateLocationErrorResponse(response);
        }

        [Test]
        public void GivenNewUniqueLocation_WhenPost_ThenReturnNewLocationSuccessResponse()
        {
            var command = GivenNewUniqueLocation();
            var response = WhenPost(command);
            ThenReturnNewLocationSuccessResponse(response);
        }

        [Test]
        public void GivenExistingLocationAndChangeToUniqueLocationName_WhenPost_ThenReturnExistingLocationSuccessResponse()
        {
            var command = GivenExistingLocationAndChangeToUniqueLocationName();
            var response = WhenPost(command);
            ThenReturnExistingLocationSuccessResponse(response);
        }

        [Test]
        public void GivenExistingLocationAndKeepLocationNameSame_WhenPost_ThenReturnExistingLocationSuccessResponse()
        {
            var command = GivenExistingLocationAndKeepLocationNameSame();
            var response = WhenPost(command);
            ThenReturnExistingLocationSuccessResponse(response);
        }


        private string GivenNoLocationSaveCommand()
        {
            return "";
        }

        private string GivenEmptyLocationSaveCommand()
        {
            return "{}";
        }

        private string GivenNonExistentLocationId()
        {
            var location = new ApiLocationSaveCommand
            {
                id = Guid.Empty,
                name = RandomString
            };

            return JsonConvert.SerializeObject(location);
        }

        private string GivenNewLocationWithAnAlreadyExistingLocationName()
        {
            var location = new ApiLocationSaveCommand
            {
                name = ORAKEI_NAME
            };

            return JsonConvert.SerializeObject(location);
        }

        private string GivenExistingLocationAndChangeToAnAlreadyExistingLocationName()
        {
            var location = new ApiLocationSaveCommand
            {
                id = RemueraId,
                name = ORAKEI_NAME
            };

            return JsonConvert.SerializeObject(location);
        }

        private string GivenNewUniqueLocation()
        {
            var location = new ApiLocationSaveCommand
            {
                name = "Mt Eden Squash Club"
            };

            return JsonConvert.SerializeObject(location);
        }

        private string GivenExistingLocationAndChangeToUniqueLocationName()
        {
            NewLocationName = "Orakei Tennis & Squash Club";

            var location = new ApiLocationSaveCommand
            {
                id = OrakeiId,
                name = NewLocationName
            };

            return JsonConvert.SerializeObject(location);
        }

        private string GivenExistingLocationAndKeepLocationNameSame()
        {
            NewLocationName = ORAKEI_NAME;

            var location = new ApiLocationSaveCommand
            {
                id = OrakeiId,
                name = NewLocationName
            };

            return JsonConvert.SerializeObject(location);
        }


        private Response WhenPost(string json)
        {
            return Post<LocationData>(json);
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
            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "location.name", "The name field is required.");
        }

        private void ThenReturnInvalidBusinessIdErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            
            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "location.businessId", "This business does not exist.");
        }

        private void ThenReturnInvalidLocationIdErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "location.id", "This location does not exist.");
        }

        private void ThenReturnDuplicateLocationErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "location.name", "This location already exists.");
        }

        private void ThenReturnNewLocationSuccessResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<LocationData>());
            var location = (LocationData)response.Payload;
            Assert.That(location.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(location.name, Is.EqualTo("Mt Eden Squash Club"));
        }

        private void ThenReturnExistingLocationSuccessResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<LocationData>());
            var location = (LocationData)response.Payload;
            Assert.That(location.id, Is.EqualTo(OrakeiId));
            Assert.That(location.name, Is.EqualTo(NewLocationName));
        }
    }
}
