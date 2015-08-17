using System;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Customer
{
    [TestFixture]
    public class OnlineBookingCustomerPostTests : CustomerTests
    {
        private ApiCustomerSaveCommand CreateNewCustomerSaveCommand(string firstName, 
                                                                    string lastName, 
                                                                    string email, 
                                                                    string phone)
        {
            return new ApiCustomerSaveCommand
            {
                firstName = firstName,
                lastName = lastName,
                email = email,
                phone = phone
            };
        }


        [Test]
        public void GivenNoCustomerSaveCommand_WhenTryAddOnlineBookCustomer_ThenReturnNoDataError()
        {
            var setup = RegisterBusiness();

            var command = GivenNoCustomerSaveCommand();
            var response = WhenTryAddOnlineBookCustomer(command, setup);
            ThenReturnNoDataError(response);
        }

        [Test]
        public void GivenEmptyCustomerSaveCommand_WhenTryAddOnlineBookCustomer_ThenReturnRootRequiredError()
        {
            var setup = RegisterBusiness();

            var command = GivenEmptyCustomerSaveCommand();
            var response = WhenTryAddOnlineBookCustomer(command, setup);
            ThenReturnRootRequiredError(response);
        }

        [Test]
        public void GivenCustomerIdSpecified_WhenTryAddOnlineBookCustomer_ThenReturnExistingCustomerError()
        {
            var setup = RegisterBusiness();

            var command = GivenCustomerIdSpecified();
            var response = WhenTryAddOnlineBookCustomer(command, setup);
            ThenReturnExistingCustomerError(response);
        }

        [Test]
        public void GivenCustomerMatchesOnEmailAndName_WhenTryAddOnlineBookCustomer_ThenReturnMatchingCustomer()
        {
            var setup = RegisterBusiness();
            RegisterCustomerFred(setup);

            var command = GivenCustomerMatchesOnEmailAndName(setup);
            var response = WhenTryAddOnlineBookCustomer(command, setup);
            ThenReturnMatchingCustomer(response, setup);
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

        [Test]
        public void GivenValidNewCustomer_WhenTryAddOnlineBookCustomer_ThenReturnNewCustomerResponse()
        {
            var setup = RegisterBusiness();

            var command = GivenValidNewCustomer();
            var response = WhenTryAddOnlineBookCustomer(command, setup);
            ThenReturnNewCustomerResponse(response);
        }

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

        private ApiCustomerSaveCommand GivenCustomerMatchesOnEmailAndName(SetupData setup)
        {
            return new ApiCustomerSaveCommand
            {
                firstName = " fred",
                lastName = "flintstone ",
                email = setup.Fred.Email,
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
            return JsonSerialiser.Serialise(CreateNewCustomerSaveCommand());
        }

        private string GivenNonExistentCustomerId()
        {
            var command = new ApiCustomerSaveCommand
            {
                id = Guid.Empty,
                firstName = Random.RandomString,
                lastName = Random.RandomString,
                email = Random.RandomEmail,
                phone = Random.RandomString,
            };

            return JsonSerialiser.Serialise(command);
        }


        protected ApiResponse WhenTryAddOnlineBookCustomer(ApiCustomerSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryAddOnlineBookCustomer(json, setup);
        }

        protected ApiResponse WhenTryAddOnlineBookCustomer(string json, SetupData setup)
        {
            return BusinessAnonymousPost<CustomerData>(json, "OnlineBooking/Customers", setup);
        }


        private void ThenReturnNoDataError(ApiResponse response)
        {
            AssertSingleError(response, "Please post us some data!");
        }

        private void ThenReturnRootRequiredError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "firstname-required", "The FirstName field is required.", null, null },
                                                    { "lastname-required", "The LastName field is required.", null, null } });
        }

        private void ThenReturnExistingCustomerError(ApiResponse response)
        {
            AssertSingleError(response,
                              ErrorCodes.UseExistingCustomerForOnlineBookingNotSupported,
                              "Using an existing customer for online booking is not supported.",
                              null);
        }

        private void ThenReturnNewCustomerResponse(ApiResponse response)
        {
            var customer = AssertSuccessResponse<CustomerData>(response);

            Assert.That(customer.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(customer.firstName, Is.EqualTo("Bob"));
            Assert.That(customer.lastName, Is.EqualTo("Saget"));
            Assert.That(customer.email, Is.EqualTo("bob@fullhouse.com"));
            Assert.That(customer.phone, Is.EqualTo("012 3456 7890"));
        }

        private void ThenReturnMatchingCustomer(ApiResponse response, SetupData setup)
        {
            var customer = AssertSuccessResponse<CustomerData>(response);

            Assert.That(customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(customer.firstName, Is.EqualTo(setup.Fred.FirstName));
            Assert.That(customer.lastName, Is.EqualTo(setup.Fred.LastName));
            Assert.That(customer.email, Is.EqualTo(setup.Fred.Email));
            Assert.That(customer.phone, Is.EqualTo(setup.Fred.Phone));
        }

        //private void ThenReturnUpdatedCustomerResponse(Response response)
        //{
        //    var customer = AssertSuccessResponse<CustomerData>(response);

        //    Assert.That(customer.id, Is.EqualTo(FredId));
        //    Assert.That(customer.firstName, Is.EqualTo("Barney"));
        //    Assert.That(customer.lastName, Is.EqualTo("Rubble"));
        //    Assert.That(customer.email, Is.EqualTo("barney@rubbles.net"));
        //    Assert.That(customer.phone, Is.EqualTo("09 456 456"));
        //}
    }
}
