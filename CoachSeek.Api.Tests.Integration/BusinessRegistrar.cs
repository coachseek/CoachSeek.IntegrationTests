using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using Newtonsoft.Json;

namespace CoachSeek.Api.Tests.Integration
{
    public static class BusinessRegistrar
    {


        public static Response RegisterBusiness(ExpectedBusiness business)
        {
            var json = CreateNewBusinessSaveCommand(business);
            var response = WebClient.AnonymousPost<RegistrationData>(json, "BusinessRegistration");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var registration = ((RegistrationData)response.Payload);
                business.Id = registration.business.id;
                business.Domain = registration.business.domain;
            }
            return response;
        }

        private static string CreateNewBusinessSaveCommand(ExpectedBusiness expectedBusiness)
        {
            var registration = new ApiBusinessRegistrationCommand
            {
                business = new ApiBusiness { name = expectedBusiness.Name },
                admin = expectedBusiness.Admin
            };

            return JsonConvert.SerializeObject(registration);
        }
    }
}
