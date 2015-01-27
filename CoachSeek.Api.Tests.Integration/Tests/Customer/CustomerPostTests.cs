using System;
using System.Net;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Customer
{
    [TestFixture]
    public class CustomerPostTests : WebIntegrationTest
    {
        protected override string RelativePath
        {
            get { return "Customers"; }
        }


        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
            //RegisterTestCustomers();
        }


        [Test]
        public void GivenNoCustomerSaveCommand_WhenPost_ThenReturnNoDataErrorResponse()
        {
            var command = GivenNoCustomerSaveCommand();
            var response = WhenPost(command);
            ThenReturnNoDataErrorResponse(response);
        }

        [Test]
        public void GivenEmptyCustomerSaveCommand_WhenPost_ThenReturnRootRequiredErrorResponse()
        {
            var command = GivenEmptyCustomerSaveCommand();
            var response = WhenPost(command);
            ThenReturnRootRequiredErrorResponse(response);
        }

        [Test]
        public void GivenValidNewCustomer_WhenPost_ThenReturnNewCustomerResponse()
        {
            var command = GivenValidNewCustomer();
            var response = WhenPost(command);
            ThenReturnNewCustomerResponse(response);
        }



        [Test]
        public void GivenNonExistentCustomerId_WhenPost_ThenReturnInvalidCustomerIdErrorResponse()
        {
            var command = GivenNonExistentCustomerId();
            var response = WhenPost(command);
            ThenReturnInvalidCustomerIdErrorResponse(response);
        }


        private string GivenNoCustomerSaveCommand()
        {
            return "";
        }

        private string GivenEmptyCustomerSaveCommand()
        {
            return "{}";
        }

        private string GivenValidNewCustomer()
        {
            var customer = new ApiCustomerSaveCommand
            {
                firstName = "Bob",
                lastName = "Saget",
                email = "bob@fullhouse.com",
                phone = "012 3456 7890",
            };

            return JsonConvert.SerializeObject(customer);
        }



        private string GivenNonExistentCustomerId()
        {
            var coach = new ApiCustomerSaveCommand
            {
                id = Guid.Empty,
                firstName = RandomString,
                lastName = RandomString,
                email = RandomEmail,
                phone = RandomString,
            };

            return JsonConvert.SerializeObject(coach);
        }


        private Response WhenPost(string json)
        {
            return Post<CustomerData>(json);
        }


        private void ThenReturnNoDataErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], null, "Please post us some data!");
        }

        private void ThenReturnRootRequiredErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;
            Assert.That(errors.GetLength(0), Is.EqualTo(2));
            AssertApplicationError(errors[0], "customer.firstName", "The firstName field is required.");
            AssertApplicationError(errors[1], "customer.lastName", "The lastName field is required.");
        }

        private void ThenReturnNewCustomerResponse(Response response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<CustomerData>());
            var customer = (CustomerData)response.Payload;

            Assert.That(customer.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(customer.firstName, Is.EqualTo("Bob"));
            Assert.That(customer.lastName, Is.EqualTo("Saget"));
            Assert.That(customer.email, Is.EqualTo("bob@fullhouse.com"));
            Assert.That(customer.phone, Is.EqualTo("012 3456 7890"));
        }



        private void ThenReturnInvalidCustomerIdErrorResponse(Response response)
        {
            AssertStatusCode(response.StatusCode, HttpStatusCode.BadRequest);

            Assert.That(response.Payload, Is.InstanceOf<ApplicationError[]>());
            var errors = (ApplicationError[])response.Payload;

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "customer.id", "This customer does not exist.");
        }
    }
}
