using System;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Customer
{
    [TestFixture]
    public class OnlineBookingCustomerPostTests : WebIntegrationTest
    {
        private Guid FredId { get; set; }


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
            RegisterCustomerFredFlintstone();
        }

        private void RegisterCustomerFredFlintstone()
        {
            var customer = CreateNewCustomerSaveCommand("Fred", "Flintstone", "fred@flintstones.net", "021 123 123");
            var json = JsonConvert.SerializeObject(customer);
            var response = Post<CustomerData>(json);
            FredId = ((CustomerData)response.Payload).id;
        }

        private ApiCustomerSaveCommand CreateNewCustomerSaveCommand(string firstName, string lastName, string email, string phone)
        {
            return new ApiCustomerSaveCommand
            {
                firstName = firstName,
                lastName = lastName,
                email = email,
                phone = phone
            };
        }

        private ApiCustomerSaveCommand CreateExistingCustomerSaveCommand(Guid id, string firstName, string lastName, string email, string phone)
        {
            var command = CreateNewCustomerSaveCommand(firstName, lastName, email, phone);
            command.id = id;

            return command;
        }


        [Test]
        public void GivenNoCustomerSaveCommand_WhenTryAddOnlineBookCustomer_ThenReturnNoDataError()
        {
            var command = GivenNoCustomerSaveCommand();
            var response = WhenTryAddOnlineBookCustomer(command);
            ThenReturnNoDataError(response);
        }

        [Test]
        public void GivenEmptyCustomerSaveCommand_WhenTryAddOnlineBookCustomer_ThenReturnRootRequiredError()
        {
            var command = GivenEmptyCustomerSaveCommand();
            var response = WhenTryAddOnlineBookCustomer(command);
            ThenReturnRootRequiredError(response);
        }

        [Test]
        public void GivenCustomerIdSpecified_WhenTryAddOnlineBookCustomer_ThenReturnExistingCustomerError()
        {
            var command = GivenCustomerIdSpecified();
            var response = WhenTryAddOnlineBookCustomer(command);
            AssertSingleError(response, "Existing customer used for online booking.");
        }

        [Test]
        public void GivenCustomerMatchesOnEmailAndName_WhenTryAddOnlineBookCustomer_ThenReturnMatchingCustomer()
        {
            var command = GivenCustomerMatchesOnEmailAndName();
            var response = WhenTryAddOnlineBookCustomer(command);
            ThenReturnMatchingCustomer(response);
        }




        //[Test]
        //public void GivenEmptyStringForEmailAddress_WhenPost_ThenReturnInvalidEmailAddressErrorResponse()
        //{
        //    var command = GivenEmptyStringForEmailAddress();
        //    var response = WhenPost(command);
        //    ThenReturnInvalidEmailAddressErrorResponse(response);
        //}

        //[Test]
        //public void GivenEmailIsNotAnEmailAddress_WhenPost_ThenReturnInvalidEmailAddressErrorResponse()
        //{
        //    var command = GivenEmailIsNotAnEmailAddress();
        //    var response = WhenPost(command);
        //    ThenReturnInvalidEmailAddressErrorResponse(response);
        //}

        //[Test]
        //public void GivenValidNewCustomer_WhenPost_ThenReturnNewCustomerResponse()
        //{
        //    var command = GivenValidNewCustomer();
        //    var response = WhenPost(command);
        //    ThenReturnNewCustomerResponse(response);
        //}

        //[Test]
        //public void GivenNonExistentCustomerId_WhenPost_ThenReturnInvalidCustomerIdErrorResponse()
        //{
        //    var command = GivenNonExistentCustomerId();
        //    var response = WhenPost(command);
        //    ThenReturnInvalidCustomerIdErrorResponse(response);
        //}

        //[Test]
        //public void GivenWantToUpdateExistingCustomer_WhenPost_ThenReturnUpdatedCustomerResponse()
        //{
        //    var command = GivenWantToUpdateExistingCustomer();
        //    var response = WhenPost(command);
        //    ThenReturnUpdatedCustomerResponse(response);
        //}


        private string GivenNoCustomerSaveCommand()
        {
            return "";
        }

        private string GivenEmptyCustomerSaveCommand()
        {
            return "{}";
        }

        private ApiCustomerSaveCommand GivenCustomerIdSpecified()
        {
            var command = CreateNewCustomerSaveCommand();
            command.id = Guid.NewGuid();

            return command;
        }

        private ApiCustomerSaveCommand GivenCustomerMatchesOnEmailAndName()
        {
            return new ApiCustomerSaveCommand
            {
                firstName = " fred",
                lastName = "flintstone ",
                email = "Fred@Flintstones.net ",
                phone = " 333 666 ",
            };
        }

        private ApiCustomerSaveCommand GivenEmptyStringForEmailAddress()
        {
            var command = CreateNewCustomerSaveCommand();
            command.email = string.Empty;

            return command;
        }

        private ApiCustomerSaveCommand GivenEmailIsNotAnEmailAddress()
        {
            var command = CreateNewCustomerSaveCommand();
            command.email = "abc123";

            return command;
        }

        private ApiCustomerSaveCommand CreateNewCustomerSaveCommand()
        {
            return new ApiCustomerSaveCommand
            {
                firstName = "Bob",
                lastName = "Saget",
                email = "bob@fullhouse.com",
                phone = "012 3456 7890",
            };
        }

        private string GivenValidNewCustomer()
        {
            return JsonConvert.SerializeObject(CreateNewCustomerSaveCommand());
        }

        private string GivenNonExistentCustomerId()
        {
            var coach = new ApiCustomerSaveCommand
            {
                id = Guid.Empty,
                firstName = Random.RandomString,
                lastName = Random.RandomString,
                email = Random.RandomEmail,
                phone = Random.RandomString,
            };

            return JsonConvert.SerializeObject(coach);
        }

        private ApiCustomerSaveCommand GivenWantToUpdateExistingCustomer()
        {
            return CreateExistingCustomerSaveCommand(FredId, "Barney", "Rubble", "barney@rubbles.net", "09 456 456");
        }




        protected Response WhenTryAddOnlineBookCustomer(ApiCustomerSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);

            return WhenTryAddOnlineBookCustomer(json);
        }

        protected Response WhenTryAddOnlineBookCustomer(string json)
        {
            return PostForOnlineBooking<CustomerData>(json);
        }


        private void ThenReturnNoDataError(Response response)
        {
            AssertSingleError(response, "Please post us some data!");
        }

        private void ThenReturnRootRequiredError(Response response)
        {
            var errors = AssertErrorResponse(response);

            Assert.That(errors.GetLength(0), Is.EqualTo(2));
            AssertApplicationError(errors[0], "customer.firstName", "The firstName field is required.");
            AssertApplicationError(errors[1], "customer.lastName", "The lastName field is required.");
        }

        private void ThenReturnInvalidEmailAddressErrorResponse(Response response)
        {
            var errors = AssertErrorResponse(response);

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "customer.email", "The email address is not valid.");
        }

        private void ThenReturnNewCustomerResponse(Response response)
        {
            var customer = AssertSuccessResponse<CustomerData>(response);

            Assert.That(customer.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(customer.firstName, Is.EqualTo("Bob"));
            Assert.That(customer.lastName, Is.EqualTo("Saget"));
            Assert.That(customer.email, Is.EqualTo("bob@fullhouse.com"));
            Assert.That(customer.phone, Is.EqualTo("012 3456 7890"));
        }

        private void ThenReturnMatchingCustomer(Response response)
        {
            var customer = AssertSuccessResponse<CustomerData>(response);

            Assert.That(customer.id, Is.EqualTo(FredId));
            Assert.That(customer.firstName, Is.EqualTo("Fred"));
            Assert.That(customer.lastName, Is.EqualTo("Flintstone"));
            Assert.That(customer.email, Is.EqualTo("fred@flintstones.net"));
            Assert.That(customer.phone, Is.EqualTo("021 123 123"));
        }

        private void ThenReturnInvalidCustomerIdErrorResponse(Response response)
        {
            var errors = AssertErrorResponse(response);

            Assert.That(errors.GetLength(0), Is.EqualTo(1));
            AssertApplicationError(errors[0], "customer.id", "This customer does not exist.");
        }

        private void ThenReturnUpdatedCustomerResponse(Response response)
        {
            var customer = AssertSuccessResponse<CustomerData>(response);

            Assert.That(customer.id, Is.EqualTo(FredId));
            Assert.That(customer.firstName, Is.EqualTo("Barney"));
            Assert.That(customer.lastName, Is.EqualTo("Rubble"));
            Assert.That(customer.email, Is.EqualTo("barney@rubbles.net"));
            Assert.That(customer.phone, Is.EqualTo("09 456 456"));
        }
    }
}
