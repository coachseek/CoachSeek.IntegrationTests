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
            public void GivenNoBusinessDomain_WhenTryGetByIdAnonymously_ThenReturnNotAuthorised()
            {
                GivenNoBusinessDomain();
                var response = WhenTryGetByIdAnonymously(OrakeiId);
                ThenReturnNotAuthorised(response);
            }

            [Test]
            public void GivenInvalidBusinessDomain_WhenTryGetByIdAnonymously_ThenReturnNotAuthorised()
            {
                GivenInvalidBusinessDomain();
                var response = WhenTryGetByIdAnonymously(OrakeiId);
                ThenReturnNotAuthorised(response);
            }

            [Test]
            public void GivenValidBusinessDomain_WhenTryGetByIdAnonymously_ThenReturnLocation()
            {
                GivenValidBusinessDomain();
                var response = WhenTryGetByIdAnonymously(OrakeiId);
                ThenReturnLocation(response);
            }

            [Test]
            public void GivenNoBusinessDomain_WhenTryGetAllAnonymously_ThenReturnNotAuthorised()
            {
                GivenNoBusinessDomain();
                var response = WhenTryGetAllAnonymously();
                ThenReturnNotAuthorised(response);
            }

            [Test]
            public void GivenInvalidBusinessDomain_WhenTryGetAllAnonymously_ThenReturnNotAuthorised()
            {
                GivenInvalidBusinessDomain();
                var response = WhenTryGetAllAnonymously();
                ThenReturnNotAuthorised(response);
            }

            [Test]
            public void GivenValidBusinessDomain_WhenTryGetAllAnonymously_ThenReturnAllLocations()
            {
                GivenValidBusinessDomain();
                var response = WhenTryGetAllAnonymously();
                ThenReturnAllLocations(response);
            }


            private void GivenNoBusinessDomain()
            {
                Business.Domain = null;
            }

            private void GivenInvalidBusinessDomain()
            {
                Business.Domain = "abc123";
            }

            private void GivenValidBusinessDomain()
            {
                // Valid domain is already set.
            }


            private Response WhenTryGetByIdAnonymously(Guid locationId)
            {
                var url = BuildGetByIdUrl(locationId);
                return GetAnonymously<LocationData>(url);
            }

            private Response WhenTryGetAllAnonymously()
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
            public void WhenTryGetAll_ThenReturnAllLocations()
            {
                var response = WhenTryGetAll();
                ThenReturnAllLocations(response);
            }

            [Test]
            public void GivenInvalidLocationId_WhenTryGetById_ThenReturnNotFound()
            {
                var locationId = GivenInvalidLocationId();
                var response = WhenTryGetById(locationId);
                ThenReturnNotFound(response);
            }

            [Test]
            public void GivenValidLocationId_WhenTryGetById_ThenReturnLocation()
            {
                var locationId = GivenValidLocationId();
                var response = WhenTryGetById(locationId);
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


        private Response WhenTryGetAll()
        {
            var url = BuildGetAllUrl();
            return AuthenticatedGet<List<LocationData>>(url);
        }

        private Response WhenTryGetById(Guid locationId)
        {
            var url = BuildGetByIdUrl(locationId);
            return AuthenticatedGet<LocationData>(url);
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
