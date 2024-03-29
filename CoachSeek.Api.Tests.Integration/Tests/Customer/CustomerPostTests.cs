﻿using System;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
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
            public void GivenEmptyStringForEmailAddress_WhenTryCreateNewCustomer_ThenReturnInvalidEmailAddressError()
            {
                var setup = RegisterBusiness();

                var command = GivenEmptyStringForEmailAddress();
                var response = WhenTryCreateNewCustomer(command, setup);
                ThenReturnInvalidEmailAddressError(response);
            }

            [Test]
            public void GivenEmailIsNotAnEmailAddress_WhenTryCreateNewCustomer_ThenReturnInvalidEmailAddressError()
            {
                var setup = RegisterBusiness();

                var command = GivenEmailIsNotAnEmailAddress();
                var response = WhenTryCreateNewCustomer(command, setup);
                ThenReturnInvalidEmailAddressError(response);
            }

            [Test]
            public void GivenValidNewCustomer_WhenTryCreateNewCustomer_ThenReturnNewCustomer()
            {
                var setup = RegisterBusiness();

                var command = GivenValidNewCustomer();
                var response = WhenTryCreateNewCustomer(command, setup);
                ThenReturnNewCustomer(response, setup);
            }

            [Test]
            public void GivenValidNewCustomerWithDateOfBirth_WhenTryCreateNewCustomer_ThenReturnNewCustomerWithDateOfBirth()
            {
                var setup = RegisterBusiness();

                var command = GivenValidNewCustomerWithDateOfBirth();
                var response = WhenTryCreateNewCustomer(command, setup);
                ThenReturnNewCustomerWithDateOfBirth(response, setup);
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

            private ApiCustomerSaveCommand GivenValidNewCustomer()
            {
                return CreateNewCustomerSaveCommand();
            }

            private ApiCustomerSaveCommand GivenValidNewCustomerWithDateOfBirth()
            {
                var command = CreateNewCustomerSaveCommand();
                command.dateOfBirth = "1956-05-17";
                return command;
            }

            private ApiResponse WhenTryCreateNewCustomer(ApiCustomerSaveCommand command, SetupData setup)
            {
                var json = JsonSerialiser.Serialise(command);
                return WhenTryPost(json, setup);
            }
        }


        [TestFixture]
        public class CustomerExistingTests : CustomerPostTests
        {
            [Test]
            public void GivenNonExistentCustomerId_WhenTryUpdateExistingCustomer_ThenReturnInvalidCustomerIdError()
            {
                var setup = RegisterBusiness();

                var command = GivenNonExistentCustomerId();
                var response = WhenTryUpdateExistingCustomer(command, setup);
                ThenReturnInvalidCustomerIdError(response, command.id.Value);
            }

            [Test]
            public void GivenWantToUpdateExistingCustomer_WhenTryUpdateExistingCustomer_ThenReturnUpdatedCustomer()
            {
                var setup = RegisterBusiness();
                RegisterCustomerFred(setup);

                var command = GivenWantToUpdateExistingCustomer(setup);
                var response = WhenTryUpdateExistingCustomer(command, setup);
                ThenReturnUpdatedCustomer(response, setup);
            }

            [Test]
            public void GivenWantToUpdateExistingCustomerWithoutEmail_WhenTryUpdateExistingCustomer_ThenReturnUpdatedCustomerWithoutEmail()
            {
                var setup = RegisterBusiness();
                RegisterCustomerFred(setup);

                var command = GivenWantToUpdateExistingCustomerWithoutEmail(setup);
                var response = WhenTryUpdateExistingCustomer(command, setup);
                ThenReturnUpdatedCustomerWithoutEmail(response, setup);
            }

            [Test]
            public void GivenCustomerMatchesOnEmailAndName_WhenTryUpdateExistingCustomer_ThenReturnDuplicateCustomerError()
            {
                var setup = RegisterBusiness();
                RegisterCustomerFred(setup);

                var command = GivenCustomerMatchesOnEmailAndName(setup);
                var response = WhenTryUpdateExistingCustomer(command, setup);
                ThenReturnDuplicateCustomerError(response, setup.Fred.Email);
            }

            [Test]
            public void 
                GivenWantToUpdateExistingCustomerWithDateOfBirth_WhenTryUpdateExistingCustomer_ThenReturnUpdatedCustomerWithDateOfBirth()
            {
                var setup = RegisterBusiness();
                RegisterCustomerFred(setup);

                var command = GivenWantToUpdateExistingCustomerWithDateOfBirth(setup);
                var response = WhenTryUpdateExistingCustomer(command, setup);
                ThenReturnUpdatedCustomerWithDateOfBirth(response, setup);
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

            private ApiCustomerSaveCommand GivenWantToUpdateExistingCustomerWithDateOfBirth(SetupData setup)
            {
                return CreateExistingCustomerSaveCommand(setup.Fred.Id,
                                                         "Barney",
                                                         "Rubble",
                                                         "barney@rubbles.net",
                                                         "09 456 456",
                                                         "1968-03-13");
            }


            private ApiResponse WhenTryUpdateExistingCustomer(ApiCustomerSaveCommand command, SetupData setup)
            {
                var json = JsonSerialiser.Serialise(command);
                return WhenTryPost(json, setup);
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
                Assert.That(customer.dateOfBirth, Is.Null);

                var getResponse = AuthenticatedGet<CustomerData>(RelativePath, customer.id, setup);
                var getCustomer = (CustomerData)getResponse.Payload;

                Assert.That(getCustomer.id, Is.EqualTo(customer.id));
                Assert.That(getCustomer.firstName, Is.EqualTo(customer.firstName));
                Assert.That(getCustomer.lastName, Is.EqualTo(customer.lastName));
                Assert.That(getCustomer.email, Is.EqualTo(customer.email));
                Assert.That(getCustomer.phone, Is.EqualTo(customer.phone));
                Assert.That(getCustomer.dateOfBirth, Is.EqualTo(customer.dateOfBirth));
            }

            private void ThenReturnUpdatedCustomerWithDateOfBirth(ApiResponse response, SetupData setup)
            {
                var customer = AssertSuccessResponse<CustomerData>(response);

                Assert.That(customer.id, Is.EqualTo(setup.Fred.Id));
                Assert.That(customer.firstName, Is.EqualTo("Barney"));
                Assert.That(customer.lastName, Is.EqualTo("Rubble"));
                Assert.That(customer.email, Is.EqualTo("barney@rubbles.net"));
                Assert.That(customer.phone, Is.EqualTo("09 456 456"));
                Assert.That(customer.dateOfBirth, Is.EqualTo("1968-03-13"));

                var getResponse = AuthenticatedGet<CustomerData>(RelativePath, customer.id, setup);
                var getCustomer = (CustomerData)getResponse.Payload;

                Assert.That(getCustomer.id, Is.EqualTo(customer.id));
                Assert.That(getCustomer.firstName, Is.EqualTo(customer.firstName));
                Assert.That(getCustomer.lastName, Is.EqualTo(customer.lastName));
                Assert.That(getCustomer.email, Is.EqualTo(customer.email));
                Assert.That(getCustomer.phone, Is.EqualTo(customer.phone));
                Assert.That(getCustomer.dateOfBirth, Is.EqualTo(customer.dateOfBirth));
            }


            private void ThenReturnDuplicateCustomerError(ApiResponse response, string email)
            {
                AssertSingleError(response, 
                                  ErrorCodes.CustomerDuplicate,
                                  string.Format("Customer 'fred flintStone' with email '{0}' already exists.", email), 
                                  string.Format("fred flintStone, {0}", email));
            }
        }


        private ApiCustomerSaveCommand CreateNewCustomerSaveCommand(string firstName, 
                                                                    string lastName, 
                                                                    string email, 
                                                                    string phone, 
                                                                    string dateOfBirth = null)
        {
            return new ApiCustomerSaveCommand
            {
                firstName = firstName,
                lastName = lastName,
                email = email,
                phone = phone,
                dateOfBirth = dateOfBirth
            };
        }

        private ApiCustomerSaveCommand CreateExistingCustomerSaveCommand(Guid id, 
                                                                         string firstName, 
                                                                         string lastName, 
                                                                         string email = null,
                                                                         string phone = null, 
                                                                         string dateOfBirth = null)
        {
            var command = CreateNewCustomerSaveCommand(firstName, lastName, email, phone, dateOfBirth);
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
            return AuthenticatedPost<CustomerData>(json, RelativePath, setup);
        }

        private ApiResponse WhenTryPostAnonymously(string json, SetupData setup)
        {
            return BusinessAnonymousPost<CustomerData>(json, "OnlineBooking/Customers", setup);
        }


        private void ThenReturnNoDataError(ApiResponse response)
        {
            AssertSingleError(response, ErrorCodes.DataRequired, "Please post us some data!");
        }

        private void ThenReturnRootRequiredError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "firstname-required", "The FirstName field is required.", null },
                                                    { "lastname-required", "The LastName field is required.", null } });
        }

        private void ThenReturnInvalidEmailAddressError(ApiResponse response)
        {
            AssertSingleError(response, "email-invalid", "The Email field is not a valid e-mail address.", null);
        }

        private void ThenReturnNewCustomer(ApiResponse response, SetupData setup)
        {
            var customer = AssertSuccessResponse<CustomerData>(response);

            Assert.That(customer.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(customer.firstName, Is.EqualTo("Bob"));
            Assert.That(customer.lastName, Is.EqualTo("Saget"));
            Assert.That(customer.email, Is.EqualTo("bob@fullhouse.com"));
            Assert.That(customer.phone, Is.EqualTo("012 3456 7890"));
            Assert.That(customer.dateOfBirth, Is.Null);

            var getResponse = AuthenticatedGet<CustomerData>(RelativePath, customer.id, setup);
            var getCustomer = (CustomerData)getResponse.Payload;

            Assert.That(getCustomer.id, Is.EqualTo(customer.id));
            Assert.That(getCustomer.firstName, Is.EqualTo(customer.firstName));
            Assert.That(getCustomer.lastName, Is.EqualTo(customer.lastName));
            Assert.That(getCustomer.email, Is.EqualTo(customer.email));
            Assert.That(getCustomer.phone, Is.EqualTo(customer.phone));
            Assert.That(getCustomer.dateOfBirth, Is.EqualTo(customer.dateOfBirth));
        }

        private void ThenReturnNewCustomerWithDateOfBirth(ApiResponse response, SetupData setup)
        {
            var customer = AssertSuccessResponse<CustomerData>(response);

            Assert.That(customer.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(customer.firstName, Is.EqualTo("Bob"));
            Assert.That(customer.lastName, Is.EqualTo("Saget"));
            Assert.That(customer.email, Is.EqualTo("bob@fullhouse.com"));
            Assert.That(customer.phone, Is.EqualTo("012 3456 7890"));
            Assert.That(customer.dateOfBirth, Is.EqualTo("1956-05-17"));

            var getResponse = AuthenticatedGet<CustomerData>(RelativePath, customer.id, setup);
            var getCustomer = (CustomerData)getResponse.Payload;

            Assert.That(getCustomer.id, Is.EqualTo(customer.id));
            Assert.That(getCustomer.firstName, Is.EqualTo(customer.firstName));
            Assert.That(getCustomer.lastName, Is.EqualTo(customer.lastName));
            Assert.That(getCustomer.email, Is.EqualTo(customer.email));
            Assert.That(getCustomer.phone, Is.EqualTo(customer.phone));
            Assert.That(getCustomer.dateOfBirth, Is.EqualTo(customer.dateOfBirth));
        }
    }
}
