using System;
using System.Collections.Generic;
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
            public void GivenNoBusinessDomain_WhenTryGetById_ThenReturnNotAuthorised()
            {
                GivenNoBusinessDomain();
                var response = WhenTryGetById(OrakeiId);
                ThenReturnNotAuthorised(response);
            }

            [Test]
            public void GivenInvalidBusinessDomain_WhenTryGetById_ThenReturnNotAuthorised()
            {
                GivenInvalidBusinessDomain();
                var response = WhenTryGetById(OrakeiId);
                ThenReturnNotAuthorised(response);
            }

            [Test]
            public void GivenValidBusinessDomain_WhenTryGetById_ThenReturnLocation()
            {
                GivenValidBusinessDomain();
                var response = WhenTryGetById(OrakeiId);
                ThenReturnLocation(response);
            }

            [Test]
            public void GivenNoBusinessDomain_WhenTryGetAll_ThenReturnNotAuthorised()
            {
                GivenNoBusinessDomain();
                var response = WhenTryGetAll();
                ThenReturnNotAuthorised(response);
            }

            [Test]
            public void GivenInvalidBusinessDomain_WhenTryGetAll_ThenReturnNotAuthorised()
            {
                GivenInvalidBusinessDomain();
                var response = WhenTryGetAll();
                ThenReturnNotAuthorised(response);
            }

            [Test]
            public void GivenValidBusinessDomain_WhenTryGetAll_ThenReturnAllLocations()
            {
                GivenValidBusinessDomain();
                var response = WhenTryGetAll();
                ThenReturnAllLocations(response);
            }


            private void GivenNoBusinessDomain()
            {
                BusinessDomain = null;
            }

            private void GivenInvalidBusinessDomain()
            {
                BusinessDomain = "abc123";
            }

            private void GivenValidBusinessDomain()
            {
                // Valid domain is already set.
            }


            private Response WhenTryGetById(Guid locationId)
            {
                var url = BuildGetByIdUrl(locationId);
                return GetAnonymously<LocationData>(url);
            }

            private Response WhenTryGetAll()
            {
                var url = BuildGetAllUrl();
                return GetAnonymously<List<LocationData>>(url);
            }


            private void ThenReturnNotAuthorised(Response response)
            {
                AssertUnauthorised(response);
            }
        }

        [TestFixture]
        public class AuthenticatedLocationGetTests : LocationGetTests
        {
            [Test]
            public void WhenGetAll_ThenReturnAllLocationsResponse()
            {
                var response = WhenGetAll();
                ThenReturnAllLocations(response);
            }

            [Test]
            public void GivenInvalidLocationId_WhenGetById_ThenReturnNotFound()
            {
                var locationId = GivenInvalidLocationId();
                var response = WhenGetById(locationId);
                ThenReturnNotFound(response);
            }

            [Test]
            public void GivenValidLocationId_WhenGetById_ThenReturnLocation()
            {
                var locationId = GivenValidLocationId();
                var response = WhenGetById(locationId);
                ThenReturnLocation(response);
            }
        }


        private Guid GivenInvalidLocationId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidLocationId()
        {
            return OrakeiId;
        }


        private Response WhenGetAll()
        {
            var url = BuildGetAllUrl();
            return Get<List<LocationData>>(url);
        }

        private Response WhenGetById(Guid locationId)
        {
            var url = BuildGetByIdUrl(locationId);
            return Get<LocationData>(url);
        }


        private void ThenReturnNotFound(Response response)
        {
            AssertNotFound(response);
        }

        private void ThenReturnAllLocations(Response response)
        {
            var locations = AssertSuccessResponse<List<LocationData>>(response);
            Assert.That(locations.Count, Is.EqualTo(2));
            AssertLocation(locations[0], OrakeiId, ORAKEI_NAME);
            AssertLocation(locations[1], RemueraId, REMUERA_NAME);
        }

        private void ThenReturnLocation(Response response)
        {
            var location = AssertSuccessResponse<LocationData>(response);
            AssertLocation(location, OrakeiId, ORAKEI_NAME);
        }

        private void AssertLocation(LocationData location, Guid expectedId, string expectedName)
        {
            Assert.That(location.id, Is.EqualTo(expectedId));
            Assert.That(location.name, Is.EqualTo(expectedName));
        }
    }
}
