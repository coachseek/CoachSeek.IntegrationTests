using System;
using System.Collections.Generic;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Customer
{
    [TestFixture]
    public class CustomerGetTests : WebIntegrationTest
    {
        private const string FRED_FIRST_NAME = "Fred";
        private const string WILMA_FIRST_NAME = "Wilma";
        private const string FLINTSTONE_LAST_NAME = "Flintstone";

        private Guid FredId { get; set; }
        private Guid WilmaId { get; set; }
        private string FredEmail { get; set; }
        private string FredPhone { get; set; }

        protected override string RelativePath
        {
            get { return "Customers"; }
        }


        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            RegisterTestCustomers();
        }

        private void RegisterTestCustomers()
        {
            RegisterFredCustomer();
            RegisterWilmaCustomer();
        }

        private void RegisterFredCustomer()
        {
            FredEmail = RandomEmail;
            FredPhone = RandomString;
            var json = CreateNewCustomerSaveCommand(FRED_FIRST_NAME, FLINTSTONE_LAST_NAME, FredEmail, FredPhone);
            var response = Post<CustomerData>(json);
            FredId = ((CustomerData)response.Payload).id;
        }

        private void RegisterWilmaCustomer()
        {
            var json = CreateNewCustomerSaveCommand(WILMA_FIRST_NAME, FLINTSTONE_LAST_NAME, "wilma@flintstones.net", "2");
            var response = Post<CustomerData>(json);
            WilmaId = ((CustomerData)response.Payload).id;
        }


        private string CreateNewCustomerSaveCommand(string firstName, string lastName, string email, string phone)
        {
            var coach = new ApiCustomerSaveCommand
            {
                firstName = firstName,
                lastName = lastName,
                email = email,
                phone = phone
            };

            return JsonConvert.SerializeObject(coach);
        }


        [Test]
        public void GivenInvalidCustomerId_WhenGetById_ThenReturnNotFoundResponse()
        {
            var id = GivenInvalidCustomerId();
            var response = WhenGetById(id);
            ThenReturnNotFoundResponse(response);
        }

        [Test]
        public void GivenValidCustomerId_WhenGetById_ThenReturnCustomerResponse()
        {
            var id = GivenValidCustomerId();
            var response = WhenGetById(id);
            ThenReturnCustomerResponse(response);
        }


        private Guid GivenInvalidCustomerId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidCustomerId()
        {
            return FredId;
        }


        private Response WhenGetAll()
        {
            var url = BuildGetAllUrl();
            return Get<List<CoachData>>(url);
        }

        private Response WhenGetById(Guid customerId)
        {
            var url = BuildGetByIdUrl(customerId);
            return Get<CustomerData>(url);
        }


        private void ThenReturnNotFoundResponse(Response response)
        {
            AssertNotFound(response);
        }

        private void ThenReturnCustomerResponse(Response response)
        {
            var customer = AssertSuccessResponse<CustomerData>(response);

            Assert.That(customer.id, Is.EqualTo(FredId));
            Assert.That(customer.firstName, Is.EqualTo(FRED_FIRST_NAME));
            Assert.That(customer.lastName, Is.EqualTo(FLINTSTONE_LAST_NAME));
            Assert.That(customer.email, Is.EqualTo(FredEmail));
            Assert.That(customer.phone, Is.EqualTo(FredPhone.ToUpperInvariant()));
        }
    }
}
