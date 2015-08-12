using System;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Customer
{
    public class CustomerPostTests : CustomerTests
    {
        [TestFixture]
        public class CustomerCommandTests : CustomerPostTests
        {
            [Test]
            public void GivenNoCustomerSaveCommand_WhenTryPost_ThenReturnNoDataError()
            {
                var setup = RegisterBusiness();

                var command = GivenNoCustomerSaveCommand();
                var response = WhenTryPost(command, setup);
                ThenReturnNoDataError(response);
            }

            [Test]
            public void GivenEmptyCustomerSaveCommand_WhenTryPost_ThenReturnRootRequiredError()
            {
                var setup = RegisterBusiness();

                var command = GivenEmptyCustomerSaveCommand();
                var response = WhenTryPost(command, setup);
                ThenReturnRootRequiredError(response);
            }


            private string GivenNoCustomerSaveCommand()
            {
                return "";
            }

            private string GivenEmptyCustomerSaveCommand()
            {
                return "{}";
            }
        }


        [TestFixture]
        public class CustomerNewTests : CustomerPostTests
        {
            [Test]
            public void GivenEmptyStringForEmailAddress_WhenTryPost_ThenReturnInvalidEmailAddressError()
            {
                var setup = RegisterBusiness();

                var command = GivenEmptyStringForEmailAddress();
                var response = WhenTryPost(command, setup);
                ThenReturnInvalidEmailAddressError(response);
            }

            [Test]
            public void GivenEmailIsNotAnEmailAddress_WhenTryPost_ThenReturnInvalidEmailAddressError()
            {
                var setup = RegisterBusiness();

                var command = GivenEmailIsNotAnEmailAddress();
                var response = WhenTryPost(command, setup);
                ThenReturnInvalidEmailAddressError(response);
            }

            [Test]
            public void GivenValidNewCustomer_WhenTryPost_ThenReturnNewCustomer()
            {
                var setup = RegisterBusiness();

                var command = GivenValidNewCustomer();
                var response = WhenTryPost(command, setup);
                ThenReturnNewCustomer(response);
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

            private string GivenValidNewCustomer()
            {
                return JsonSerialiser.Serialise(CreateNewCustomerSaveCommand());
            }
        }


        [TestFixture]
        public class CustomerExistingTests : CustomerPostTests
        {
            [Test]
            public void GivenNonExistentCustomerId_WhenTryPost_ThenReturnInvalidCustomerIdError()
            {
                var setup = RegisterBusiness();

                var command = GivenNonExistentCustomerId();
                var response = WhenTryPost(command, setup);
                ThenReturnInvalidCustomerIdError(response, command.id.Value);
            }

            [Test]
            public void GivenWantToUpdateExistingCustomer_WhenTryPost_ThenReturnUpdatedCustomer()
            {
                var setup = RegisterBusiness();
                RegisterCustomerFred(setup);

                var command = GivenWantToUpdateExistingCustomer(setup);
                var response = WhenTryPost(command, setup);
                ThenReturnUpdatedCustomer(response, setup);
            }

            [Test]
            public void GivenWantToUpdateExistingCustomerWithoutEmail_WhenTryPost_ThenReturnUpdatedCustomerWithoutEmail()
            {
                var setup = RegisterBusiness();
                RegisterCustomerFred(setup);

                var command = GivenWantToUpdateExistingCustomerWithoutEmail(setup);
                var response = WhenTryPost(command, setup);
                ThenReturnUpdatedCustomerWithoutEmail(response, setup);
            }

            [Test]
            public void GivenCustomerMatchesOnEmailAndName_WhenTryPost_ThenReturnDuplicateCustomerError()
            {
                var setup = RegisterBusiness();
                RegisterCustomerFred(setup);

                var command = GivenCustomerMatchesOnEmailAndName(setup);
                var response = WhenTryPost(command, setup);
                ThenReturnDuplicateCustomerError(response, setup.Fred.Email);
            }


            private ApiCustomerSaveCommand GivenNonExistentCustomerId()
            {
                return new ApiCustomerSaveCommand
                {
                    id = Guid.NewGuid(),
                    firstName = Random.RandomString,
                    lastName = Random.RandomString,
                    email = Random.RandomEmail,
                    phone = Random.RandomString,
                };
            }

            private ApiCustomerSaveCommand GivenWantToUpdateExistingCustomer(SetupData setup)
            {
                return CreateExistingCustomerSaveCommand(setup.Fred.Id,
                                                         "Barney",
                                                         "Rubble",
                                                         "barney@rubbles.net",
                                                         "09 456 456");
            }

            private ApiCustomerSaveCommand GivenWantToUpdateExistingCustomerWithoutEmail(SetupData setup)
            {
                return CreateExistingCustomerSaveCommand(setup.Fred.Id, "Bam Bam", "Rubble");
            }

            private ApiCustomerSaveCommand GivenCustomerMatchesOnEmailAndName(SetupData setup)
            {
                return CreateNewCustomerSaveCommand("fred ", " flintStone", setup.Fred.Email, "12345");
            }


            private void ThenReturnInvalidCustomerIdError(ApiResponse response, Guid customerId)
            {
                AssertSingleError(response, ErrorCodes.CustomerInvalid, "This customer does not exist.", customerId.ToString());
            }

            private void ThenReturnUpdatedCustomer(ApiResponse response, SetupData setup)
            {
                var customer = AssertSuccessResponse<CustomerData>(response);

                Assert.That(customer.id, Is.EqualTo(setup.Fred.Id));
                Assert.That(customer.firstName, Is.EqualTo("Barney"));
                Assert.That(customer.lastName, Is.EqualTo("Rubble"));
                Assert.That(customer.email, Is.EqualTo("barney@rubbles.net"));
                Assert.That(customer.phone, Is.EqualTo("09 456 456"));
            }

            private void ThenReturnUpdatedCustomerWithoutEmail(ApiResponse response, SetupData setup)
            {
                var customer = AssertSuccessResponse<CustomerData>(response);

                Assert.That(customer.id, Is.EqualTo(setup.Fred.Id));
                Assert.That(customer.firstName, Is.EqualTo("Bam Bam"));
                Assert.That(customer.lastName, Is.EqualTo("Rubble"));
                Assert.That(customer.email, Is.Null);
                Assert.That(customer.phone, Is.Null);
            }

            private void ThenReturnDuplicateCustomerError(ApiResponse response, string email)
            {
                AssertSingleError(response, 
                                  ErrorCodes.CustomerDuplicate,
                                  string.Format("Customer 'fred flintStone' with email '{0}' already exists.", email), 
                                  string.Format("fred flintStone, {0}", email));
            }
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


        private ApiResponse WhenTryPost(ApiCustomerSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryPost(json, setup);
        }

        private ApiResponse WhenTryPost(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<CustomerData>(json,
                                                                       setup.Business.UserName,
                                                                       setup.Business.Password,
                                                                       RelativePath);
        }


        private void ThenReturnNoDataError(ApiResponse response)
        {
            AssertSingleError(response, "Please post us some data!");
        }

        private void ThenReturnRootRequiredError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "The firstName field is required.", "customer.firstName" },
                                                    { "The lastName field is required.", "customer.lastName" } });
        }

        private void ThenReturnInvalidEmailAddressError(ApiResponse response)
        {
            AssertSingleError(response, "The email address is not valid.", "customer.email");
        }

        private void ThenReturnNewCustomer(ApiResponse response)
        {
            var customer = AssertSuccessResponse<CustomerData>(response);

            Assert.That(customer.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(customer.firstName, Is.EqualTo("Bob"));
            Assert.That(customer.lastName, Is.EqualTo("Saget"));
            Assert.That(customer.email, Is.EqualTo("bob@fullhouse.com"));
            Assert.That(customer.phone, Is.EqualTo("012 3456 7890"));
        }
    }
}
