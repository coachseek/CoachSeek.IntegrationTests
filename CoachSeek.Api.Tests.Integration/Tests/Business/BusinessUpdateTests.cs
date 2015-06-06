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
                    currency = "EUR"
                };
            }
        }

        [TestFixture]
        public class BusinessExistingTests : BusinessUpdateTests
        {
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

            [Test]
            public void GivenValidBusinessSaveCommandWithPaymentProvider_WhenTryUpdateBusiness_ThenUpdateTheBusinessWithPaymentProvider()
            {
                var command = GivenValidBusinessSaveCommandWithPaymentProvider();
                var response = WhenTryUpdateBusiness(command);
                ThenUpdateTheBusinessWithPaymentProvider(response, command);
            }


            private ApiBusinessSaveCommand GivenInvalidCurrency()
            {
                return new ApiBusinessSaveCommand
                {
                    name = Business.Name,
                    currency = "QQQ"
                };
            }

            private ApiBusinessSaveCommand GivenInvalidPaymentProvider()
            {
                return new ApiBusinessSaveCommand
                {
                    name = Business.Name,
                    currency = "AUD",
                    paymentProvider = "ABC"
                };
            }

            private ApiBusinessSaveCommand GivenNoMerchantAccountIdentifier()
            {
                return new ApiBusinessSaveCommand
                {
                    name = Business.Name,
                    currency = "AUD",
                    paymentProvider = "PayPal"
                };
            }

            private ApiBusinessSaveCommand GivenInvalidMerchantAccountIdentifierFormat()
            {
                return new ApiBusinessSaveCommand
                {
                    name = Business.Name,
                    currency = "AUD",
                    paymentProvider = "PayPal",
                    merchantAccountIdentifier = "abc123"
                };
            }


            private void ThenReturnsCurrencyNotSupportedError(Response response)
            {
                AssertSingleError(response, "This currency is not supported.", "business.currency");
            }

            private void ThenReturnsPaymentProviderNotSupportedError(Response response)
            {
                AssertSingleError(response, "This payment provider is not supported.", "business.paymentProvider");
            }

            private void ThenReturnsMissingMerchantAccountIdentifierError(Response response)
            {
                AssertSingleError(response, "Missing merchant account identifier.", "business.merchantAccountIdentifier");
            }

            private void ThenReturnsMissingMerchantAccountIdentifierFormatError(Response response)
            {
                AssertSingleError(response, "Invalid merchant account identifier format.", "business.merchantAccountIdentifier");
            }

            private void ThenUpdateTheBusinessWithoutPaymentProvider(Response response, ApiBusinessSaveCommand command)
            {
                var responseBusiness = AssertSuccessResponse<BusinessData>(response);

                Assert.That(responseBusiness.id, Is.EqualTo(Business.Id));
                Assert.That(responseBusiness.name, Is.EqualTo(command.name));
                Assert.That(responseBusiness.currency, Is.EqualTo(command.currency));
                Assert.That(responseBusiness.domain, Is.EqualTo(Business.Domain));

                var getResponse = AuthenticatedGet<BusinessData>(Url.AbsoluteUri);
                var getBusiness = (BusinessData) getResponse.Payload;

                Assert.That(getBusiness.id, Is.EqualTo(Business.Id));
                Assert.That(getBusiness.name, Is.EqualTo(command.name));
                Assert.That(getBusiness.currency, Is.EqualTo(command.currency));
                Assert.That(getBusiness.domain, Is.EqualTo(Business.Domain));
            }

            private void ThenUpdateTheBusinessWithPaymentProvider(Response response, ApiBusinessSaveCommand command)
            {
                var responseBusiness = AssertSuccessResponse<BusinessData>(response);

                Assert.That(responseBusiness.id, Is.EqualTo(Business.Id));
                Assert.That(responseBusiness.name, Is.EqualTo(command.name));
                Assert.That(responseBusiness.currency, Is.EqualTo(command.currency));
                Assert.That(responseBusiness.domain, Is.EqualTo(Business.Domain));
                Assert.That(responseBusiness.paymentProvider, Is.EqualTo(command.paymentProvider));
                Assert.That(responseBusiness.merchantAccountIdentifier, Is.EqualTo(command.merchantAccountIdentifier));

                var getResponse = AuthenticatedGet<BusinessData>(Url.AbsoluteUri);
                var getBusiness = (BusinessData)getResponse.Payload;

                Assert.That(getBusiness.id, Is.EqualTo(Business.Id));
                Assert.That(getBusiness.name, Is.EqualTo(command.name));
                Assert.That(getBusiness.currency, Is.EqualTo(command.currency));
                Assert.That(getBusiness.domain, Is.EqualTo(Business.Domain));
                Assert.That(getBusiness.paymentProvider, Is.EqualTo(command.paymentProvider));
                Assert.That(getBusiness.merchantAccountIdentifier, Is.EqualTo(command.merchantAccountIdentifier));
            }
        }


        private ApiBusinessSaveCommand GivenValidBusinessSaveCommandWithoutPaymentProvider()
        {
            return new ApiBusinessSaveCommand
            {
                name = Random.RandomString,
                currency = "EUR"
            };
        }

        private ApiBusinessSaveCommand GivenValidBusinessSaveCommandWithPaymentProvider()
        {
            return new ApiBusinessSaveCommand
            {
                name = Random.RandomString,
                currency = "EUR",
                paymentProvider = "PayPal",
                merchantAccountIdentifier = "bob@example.com"
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
                                                    { "The currency field is required.", "business.currency" } });
        }
    }
}
