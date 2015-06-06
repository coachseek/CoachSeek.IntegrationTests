using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using Newtonsoft.Json;

namespace CoachSeek.Api.Tests.Integration
{
    public static class CustomerRegistrar
    {
        public static void RegisterCustomer(ExpectedCustomer customer, ExpectedBusiness business)
        {
            var json = CreateNewCustomerSaveCommand(customer);
            var response = PostCustomer(business, json);
            if (response.Payload != null)
                customer.Id = ((CustomerData)response.Payload).id;
        }

        private static string CreateNewCustomerSaveCommand(ExpectedCustomer expectedCustomer)
        {
            var customer = new ApiCustomerSaveCommand
            {
                firstName = expectedCustomer.FirstName,
                lastName = expectedCustomer.LastName,
                email = expectedCustomer.Email,
                phone = expectedCustomer.Phone
            };

            return JsonConvert.SerializeObject(customer);
        }

        private static ApiResponse PostCustomer(ExpectedBusiness business, string json)
        {
            return new TestAuthenticatedApiClient().Post<CustomerData>(json, business.UserName, business.Password, "Customers");
        }
    }
}
