﻿using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Business
{
    public class BusinessUpdateTests : BusinessTests
    {
        [TestFixture]
        public class BusinessCommandTests : BusinessUpdateTests
        {
            [Test]
            public void GivenNoBusinessSaveCommand_WhenTryUpdateBusiness_ThenReturnNoDataError()
            {
                var setup = RegisterBusiness();

                var command = GivenNoBusinessSaveCommand();
                var response = WhenTryUpdateBusiness(command, setup);
                ThenReturnNoDataError(response);
            }

            [Test]
            public void GivenEmptyBusinessSaveCommand_WhenTryUpdateBusiness_ThenReturnRootRequiredError()
            {
                var setup = RegisterBusiness();

                var command = GivenEmptyBusinessSaveCommand();
                var response = WhenTryUpdateBusiness(command, setup);
                ThenReturnRootRequiredErrorResponse(response);
            }

            [Test]
            public void GivenValidBusinessSaveCommand_WhenTryUpdateBusinessAnonymously_ThenReturnUnauthorised()
            {
                var setup = RegisterBusiness();

                var command = GivenValidBusinessSaveCommand();
                var response = WhenTryUpdateBusinessAnonymously(command, setup);
                AssertUnauthorised(response);
            }


            private string GivenNoBusinessSaveCommand()
            {
                return "";
            }

            private string GivenEmptyBusinessSaveCommand()
            {
                return "{}";
            }

            private ApiBusinessSaveCommand GivenValidBusinessSaveCommand()
            {
                return new ApiBusinessSaveCommand
                {
                    name = Random.RandomString,
                    payment = new ApiBusinessPaymentOptions
                    {
                        currency = "EUR",
                        isOnlinePaymentEnabled = true,
                        forceOnlinePayment = false,
                        paymentProvider = "PayPal",
                        merchantAccountIdentifier = "olaf@coachseek.com"
                    }
                };
            }
        }

        [TestFixture]
        public class BusinessExistingTests : BusinessUpdateTests
        {
            [Test]
            public void GivenEmptyPayment_WhenTryUpdateBusiness_ThenReturnsRequiredPaymentOptionErrors()
            {
                var setup = RegisterBusiness();

                var command = GivenEmptyPayment(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenReturnsRequiredPaymentOptionErrors(response);
            }

            [Test]
            public void GivenInvalidCurrency_WhenTryUpdateBusiness_ThenReturnsCurrencyNotSupportedError()
            {
                var setup = RegisterBusiness();

                var command = GivenInvalidCurrency(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenReturnsCurrencyNotSupportedError(response);
            }

            [Test]
            public void GivenInvalidPaymentProvider_WhenTryUpdateBusiness_ThenReturnsPaymentProviderNotSupportedError()
            {
                var setup = RegisterBusiness();

                var command = GivenInvalidPaymentProvider(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenReturnsPaymentProviderNotSupportedError(response);
            }

            [Test]
            public void GivenNoMerchantAccountIdentifier_WhenTryUpdateBusiness_ThenReturnsMissingMerchantAccountIdentifierError()
            {
                var setup = RegisterBusiness();

                var command = GivenNoMerchantAccountIdentifier(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenReturnsMissingMerchantAccountIdentifierError(response);
            }

            [Test]
            public void GivenInvalidMerchantAccountIdentifierFormat_WhenTryUpdateBusiness_ThenReturnsMissingMerchantAccountIdentifierFormatError()
            {
                var setup = RegisterBusiness();

                var command = GivenInvalidMerchantAccountIdentifierFormat(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenReturnsMissingMerchantAccountIdentifierFormatError(response);
            }

            [Test]
            public void GivenValidBusinessSaveCommandWithoutPaymentProvider_WhenTryUpdateBusiness_ThenUpdateTheBusinessWithoutPaymentProvider()
            {
                var setup = RegisterBusiness();

                var command = GivenValidBusinessSaveCommandWithoutPaymentProvider(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenUpdateTheBusinessWithoutPaymentProvider(response, command, setup);
            }

            [Test, Ignore("Uncomment when have isOnlinePaymentEnabled and forceOnlinePayment")]
            public void GivenValidBusinessSaveCommandWithOnlinePaymentOn_WhenTryUpdateBusiness_ThenUpdateTheBusinessWithPaymentOptionsSet()
            {
                //var setup = RegisterBusiness();

                //var command = GivenValidBusinessSaveCommandWithOnlinePaymentOn();
                //var response = WhenTryUpdateBusiness(command, setup);
                //ThenUpdateTheBusinessWithPaymentOptionsSet(response, command);
            }


            private ApiBusinessSaveCommand GivenEmptyPayment(SetupData setup)
            {
                return new ApiBusinessSaveCommand
                {
                    name = setup.Business.Name,
                    payment = new ApiBusinessPaymentOptions()
                };
            }

            private ApiBusinessSaveCommand GivenInvalidCurrency(SetupData setup)
            {
                return new ApiBusinessSaveCommand
                {
                    name = setup.Business.Name,
                    payment = new ApiBusinessPaymentOptions
                    {
                        currency = "QQQ",
                        isOnlinePaymentEnabled = false,
                        forceOnlinePayment = false,
                        paymentProvider = "PayPal",
                        merchantAccountIdentifier = "olaf@coachseek.com"
                    }
                };
            }

            private ApiBusinessSaveCommand GivenInvalidPaymentProvider(SetupData setup)
            {
                return new ApiBusinessSaveCommand
                {
                    name = setup.Business.Name,
                    payment = new ApiBusinessPaymentOptions
                    {
                        currency = "AUD",
                        isOnlinePaymentEnabled = false,
                        forceOnlinePayment = false,
                        paymentProvider = "ABC"
                    }
                };
            }

            private ApiBusinessSaveCommand GivenNoMerchantAccountIdentifier(SetupData setup)
            {
                return new ApiBusinessSaveCommand
                {
                    name = setup.Business.Name,
                    payment = new ApiBusinessPaymentOptions
                    {
                        currency = "AUD",
                        isOnlinePaymentEnabled = true,
                        forceOnlinePayment = false,
                        paymentProvider = "PayPal"
                    }
                };
            }

            private ApiBusinessSaveCommand GivenInvalidMerchantAccountIdentifierFormat(SetupData setup)
            {
                return new ApiBusinessSaveCommand
                {
                    name = setup.Business.Name,
                    payment = new ApiBusinessPaymentOptions
                    {
                        currency = "AUD",
                        isOnlinePaymentEnabled = false,
                        forceOnlinePayment = false,
                        paymentProvider = "PayPal",
                        merchantAccountIdentifier = "abc123"
                    }
                };
            }


            private void ThenReturnsRequiredPaymentOptionErrors(ApiResponse response)
            {
                AssertMultipleErrors(response, new[,] { { "The currency field is required.", "business.payment.currency" },
                                                        { "The isOnlinePaymentEnabled field is required.", "business.payment.isOnlinePaymentEnabled" } });
            }

            private void ThenReturnsCurrencyNotSupportedError(ApiResponse response)
            {
                AssertSingleError(response, "This currency is not supported.", "business.payment.currency");
            }

            private void ThenReturnsPaymentProviderNotSupportedError(ApiResponse response)
            {
                AssertSingleError(response, "This payment provider is not supported.", "business.payment.paymentProvider");
            }

            private void ThenReturnsMissingMerchantAccountIdentifierError(ApiResponse response)
            {
                AssertSingleError(response, "Missing merchant account identifier.", "business.payment.merchantAccountIdentifier");
            }

            private void ThenReturnsMissingMerchantAccountIdentifierFormatError(ApiResponse response)
            {
                AssertSingleError(response, "Invalid merchant account identifier format.", "business.payment.merchantAccountIdentifier");
            }

            private void ThenUpdateTheBusinessWithoutPaymentProvider(ApiResponse response, ApiBusinessSaveCommand command, SetupData setup)
            {
                var responseBusiness = AssertSuccessResponse<BusinessData>(response);

                Assert.That(responseBusiness.id, Is.EqualTo(setup.Business.Id));
                Assert.That(responseBusiness.name, Is.EqualTo(command.name));
                Assert.That(responseBusiness.domain, Is.EqualTo(setup.Business.Domain));
                Assert.That(responseBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(responseBusiness.payment.paymentProvider, Is.Null);

                var getResponse = AuthenticatedGet<BusinessData>("Business", setup);
                var getBusiness = (BusinessData) getResponse.Payload;

                Assert.That(getBusiness.id, Is.EqualTo(setup.Business.Id));
                Assert.That(getBusiness.name, Is.EqualTo(command.name));
                Assert.That(getBusiness.domain, Is.EqualTo(setup.Business.Domain));
                Assert.That(getBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(getBusiness.payment.paymentProvider, Is.Null);
            }

            //private void ThenUpdateTheBusinessWithPaymentOptionsSet(ApiResponse response, ApiBusinessSaveCommand command)
            //{
            //    var responseBusiness = AssertSuccessResponse<BusinessData>(response);

            //    Assert.That(responseBusiness.id, Is.EqualTo(Business.Id));
            //    Assert.That(responseBusiness.name, Is.EqualTo(command.name));
            //    Assert.That(responseBusiness.payment.currency, Is.EqualTo(command.payment.currency));
            //    Assert.That(responseBusiness.domain, Is.EqualTo(Business.Domain));
            //    Assert.That(responseBusiness.payment.isOnlinePaymentEnabled, Is.EqualTo(command.payment.isOnlinePaymentEnabled));
            //    Assert.That(responseBusiness.payment.forceOnlinePayment, Is.EqualTo(command.payment.forceOnlinePayment));
            //    Assert.That(responseBusiness.payment.paymentProvider, Is.EqualTo(command.payment.paymentProvider));
            //    Assert.That(responseBusiness.payment.merchantAccountIdentifier, Is.EqualTo(command.payment.merchantAccountIdentifier));

            //    var getResponse = AuthenticatedGet<BusinessData>(Url.AbsoluteUri);
            //    var getBusiness = (BusinessData)getResponse.Payload;

            //    Assert.That(getBusiness.id, Is.EqualTo(Business.Id));
            //    Assert.That(getBusiness.name, Is.EqualTo(command.name));
            //    Assert.That(getBusiness.domain, Is.EqualTo(Business.Domain));
            //    Assert.That(getBusiness.payment.currency, Is.EqualTo(command.payment.currency));
            //    Assert.That(getBusiness.payment.isOnlinePaymentEnabled, Is.EqualTo(command.payment.isOnlinePaymentEnabled));
            //    Assert.That(getBusiness.payment.forceOnlinePayment, Is.EqualTo(command.payment.forceOnlinePayment));
            //    Assert.That(getBusiness.payment.paymentProvider, Is.EqualTo(command.payment.paymentProvider));
            //    Assert.That(getBusiness.payment.merchantAccountIdentifier, Is.EqualTo(command.payment.merchantAccountIdentifier));
            //}
        }


        private ApiBusinessSaveCommand GivenValidBusinessSaveCommandWithoutPaymentProvider(SetupData setup)
        {
            return new ApiBusinessSaveCommand
            {
                name = setup.Business.Name,
                payment = new ApiBusinessPaymentOptions
                {
                    currency = "EUR",
                    isOnlinePaymentEnabled = false                    
                }
            };
        }

        private ApiBusinessSaveCommand GivenValidBusinessSaveCommandWithOnlinePaymentOn()
        {
            return new ApiBusinessSaveCommand
            {
                name = Random.RandomString,
                payment = new ApiBusinessPaymentOptions
                {
                    currency = "EUR",
                    isOnlinePaymentEnabled = true,
                    forceOnlinePayment = true,
                    paymentProvider = "PayPal",
                    merchantAccountIdentifier = "bob@example.com"
                }
            };
        }

        private ApiResponse WhenTryUpdateBusiness(ApiBusinessSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryUpdateBusiness(json, setup);
        }

        private ApiResponse WhenTryUpdateBusiness(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<BusinessData>(json, 
                                                                       setup.Business.UserName,
                                                                       setup.Business.Password,
                                                                       RelativePath);
        }

        private ApiResponse WhenTryUpdateBusinessAnonymously(ApiBusinessSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryUpdateBusinessAnonymously(json, setup);
        }

        private ApiResponse WhenTryUpdateBusinessAnonymously(string json, SetupData setup)
        {
            return new TestBusinessAnonymousApiClient().Post<BusinessData>(json,
                                                                           setup.Business.Domain,
                                                                           RelativePath);
        }


        private void ThenReturnNoDataError(ApiResponse response)
        {
            AssertSingleError(response, "Please post us some data!");
        }

        private void ThenReturnRootRequiredErrorResponse(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "The name field is required.", "business.name" },
                                                    { "The payment field is required.", "business.payment" } });
        }
    }
}