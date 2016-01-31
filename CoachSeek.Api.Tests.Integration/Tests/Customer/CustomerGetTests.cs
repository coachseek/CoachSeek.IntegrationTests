using System;
using System.Collections.Generic;
using System.Net;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Customer
{
    [TestFixture]
    public class CustomerGetTests : CustomerTests
    {
        [Test]
        public void WhenTryGetAllCustomers_ThenReturnAllCustomers()
        {
            var setup = RegisterBusiness();
            RegisterTestCustomers(setup);

            var response = WhenTryGetAllCustomers(setup);
            ThenReturnAllCustomers(response, setup);
        }

        [Test]
        public void GivenInvalidCustomerId_WhenTryGetCustomerById_ThenReturnNotFound()
        {
            var setup = RegisterBusiness();

            var id = GivenInvalidCustomerId();
            var response = WhenTryGetCustomerById(id, setup);
            ThenReturnNotFound(response);
        }

        [Test]
        public void GivenValidCustomerId_WhenTryGetCustomerById_ThenReturnCustomer()
        {
            var setup = RegisterBusiness();
            RegisterCustomerFred(setup);

            var id = GivenValidCustomerId(setup);
            var response = WhenTryGetCustomerById(id, setup);
            ThenReturnCustomer(response, setup);
        }

        [Test]
        public void GivenCustomerCustomFields_WhenTryGetCustomerById_ThenReturnCustomerWithBlankCustomFields()
        {
            var setup = RegisterBusiness();
            CreateCustomFields(setup);
            RegisterCustomerFred(setup);

            var id = GivenValidCustomerId(setup);
            var response = WhenTryGetCustomerById(id, setup);
            ThenReturnCustomer(response, setup);
        }


        private void CreateCustomFields(SetupData setup)
        {
            CreateCustomerMedicalInfoCustomField(setup);
            CreateCustomerSchoolYearCustomField(setup);
        }

        private void CreateCustomerMedicalInfoCustomField(SetupData setup)
        {
            var command = CreateNewCustomFieldSaveCommand("Customer", "Medical Info", true);
            var response = CreateNewCustomField(command, setup);
            setup.CustomFields.Add((CustomFieldTemplateData)response.Payload);
        }

        private void CreateCustomerSchoolYearCustomField(SetupData setup)
        {
            var command = CreateNewCustomFieldSaveCommand("customer", "School Year");
            var response = CreateNewCustomField(command, setup);
            setup.CustomFields.Add((CustomFieldTemplateData)response.Payload);
        }

        private ApiCustomFieldSaveCommand CreateNewCustomFieldSaveCommand(string type, string name, bool isRequired = false)
        {
            return new ApiCustomFieldSaveCommand
            {
                type = type,
                name = name,
                isRequired = isRequired
            };
        }

        private ApiResponse CreateNewCustomField(ApiCustomFieldSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return AuthenticatedPost<CustomFieldTemplateData>(json, "CustomFields", setup);
        }


        private Guid GivenInvalidCustomerId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidCustomerId(SetupData setup)
        {
            return setup.Fred.Id;
        }


        private ApiResponse WhenTryGetAllCustomers(SetupData setup)
        {
            return AuthenticatedGet<List<CustomerData>>(RelativePath, setup); 
        }

        private ApiResponse WhenTryGetCustomerById(Guid customerId, SetupData setup)
        {
            return AuthenticatedGet<CustomerData>(RelativePath, customerId, setup);
        }


        private void ThenReturnAllCustomers(ApiResponse response, SetupData setup)
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Payload, Is.Not.Null);
            var customers = (List<CustomerData>)response.Payload;
            Assert.That(customers.Count, Is.EqualTo(4));

            setup.Fred.Assert(customers[0]);
            setup.Wilma.Assert(customers[1]);
            setup.BamBam.Assert(customers[2]);
            setup.Barney.Assert(customers[3]);
        }

        private void ThenReturnCustomer(ApiResponse response, SetupData setup)
        {
            var customer = AssertSuccessResponse<CustomerData>(response);
            setup.Fred.Assert(customer);
        }
    }
}
