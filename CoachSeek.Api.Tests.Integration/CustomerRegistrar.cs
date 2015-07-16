using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;

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
            var command = new ApiCustomerSaveCommand
            {
                firstName = expectedCustomer.FirstName,
                lastName = expectedCustomer.LastName,
                email = expectedCustomer.Email,
                phone = expectedCustomer.Phone
            };

            return JsonSerialiser.Serialise(command);
        }

        private static ApiResponse PostCustomer(ExpectedBusiness business, string json)
        {
            return new TestAuthenticatedApiClient().Post<CustomerData>(json, business.UserName, business.Password, "Customers");
        }
    }
}
