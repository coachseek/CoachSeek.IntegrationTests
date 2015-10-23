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
            var response = PostBusinessRegistration(json, scheme);
            if (response.StatusCode == HttpStatusCode.OK)
                UpdateBusiness(business, response);
            return response;
        }

        private static string CreateNewBusinessSaveCommand(ExpectedBusiness expectedBusiness)
        {
            var command = new ApiBusinessRegistrationCommand
            {
                business = new ApiBusiness 
                { 
                    name = expectedBusiness.Name,
                    sport = expectedBusiness.Sport, 
                    currency = expectedBusiness.Payment.currency
                },
                admin = expectedBusiness.Admin
            };

            return JsonSerialiser.Serialise(command);
        }

        private static ApiResponse PostBusinessRegistration(string json, string scheme = "https")
        {
            return new TestCoachseekAnonymousApiClient(scheme)
                .PostAsync<RegistrationData, ApiApplicationError[]>(json, "BusinessRegistration").Result;
        }

        private static void UpdateBusiness(ExpectedBusiness business, ApiResponse response)
        {
            var registration = ((RegistrationData)response.Payload);
            business.Id = registration.business.id;
            business.Domain = registration.business.domain;
            business.Payment.currency = registration.business.payment.currency;
        }
    }
}
