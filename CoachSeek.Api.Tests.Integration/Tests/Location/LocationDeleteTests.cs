using System;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Location
{
    [TestFixture]
    public class LocationDeleteTests : WebIntegrationTest
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
        public void GivenNonExistentLocationId_WhenTryDelete_ThenReturnNotFoundErrorResponse()
        {
            var id = GivenNonExistentLocationId();
            var response = WhenTryDelete(id);
            AssertNotFound(response);
        }


        private Guid GivenNonExistentLocationId()
        {
            return Guid.NewGuid();
        }


        private Response WhenTryDelete(Guid id)
        {
            return Delete<LocationData>("Locations", id);
        }
    }
}
