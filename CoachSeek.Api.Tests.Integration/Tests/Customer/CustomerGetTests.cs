using System;
using System.Collections.Generic;
using System.Net;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
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
            return new TestAuthenticatedApiClient().Get<List<CustomerData>>(setup.Business.UserName,
                                                                            setup.Business.Password,
                                                                            RelativePath);
        }

        private ApiResponse WhenTryGetCustomerById(Guid customerId, SetupData setup)
        {
            var url = string.Format("{0}/{1}", RelativePath, customerId);
            return new TestAuthenticatedApiClient().Get<CustomerData>(setup.Business.UserName,
                                                                      setup.Business.Password,
                                                                      url);
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
