using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Location;

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
            var command = new ApiLocationSaveCommand
            {
                name = expectedLocation.Name
            };

            return JsonSerialiser.Serialise(command);
        }

        private static ApiResponse PostLocation(ExpectedBusiness business, string json)
        {
            return new TestCoachseekAuthenticatedApiClient(business.UserName, business.Password)
                        .Post<LocationData>(json, "Locations");
        }
    }
}

