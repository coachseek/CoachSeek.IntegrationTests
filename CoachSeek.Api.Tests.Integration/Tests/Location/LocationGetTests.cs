using System;
using System.Collections.Generic;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Location
{
    [TestFixture]
    public class LocationGetTests : WebIntegrationTest
    {
        private const string ORAKEI_NAME = "Orakei Tennis Club";
        private const string REMUERA_NAME = "Remuera Racquets Club";

        private Guid OrakeiId { get; set; }
        private Guid RemueraId { get; set; }

        protected override string RelativePath
        {
            get { return "Locations"; }
        }

        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            RegisterTestLocations();
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
        public void WhenGetAll_ThenReturnAllLocationsResponse()
        {
            var response = WhenGetAll();
            ThenReturnAllLocationsResponse(response);
        }

        [Test]
        public void GivenInvalidLocationId_WhenGetById_ThenReturnNotFoundResponse()
        {
            var locationId = GivenInvalidLocationId();
            var response = WhenGetById(locationId);
            ThenReturnNotFoundResponse(response);
        }

        [Test]
        public void GivenValidLocationId_WhenGetById_ThenReturnLocationResponse()
        {
            var locationId = GivenValidLocationId();
            var response = WhenGetById(locationId);
            ThenReturnLocationResponse(response);
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


        private void ThenReturnAllLocationsResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Payload, Is.Not.Null);
            var locations = (List<LocationData>)response.Payload;
            Assert.That(locations.Count, Is.EqualTo(2));
            var locationOne = locations[0];
            Assert.That(locationOne.id, Is.EqualTo(OrakeiId));
            Assert.That(locationOne.name, Is.EqualTo(ORAKEI_NAME));
            var locationTwo = locations[1];
            Assert.That(locationTwo.id, Is.EqualTo(RemueraId));
            Assert.That(locationTwo.name, Is.EqualTo(REMUERA_NAME));
        }

        private void ThenReturnNotFoundResponse(Response response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        private void ThenReturnLocationResponse(Response response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var location = (LocationData)response.Payload;
            Assert.That(location.id, Is.EqualTo(OrakeiId));
            Assert.That(location.name, Is.EqualTo(ORAKEI_NAME));
        }
    }
}
