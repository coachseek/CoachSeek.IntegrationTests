using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
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
                var command = GivenNoBusinessSaveCommand();
                var response = WhenTryUpdateBusiness(command);
                ThenReturnNoDataErrorResponse(response);
            }

            [Test]
            public void GivenEmptyBusinessSaveCommand_WhenTryUpdateBusiness_ThenReturnRootRequiredError()
            {
                var command = GivenEmptyBusinessSaveCommand();
                var response = WhenTryUpdateBusiness(command);
                ThenReturnRootRequiredErrorResponse(response);
            }

            [Test]
            public void GivenValidBusinessSaveCommand_WhenTryUpdateBusinessAnonymously_ThenReturnUnauthorised()
            {
                var command = GivenValidBusinessSaveCommand();
                var response = WhenTryUpdateBusinessAnonymously(command);
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
                var command = GivenEmptyPayment();
                var response = WhenTryUpdateBusiness(command);
                ThenReturnsRequiredPaymentOptionErrors(response);
            }

            [Test]
            public void GivenInvalidCurrency_WhenTryUpdateBusiness_ThenReturnsCurrencyNotSupportedError()
            {
                var command = GivenInvalidCurrency();
                var response = WhenTryUpdateBusiness(command);
                ThenReturnsCurrencyNotSupportedError(response);
            }

            [Test]
            public void GivenInvalidPaymentProvider_WhenTryUpdateBusiness_ThenReturnsPaymentProviderNotSupportedError()
            {
                var command = GivenInvalidPaymentProvider();
                var response = WhenTryUpdateBusiness(command);
                ThenReturnsPaymentProviderNotSupportedError(response);
            }

            [Test]
            public void GivenNoMerchantAccountIdentifier_WhenTryUpdateBusiness_ThenReturnsMissingMerchantAccountIdentifierError()
            {
                var command = GivenNoMerchantAccountIdentifier();
                var response = WhenTryUpdateBusiness(command);
                ThenReturnsMissingMerchantAccountIdentifierError(response);
            }

            [Test]
            public void GivenInvalidMerchantAccountIdentifierFormat_WhenTryUpdateBusiness_ThenReturnsMissingMerchantAccountIdentifierFormatError()
            {
                var command = GivenInvalidMerchantAccountIdentifierFormat();
                var response = WhenTryUpdateBusiness(command);
                ThenReturnsMissingMerchantAccountIdentifierFormatError(response);
            }

            [Test]
            public void GivenValidBusinessSaveCommandWithoutPaymentProvider_WhenTryUpdateBusiness_ThenUpdateTheBusinessWithoutPaymentProvider()
            {
                var command = GivenValidBusinessSaveCommandWithoutPaymentProvider();
                var response = WhenTryUpdateBusiness(command);
                ThenUpdateTheBusinessWithoutPaymentProvider(response, command);
            }

            [Test, Ignore("Uncomment when have isOnlinePaymentEnabled and forceOnlinePayment")]
            public void GivenValidBusinessSaveCommandWithOnlinePaymentOn_WhenTryUpdateBusiness_ThenUpdateTheBusinessWithPaymentOptionsSet()
            {
                var command = GivenValidBusinessSaveCommandWithOnlinePaymentOn();
                var response = WhenTryUpdateBusiness(command);
                ThenUpdateTheBusinessWithPaymentOptionsSet(response, command);
            }


            private ApiBusinessSaveCommand GivenEmptyPayment()
            {
                return new ApiBusinessSaveCommand
                {
                    name = Business.Name,
                    payment = new ApiBusinessPaymentOptions()
                };
            }

            private ApiBusinessSaveCommand GivenInvalidCurrency()
            {
                return new ApiBusinessSaveCommand
                {
                    name = Business.Name,
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

            private ApiBusinessSaveCommand GivenInvalidPaymentProvider()
            {
                return new ApiBusinessSaveCommand
                {
                    name = Business.Name,
                    payment = new ApiBusinessPaymentOptions
                    {
                        currency = "AUD",
                        isOnlinePaymentEnabled = false,
                        forceOnlinePayment = false,
                        paymentProvider = "ABC"
                    }
                };
            }

            private ApiBusinessSaveCommand GivenNoMerchantAccountIdentifier()
            {
                return new ApiBusinessSaveCommand
                {
                    name = Business.Name,
                    payment = new ApiBusinessPaymentOptions
                    {
                        currency = "AUD",
                        isOnlinePaymentEnabled = true,
                        forceOnlinePayment = false,
                        paymentProvider = "PayPal"
                    }
                };
            }

            private ApiBusinessSaveCommand GivenInvalidMerchantAccountIdentifierFormat()
            {
                return new ApiBusinessSaveCommand
                {
                    name = Business.Name,
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


            private void ThenReturnsRequiredPaymentOptionErrors(Response response)
            {
                AssertMultipleErrors(response, new[,] { { "The currency field is required.", "business.payment.currency" },
                                                        { "The isOnlinePaymentEnabled field is required.", "business.payment.isOnlinePaymentEnabled" } });
            }

            private void ThenReturnsCurrencyNotSupportedError(Response response)
            {
                AssertSingleError(response, "This currency is not supported.", "business.payment.currency");
            }

            private void ThenReturnsPaymentProviderNotSupportedError(Response response)
            {
                AssertSingleError(response, "This payment provider is not supported.", "business.payment.paymentProvider");
            }

            private void ThenReturnsMissingMerchantAccountIdentifierError(Response response)
            {
                AssertSingleError(response, "Missing merchant account identifier.", "business.payment.merchantAccountIdentifier");
            }

            private void ThenReturnsMissingMerchantAccountIdentifierFormatError(Response response)
            {
                AssertSingleError(response, "Invalid merchant account identifier format.", "business.payment.merchantAccountIdentifier");
            }

            private void ThenUpdateTheBusinessWithoutPaymentProvider(Response response, ApiBusinessSaveCommand command)
            {
                var responseBusiness = AssertSuccessResponse<BusinessData>(response);

                Assert.That(responseBusiness.id, Is.EqualTo(Business.Id));
                Assert.That(responseBusiness.name, Is.EqualTo(command.name));
                Assert.That(responseBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(responseBusiness.domain, Is.EqualTo(Business.Domain));

                var getResponse = AuthenticatedGet<BusinessData>(Url.AbsoluteUri);
                var getBusiness = (BusinessData) getResponse.Payload;

                Assert.That(getBusiness.id, Is.EqualTo(Business.Id));
                Assert.That(getBusiness.name, Is.EqualTo(command.name));
                Assert.That(getBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(getBusiness.domain, Is.EqualTo(Business.Domain));
            }

            private void ThenUpdateTheBusinessWithPaymentOptionsSet(Response response, ApiBusinessSaveCommand command)
            {
                var responseBusiness = AssertSuccessResponse<BusinessData>(response);

                Assert.That(responseBusiness.id, Is.EqualTo(Business.Id));
                Assert.That(responseBusiness.name, Is.EqualTo(command.name));
                Assert.That(responseBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(responseBusiness.domain, Is.EqualTo(Business.Domain));
                Assert.That(responseBusiness.payment.isOnlinePaymentEnabled, Is.EqualTo(command.payment.isOnlinePaymentEnabled));
                Assert.That(responseBusiness.payment.forceOnlinePayment, Is.EqualTo(command.payment.forceOnlinePayment));
                Assert.That(responseBusiness.payment.paymentProvider, Is.EqualTo(command.payment.paymentProvider));
                Assert.That(responseBusiness.payment.merchantAccountIdentifier, Is.EqualTo(command.payment.merchantAccountIdentifier));

                var getResponse = AuthenticatedGet<BusinessData>(Url.AbsoluteUri);
                var getBusiness = (BusinessData)getResponse.Payload;

                Assert.That(getBusiness.id, Is.EqualTo(Business.Id));
                Assert.That(getBusiness.name, Is.EqualTo(command.name));
                Assert.That(getBusiness.domain, Is.EqualTo(Business.Domain));
                Assert.That(getBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(getBusiness.payment.isOnlinePaymentEnabled, Is.EqualTo(command.payment.isOnlinePaymentEnabled));
                Assert.That(getBusiness.payment.forceOnlinePayment, Is.EqualTo(command.payment.forceOnlinePayment));
                Assert.That(getBusiness.payment.paymentProvider, Is.EqualTo(command.payment.paymentProvider));
                Assert.That(getBusiness.payment.merchantAccountIdentifier, Is.EqualTo(command.payment.merchantAccountIdentifier));
            }
        }


        private ApiBusinessSaveCommand GivenValidBusinessSaveCommandWithoutPaymentProvider()
        {
            return new ApiBusinessSaveCommand
            {
                name = Random.RandomString,
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

        private Response WhenTryUpdateBusiness(ApiBusinessSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);
            return WhenTryUpdateBusiness(json);
        }

        private Response WhenTryUpdateBusiness(string json)
        {
            return Post<BusinessData>(json);
        }

        private Response WhenTryUpdateBusinessAnonymously(ApiBusinessSaveCommand command)
        {
            var json = JsonConvert.SerializeObject(command);
            return WhenTryUpdateBusinessAnonymously(json);
        }

        private Response WhenTryUpdateBusinessAnonymously(string json)
        {
            return PostAnonymouslyToBusiness<BusinessData>(json);
        }


        private void ThenReturnNoDataErrorResponse(Response response)
        {
            AssertSingleError(response, "Please post us some data!");
        }

        private void ThenReturnRootRequiredErrorResponse(Response response)
        {
            AssertMultipleErrors(response, new[,] { { "The name field is required.", "business.name" },
                                                    { "The payment field is required.", "business.payment" } });
        }
    }
}
