using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Location
{
    public class LocationPostTests : LocationTests
    {
        private string NewLocationName { get; set; }


        protected override void SetupAddtitional()
        {
            NewLocationName = string.Empty;
        }


        [TestFixture]
        public class LocationCommandTests : LocationPostTests
        {
            [Test]
            public void GivenNoLocationSaveCommand_WhenTryPost_ThenReturnNoDataError()
            {
                var command = GivenNoLocationSaveCommand();
                var response = WhenTryPost(command);
                ThenReturnNoDataErrorResponse(response);
            }

            [Test]
            public void GivenEmptyLocationSaveCommand_WhenTryPost_ThenReturnRootRequiredError()
            {
                var command = GivenEmptyLocationSaveCommand();
                var response = WhenTryPost(command);
                ThenReturnRootRequiredErrorResponse(response);
            }

            [Test]
            public void GivenValidLocationSaveCommand_WhenTryPostAnonymously_ThenReturnUnauthorised()
            {
                var command = GivenValidLocationSaveCommand();
                var response = WhenTryPostAnonymously(command);
                AssertUnauthorised(response);
            }

            
            private string GivenNoLocationSaveCommand()
            {
                return "";
            }

            private string GivenEmptyLocationSaveCommand()
            {
                return "{}";
            }

            private string GivenValidLocationSaveCommand()
            {
                var command = new ApiLocationSaveCommand
                {
                    name = "Mt Eden Soccer Club"
                };

                return JsonConvert.SerializeObject(command);
            }
        }


        [TestFixture]
        public class LocationNewTests : LocationPostTests
        {
            [Test]
            public void GivenNewLocationWithAnAlreadyExistingLocationName_WhenTryPost_ThenReturnDuplicateLocationErrorResponse()
            {
                var command = GivenNewLocationWithAnAlreadyExistingLocationName();
                var response = WhenTryPost(command);
                ThenReturnDuplicateLocationErrorResponse(response);
            }

            [Test]
            public void GivenNewUniqueLocation_WhenTryPost_ThenReturnNewLocationSuccessResponse()
            {
                var command = GivenNewUniqueLocation();
                var response = WhenTryPost(command);
                ThenReturnNewLocationSuccessResponse(response);
            }


            private string GivenNewLocationWithAnAlreadyExistingLocationName()
            {
                var location = new ApiLocationSaveCommand
                {
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
        }


        [TestFixture]
        public class LocationExistingTests : LocationPostTests
        {
            [Test]
            public void GivenNonExistentLocationId_WhenTryPost_ThenReturnInvalidLocationIdErrorResponse()
            {
                var command = GivenNonExistentLocationId();
                var response = WhenTryPost(command);
                ThenReturnInvalidLocationIdErrorResponse(response);
            }

            [Test]
            public void GivenExistingLocationAndChangeToAnAlreadyExistingLocationName_WhenTryPost_ThenReturnDuplicateLocationErrorResponse()
            {
                var command = GivenExistingLocationAndChangeToAnAlreadyExistingLocationName();
                var response = WhenTryPost(command);
                ThenReturnDuplicateLocationErrorResponse(response);
            }

            [Test]
            public void GivenExistingLocationAndChangeToUniqueLocationName_WhenTryPost_ThenReturnExistingLocationSuccessResponse()
            {
                var command = GivenExistingLocationAndChangeToUniqueLocationName();
                var response = WhenTryPost(command);
                ThenReturnExistingLocationSuccessResponse(response);
            }

            [Test]
            public void GivenExistingLocationAndKeepLocationNameSame_WhenTryPost_ThenReturnExistingLocationSuccessResponse()
            {
                var command = GivenExistingLocationAndKeepLocationNameSame();
                var response = WhenTryPost(command);
                ThenReturnExistingLocationSuccessResponse(response);
            }

            
            private string GivenNonExistentLocationId()
            {
                var location = new ApiLocationSaveCommand
                {
                    id = Guid.Empty,
                    name = Random.RandomString
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
        }


        private Response WhenTryPost(string json)
        {
            return Post<LocationData>(json);
        }

        private Response WhenTryPostAnonymously(string json)
        {
            return PostAnonymouslyToBusiness<LocationData>(json);
        }


        private void ThenReturnNoDataErrorResponse(Response response)
        {
            AssertSingleError(response, "Please post us some data!");
        }

        private void ThenReturnRootRequiredErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "location.name", "The name field is required.");
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
