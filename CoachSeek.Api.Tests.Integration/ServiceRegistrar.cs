using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Service;

namespace CoachSeek.Api.Tests.Integration
{
    public static class ServiceRegistrar
    {
        public static void RegisterService(ExpectedService service, ExpectedBusiness business)
        {
            var json = CreateNewServiceSaveCommand(service);
            var response = PostService(business, json);
            if (response.Payload != null)
                service.Id = ((ServiceData)response.Payload).id;
        }

        private static string CreateNewServiceSaveCommand(ExpectedService expectedService)
        {
            var command = new ApiServiceSaveCommand
            {
                name = expectedService.Name,
                description = expectedService.Description,
                repetition = expectedService.Repetition,
                presentation = expectedService.Presentation
            };

            if (expectedService.Timing != null)
                command.timing = expectedService.Timing;
            if (expectedService.Pricing != null)
                command.pricing = expectedService.Pricing;
            if (expectedService.Booking != null)
                command.booking = expectedService.Booking;

            return JsonSerialiser.Serialise(command);
        }

        private static ApiResponse PostService(ExpectedBusiness business, string json)
        {
            return new TestCoachseekAuthenticatedApiClient(business.UserName, business.Password)
                        .Post<ServiceData>(json, "Services");
        }
    }
}
