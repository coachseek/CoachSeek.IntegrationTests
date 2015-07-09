using System;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Location
{
    public abstract class LocationTests : ScheduleTests
    {
        protected const string ORAKEI_NAME = "Orakei Tennis Club";
        protected const string REMUERA_NAME = "Remuera Racquets Club";

        protected Guid OrakeiId { get; set; }
        protected Guid RemueraId { get; set; }

        protected override string RelativePath { get { return "Locations"; } }


        //[SetUp]
        //public void Setup()
        //{
        //    RegisterTestBusiness();
        //    RegisterTestLocations();

        //    SetupAddtitional();
        //}

        protected virtual void SetupAddtitional()
        { }

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
            var location = new ApiLocationSaveCommand { name = name };
            return JsonConvert.SerializeObject(location);
        }
    }
}
