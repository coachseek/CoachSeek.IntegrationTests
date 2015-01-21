using System;
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


        private Response WhenGetById(Guid locationId)
        {
            var url = BuildGetByIdUrl(locationId);
            return Get<LocationData>(url);
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
