using System;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Customer
{
    [TestFixture]
    public class CustomerPostTests : WebIntegrationTest
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

        private ApiCustomerSaveCommand CreateExistingCustomerSaveCommand(Guid id, string firstName, string lastName, string email = null, string phone = null)
        {
            var command = CreateNewCustomerSaveCommand(firstName, lastName, email, phone);
            command.id = id;

            return command;
        }


        [Test]
        public void GivenNoCustomerSaveCommand_WhenTryPost_ThenReturnNoDataErrorResponse()
        {
            var command = GivenNoCustomerSaveCommand();
            var response = WhenTryPost(command);
            ThenReturnNoDataErrorResponse(response);
        }

        [Test]
        public void GivenEmptyCustomerSaveCommand_WhenTryPost_ThenReturnRootRequiredErrorResponse()
        {
            var command = GivenEmptyCustomerSaveCommand();
            var response = WhenTryPost(command);
            ThenReturnRootRequiredErrorResponse(response);
        }

        [Test]
        public void GivenEmptyStringForEmailAddress_WhenTryPost_ThenReturnInvalidEmailAddressErrorResponse()
        {
            var command = GivenEmptyStringForEmailAddress();
            var response = WhenTryPost(command);
            ThenReturnInvalidEmailAddressErrorResponse(response);
        }

        [Test]
        public void GivenEmailIsNotAnEmailAddress_WhenTryPost_ThenReturnInvalidEmailAddressErrorResponse()
        {
            var command = GivenEmailIsNotAnEmailAddress();
            var response = WhenTryPost(command);
            ThenReturnInvalidEmailAddressErrorResponse(response);
        }

        [Test]
        public void GivenValidNewCustomer_WhenTryPost_ThenReturnNewCustomerResponse()
        {
            var command = GivenValidNewCustomer();
            var response = WhenTryPost(command);
            ThenReturnNewCustomerResponse(response);
        }

        [Test]
        public void GivenNonExistentCustomerId_WhenTryPost_ThenReturnInvalidCustomerIdErrorResponse()
        {
            var command = GivenNonExistentCustomerId();
            var response = WhenTryPost(command);
            ThenReturnInvalidCustomerIdErrorResponse(response);
        }

        [Test]
        public void GivenWantToUpdateExistingCustomer_WhenTryPost_ThenReturnUpdatedCustomerResponse()
        {
            var command = GivenWantToUpdateExistingCustomer();
            var response = WhenTryPost(command);
            ThenReturnUpdatedCustomerResponse(response);
        }

        [Test]
        public void GivenWantToUpdateExistingCustomerWithoutEmail_WhenTryPost_ThenReturnUpdatedCustomerWithoutEmail()
        {
            var command = GivenWantToUpdateExistingCustomerWithoutEmail();
            var response = WhenTryPost(command);
            ThenReturnUpdatedCustomerWithoutEmail(response);
        }

        [Test]
        public void GivenCustomerMatchesOnEmailAndName_WhenTryPost_ThenReturnDuplicateCustomerError()
        {
            var command = GivenCustomerMatchesOnEmailAndName();
            var response = WhenTryPost(command);
            ThenReturnDuplicateCustomerError(response);
        }


        private string GivenNoCustomerSaveCommand()
        {
            return "";
        }

        private string GivenEmptyCustomerSaveCommand()
        {
            return "{}";
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

        private ApiCustomerSaveCommand GivenWantToUpdateExistingCustomerWithoutEmail()
        {
            return CreateExistingCustomerSaveCommand(FredId, "Bam Bam", "Rubble");
        }

        private ApiCustomerSaveCommand GivenCustomerMatchesOnEmailAndName()
        {
            return CreateNewCustomerSaveCommand("fred ", " flintStone", " Fred@Flintstones.net ", "12345");
        }


        private Response WhenTryPost(string json)
        {
            return Post<CustomerData>(json);
        }

        private Response WhenTryPost(ApiCustomerSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);
            return Post<CustomerData>(json);
        }


        private void ThenReturnNoDataErrorResponse(Response response)
        {
            AssertSingleError(response, "Please post us some data!");
        }

        private void ThenReturnRootRequiredErrorResponse(Response response)
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

        private void ThenReturnUpdatedCustomerWithoutEmail(Response response)
        {
            var customer = AssertSuccessResponse<CustomerData>(response);

            Assert.That(customer.id, Is.EqualTo(FredId));
            Assert.That(customer.firstName, Is.EqualTo("Bam Bam"));
            Assert.That(customer.lastName, Is.EqualTo("Rubble"));
            Assert.That(customer.email, Is.Null);
            Assert.That(customer.phone, Is.Null);
        }

        private void ThenReturnDuplicateCustomerError(Response response)
        {
            AssertSingleError(response, "This customer already exists.");
        }
    }
}
