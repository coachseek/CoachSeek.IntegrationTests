using System;
using System.Collections.Generic;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Location
{
    public class LocationGetTests : LocationTests
    {
        [TestFixture]
        public class AnonymousLocationGetTests : LocationGetTests
        {
            [Test]
            public void GivenNoBusinessDomain_WhenTryGetLocationByIdAnonymously_ThenReturnNotAuthorised()
            {
                var setup = RegisterBusiness();
                RegisterLocationOrakei(setup);

                var businessDomain = GivenNoBusinessDomain();
                var response = WhenTryGetLocationByIdAnonymously(setup.Orakei.Id, businessDomain);
                ThenReturnUnauthorised(response);
            }

            [Test]
            public void GivenInvalidBusinessDomain_WhenTryGetLocationByIdAnonymously_ThenReturnNotAuthorised()
            {
                var setup = RegisterBusiness();
                RegisterLocationOrakei(setup);

                var businessDomain = GivenInvalidBusinessDomain();
                var response = WhenTryGetLocationByIdAnonymously(setup.Orakei.Id, businessDomain);
                ThenReturnUnauthorised(response);
            }

            [Test]
            public void GivenValidBusinessDomain_WhenTryGetLocationByIdAnonymously_ThenReturnLocation()
            {
                var setup = RegisterBusiness();
                RegisterLocationOrakei(setup);

                var businessDomain = GivenValidBusinessDomain(setup);
                var response = WhenTryGetLocationByIdAnonymously(setup.Orakei.Id, businessDomain);
                ThenReturnLocation(response, setup);
            }

            [Test]
            public void GivenNoBusinessDomain_WhenTryGetAllLocationsAnonymously_ThenReturnNotAuthorised()
            {
                var businessDomain = GivenNoBusinessDomain();
                var response = WhenTryGetAllLocationsAnonymously(businessDomain);
                ThenReturnUnauthorised(response);
            }

            [Test]
            public void GivenInvalidBusinessDomain_WhenTryGetAllLocationsAnonymously_ThenReturnNotAuthorised()
            {
                var businessDomain = GivenInvalidBusinessDomain();
                var response = WhenTryGetAllLocationsAnonymously(businessDomain);
                ThenReturnUnauthorised(response);
            }

            [Test]
            public void GivenValidBusinessDomain_WhenTryGetAllLocationsAnonymously_ThenReturnAllLocations()
            {
                var setup = RegisterBusiness();
                RegisterTestLocations(setup);

                var businessDomain = GivenValidBusinessDomain(setup);
                var response = WhenTryGetAllLocationsAnonymously(businessDomain);
                ThenReturnAllLocations(response, setup);
            }


            private string GivenNoBusinessDomain()
            {
                return null;
            }

            private string GivenInvalidBusinessDomain()
            {
                return "abc123";
            }

            private string GivenValidBusinessDomain(SetupData setup)
            {
                return setup.Business.Domain;
            }


            private ApiResponse WhenTryGetLocationByIdAnonymously(Guid locationId, string businessDomain)
            {
                var url = string.Format("{0}/{1}", RelativePath, locationId);
                return new TestBusinessAnonymousApiClient().Get<LocationData>(businessDomain, url);
            }

            private ApiResponse WhenTryGetAllLocationsAnonymously(string businessDomain)
            {
                return new TestBusinessAnonymousApiClient().Get<List<LocationData>>(businessDomain, RelativePath);
            }


            private void ThenReturnUnauthorised(ApiResponse response)
            {
                AssertUnauthorised(response);
            }
        }


        [TestFixture]
        public class AuthenticatedLocationGetTests : LocationGetTests
        {
            [Test]
            public void WhenTryGetAllLocations_ThenReturnAllLocations()
            {
                var setup = RegisterBusiness();
                RegisterTestLocations(setup);

                var response = WhenTryGetAllLocations(setup);
                ThenReturnAllLocations(response, setup);
            }

            [Test]
            public void GivenInvalidLocationId_WhenTryGetLocationById_ThenReturnNotFound()
            {
                var setup = RegisterBusiness();

                var locationId = GivenInvalidLocationId();
                var response = WhenTryGetLocationById(locationId, setup);
                ThenReturnNotFound(response);
            }

            [Test]
            public void GivenValidLocationId_WhenTryGetLocationById_ThenReturnLocation()
            {
                var setup = RegisterBusiness();
                RegisterLocationOrakei(setup);

                var locationId = GivenValidLocationId(setup);
                var response = WhenTryGetLocationById(locationId, setup);
                ThenReturnLocation(response, setup);
            }
        }


        private Guid GivenInvalidLocationId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidLocationId(SetupData setup)
        {
            return setup.Orakei.Id;
        }


        private ApiResponse WhenTryGetAllLocations(SetupData setup)
        {
            return new TestAuthenticatedApiClient().Get<List<LocationData>>(setup.Business.UserName,
                                                                            setup.Business.Password, 
                                                                            RelativePath);
        }

        private ApiResponse WhenTryGetLocationById(Guid locationId, SetupData setup)
        {
            var url = string.Format("{0}/{1}", RelativePath, locationId);
            return new TestAuthenticatedApiClient().Get<LocationData>(setup.Business.UserName,
                                                                      setup.Business.Password,
                                                                      url);
        }


        private void ThenReturnNotFound(ApiResponse response)
        {
            AssertNotFound(response);
        }

        private void ThenReturnAllLocations(ApiResponse response, SetupData setup)
        {
            var locations = AssertSuccessResponse<List<LocationData>>(response);
            Assert.That(locations.Count, Is.EqualTo(2));
            AssertLocation(locations[0], setup.Orakei.Id, setup.Orakei.Name);
            AssertLocation(locations[1], setup.Remuera.Id, setup.Remuera.Name);
        }

        private void ThenReturnLocation(ApiResponse response, SetupData setup)
        {
            var location = AssertSuccessResponse<LocationData>(response);
            AssertLocation(location, setup.Orakei.Id, setup.Orakei.Name);
        }

        private void AssertLocation(LocationData location, Guid expectedId, string expectedName)
        {
            Assert.That(location.id, Is.EqualTo(expectedId));
            Assert.That(location.name, Is.EqualTo(expectedName));
        }
    }
}
