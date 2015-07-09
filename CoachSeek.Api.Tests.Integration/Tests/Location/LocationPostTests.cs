using System;
using System.Net;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Location
{
    public class LocationPostTests : LocationTests
    {
        [TestFixture]
        public class LocationCommandTests : LocationPostTests
        {
            [Test]
            public void GivenNoLocationSaveCommand_WhenTryPost_ThenReturnNoDataError()
            {
                var setup = RegisterBusiness();

                var command = GivenNoLocationSaveCommand();
                var response = WhenTryPost(command, setup);
                ThenReturnNoDataError(response);
            }

            [Test]
            public void GivenEmptyLocationSaveCommand_WhenTryPost_ThenReturnRootRequiredError()
            {
                var setup = RegisterBusiness();

                var command = GivenEmptyLocationSaveCommand();
                var response = WhenTryPost(command, setup);
                ThenReturnRootRequiredError(response);
            }

            [Test]
            public void GivenValidLocationSaveCommand_WhenTryPostAnonymously_ThenReturnUnauthorised()
            {
                var setup = RegisterBusiness();

                var command = GivenValidLocationSaveCommand();
                var response = WhenTryPostAnonymously(command, setup);
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
            public void GivenNewLocationWithAnAlreadyExistingLocationName_WhenTryPost_ThenReturnDuplicateLocationError()
            {
                var setup = RegisterBusiness();
                RegisterLocationOrakei(setup);

                var command = GivenNewLocationWithAnAlreadyExistingLocationName(setup.Orakei.Name);
                var response = WhenTryPost(command, setup);
                ThenReturnDuplicateLocationError(response);
            }

            [Test]
            public void GivenNewUniqueLocation_WhenTryPost_ThenCreateNewLocation()
            {
                var setup = RegisterBusiness();
                RegisterLocationOrakei(setup);

                var command = GivenNewUniqueLocation();
                var response = WhenTryPost(command, setup);
                ThenCreateNewLocation(response);
            }


            private string GivenNewLocationWithAnAlreadyExistingLocationName(string duplicateLocationName)
            {
                var location = new ApiLocationSaveCommand
                {
                    name = duplicateLocationName
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
            public void GivenNonExistentLocationId_WhenTryPost_ThenReturnInvalidLocationIdError()
            {
                var setup = RegisterBusiness();

                var command = GivenNonExistentLocationId();
                var response = WhenTryPost(command, setup);
                ThenReturnInvalidLocationIdError(response);
            }

            [Test]
            public void GivenExistingLocationAndChangeToAnAlreadyExistingLocationName_WhenTryPost_ThenReturnDuplicateLocationErrorResponse()
            {
                var setup = RegisterBusiness();
                RegisterTestLocations(setup);

                var command = GivenExistingLocationAndChangeToAnAlreadyExistingLocationName(setup);
                var response = WhenTryPost(command, setup);
                ThenReturnDuplicateLocationError(response);
            }

            [Test]
            public void GivenExistingLocationAndChangeToUniqueLocationName_WhenTryPost_ThenReturnExistingLocationSuccessResponse()
            {
                var setup = RegisterBusiness();
                RegisterLocationOrakei(setup);

                const string newLocationName = "Orakei Tennis & Squash Club";
                var command = GivenExistingLocationAndChangeToUniqueLocationName(setup, newLocationName);
                var response = WhenTryPost(command, setup);
                ThenReturnExistingLocationSuccessResponse(response, setup, newLocationName);
            }

            [Test]
            public void GivenExistingLocationAndKeepLocationNameSame_WhenTryPost_ThenReturnExistingLocationSuccessResponse()
            {
                var setup = RegisterBusiness();
                RegisterLocationOrakei(setup);

                var command = GivenExistingLocationAndKeepLocationNameSame(setup);
                var response = WhenTryPost(command, setup);
                ThenReturnExistingLocationSuccessResponse(response, setup);
            }

            
            private string GivenNonExistentLocationId()
            {
                var location = new ApiLocationSaveCommand
                {
                    id = Guid.NewGuid(),
                    name = Random.RandomString
                };

                return JsonConvert.SerializeObject(location);
            }

            private string GivenExistingLocationAndChangeToAnAlreadyExistingLocationName(SetupData setup)
            {
                var location = new ApiLocationSaveCommand
                {
                    id = setup.Remuera.Id,
                    name = setup.Orakei.Name
                };

                return JsonConvert.SerializeObject(location);
            }

            private string GivenExistingLocationAndChangeToUniqueLocationName(SetupData setup, string newLocationName)
            {
                var location = new ApiLocationSaveCommand
                {
                    id = setup.Orakei.Id,
                    name = newLocationName
                };

                return JsonConvert.SerializeObject(location);
            }

            private string GivenExistingLocationAndKeepLocationNameSame(SetupData setup)
            {
                var location = new ApiLocationSaveCommand
                {
                    id = setup.Orakei.Id,
                    name = setup.Orakei.Name
                };

                return JsonConvert.SerializeObject(location);
            }
        }


        private ApiResponse WhenTryPost(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<LocationData>(json,
                                                                       setup.Business.UserName,
                                                                       setup.Business.Password,
                                                                       RelativePath);
        }

        private ApiResponse WhenTryPostAnonymously(string json, SetupData setup)
        {
            return new TestBusinessAnonymousApiClient().Post<LocationData>(json,
                                                                           setup.Business.Domain,
                                                                           RelativePath);
        }


        private void ThenReturnNoDataError(ApiResponse response)
        {
            AssertSingleError(response, "Please post us some data!");
        }

        private void ThenReturnRootRequiredError(ApiResponse response)
        {
            AssertSingleError(response, "The name field is required.", "location.name");
        }

        private void ThenReturnInvalidLocationIdError(ApiResponse response)
        {
            AssertSingleError(response, "This location does not exist.", "location.id");
        }

        private void ThenReturnDuplicateLocationError(ApiResponse response)
        {
            AssertSingleError(response, "This location already exists.", "location.name");
        }

        private void ThenCreateNewLocation(ApiResponse response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<LocationData>());
            var location = (LocationData)response.Payload;
            Assert.That(location.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(location.name, Is.EqualTo("Mt Eden Squash Club"));
        }

        private void ThenReturnExistingLocationSuccessResponse(ApiResponse response, 
                                                               SetupData setup, 
                                                               string newLocationName = null)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<LocationData>());
            var location = (LocationData)response.Payload;
            Assert.That(location.id, Is.EqualTo(setup.Orakei.Id));
            Assert.That(location.name, Is.EqualTo(newLocationName ?? setup.Orakei.Name));
        }
    }
}
