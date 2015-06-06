using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Service;
using Newtonsoft.Json;

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
            var service = new ApiServiceSaveCommand
            {
                name = expectedService.Name,
                description = expectedService.Description,
                repetition = expectedService.Repetition,
                presentation = expectedService.Presentation
            };

            if (expectedService.Timing != null)
                service.timing = expectedService.Timing;
            if (expectedService.Pricing != null)
                service.pricing = expectedService.Pricing;
            if (expectedService.Booking != null)
                service.booking = expectedService.Booking;

            return JsonConvert.SerializeObject(service);
        }

        private static ApiResponse PostService(ExpectedBusiness business, string json)
        {
            return new TestAuthenticatedApiClient().Post<ServiceData>(json, business.UserName, business.Password, "Services");
        }
    }
}
