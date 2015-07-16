using System.Net;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;

namespace CoachSeek.Api.Tests.Integration
{
    public static class BusinessRegistrar
    {
        public static ApiResponse RegisterBusiness(ExpectedBusiness business, string scheme = "https")
        {
            var json = CreateNewBusinessSaveCommand(business);
            var response = new TestAnonymousApiClient().Post<RegistrationData>(json, "BusinessRegistration", scheme);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var registration = ((RegistrationData)response.Payload);
                business.Id = registration.business.id;
                business.Domain = registration.business.domain;
                business.Payment.currency = registration.business.payment.currency;
            }
            return response;
        }

        private static string CreateNewBusinessSaveCommand(ExpectedBusiness expectedBusiness)
        {
            var command = new ApiBusinessRegistrationCommand
            {
                business = new ApiBusiness { name = expectedBusiness.Name, currency = expectedBusiness.Payment.currency },
                admin = expectedBusiness.Admin
            };

            return JsonSerialiser.Serialise(command);
        }
    }
}
