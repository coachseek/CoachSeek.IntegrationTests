using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Location;
using Newtonsoft.Json;

namespace CoachSeek.Api.Tests.Integration
{
    public static class LocationRegistrar
    {
        public static void RegisterLocation(ExpectedLocation location, ExpectedBusiness business)
        {
            var json = CreateNewCoachSaveCommand(location);
            var response = PostLocation(business, json);
            if (response.Payload != null)
                location.Id = ((LocationData)response.Payload).id;
        }

        private static string CreateNewCoachSaveCommand(ExpectedLocation expectedLocation)
        {
            var location = new ApiLocationSaveCommand
            {
                name = expectedLocation.Name
            };

            return JsonConvert.SerializeObject(location);
        }

        private static Response PostLocation(ExpectedBusiness business, string json)
        {
            return WebClient.AuthenticatedPost<LocationData>(business.UserName, business.Password, "Locations", json);
        }
    }
}
