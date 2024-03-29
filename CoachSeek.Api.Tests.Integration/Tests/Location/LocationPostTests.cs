﻿using System;
using System.Net;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
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

                return JsonSerialiser.Serialise(command);
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
                var command = new ApiLocationSaveCommand
                {
                    name = duplicateLocationName
                };

                return JsonSerialiser.Serialise(command);
            }

            private string GivenNewUniqueLocation()
            {
                var command = new ApiLocationSaveCommand
                {
                    name = "Mt Eden Squash Club"
                };

                return JsonSerialiser.Serialise(command);
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
                ThenReturnInvalidLocationIdError(response, command.id.Value);
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


            private ApiLocationSaveCommand GivenNonExistentLocationId()
            {
                return new ApiLocationSaveCommand
                {
                    id = Guid.NewGuid(),
                    name = Random.RandomString
                };
            }

            private string GivenExistingLocationAndChangeToAnAlreadyExistingLocationName(SetupData setup)
            {
                var command = new ApiLocationSaveCommand
                {
                    id = setup.Remuera.Id,
                    name = setup.Orakei.Name
                };

                return JsonSerialiser.Serialise(command);
            }

            private string GivenExistingLocationAndChangeToUniqueLocationName(SetupData setup, string newLocationName)
            {
                var command = new ApiLocationSaveCommand
                {
                    id = setup.Orakei.Id,
                    name = newLocationName
                };

                return JsonSerialiser.Serialise(command);
            }

            private string GivenExistingLocationAndKeepLocationNameSame(SetupData setup)
            {
                var command = new ApiLocationSaveCommand
                {
                    id = setup.Orakei.Id,
                    name = setup.Orakei.Name
                };

                return JsonSerialiser.Serialise(command);
            }
        }


        private ApiResponse WhenTryPost(ApiLocationSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return AuthenticatedPost<LocationData>(json, RelativePath, setup);
        }

        private ApiResponse WhenTryPost(string json, SetupData setup)
        {
            return AuthenticatedPost<LocationData>(json, RelativePath, setup);
        }

        private ApiResponse WhenTryPostAnonymously(string json, SetupData setup)
        {
            return BusinessAnonymousPost<LocationData>(json, RelativePath, setup);
        }


        private void ThenReturnNoDataError(ApiResponse response)
        {
            AssertSingleError(response, ErrorCodes.DataRequired, "Please post us some data!");
        }

        private void ThenReturnRootRequiredError(ApiResponse response)
        {
            AssertSingleError(response, "name-required", "The Name field is required.", null);
        }

        private void ThenReturnInvalidLocationIdError(ApiResponse response, Guid locationId)
        {
            AssertSingleError(response, ErrorCodes.LocationInvalid, "This location does not exist.", locationId.ToString());
        }

        private void ThenReturnDuplicateLocationError(ApiResponse response)
        {
            AssertSingleError(response, ErrorCodes.LocationDuplicate, "Location 'Orakei Tennis Club' already exists.", "Orakei Tennis Club");
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
