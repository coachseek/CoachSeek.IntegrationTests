using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
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

            [Test]
            public void GivenMissingUseProRataPricing_WhenTryUpdateBusiness_ThenReturnUseProRataPricingRequiredError()
            {
                var setup = RegisterBusiness();

                var command = GivenMissingUseProRataPricing(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenReturnUseProRataPricingRequiredError(response);
            }

            [Test]
            public void GivenIsOnlinePaymentEnabledButNoPaymentProvider_WhenTryUpdateBusiness_ThenReturnPaymentProviderRequiredWhenOnlineBookingIsEnabledError()
            {
                var setup = RegisterBusiness();

                var command = GivenIsOnlinePaymentEnabledButNoPaymentProvider(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenReturnPaymentProviderRequiredWhenOnlineBookingIsEnabledError(response);
            }


            private string GivenNoBusinessSaveCommand()
            {
                return "";
            }

            private string GivenEmptyBusinessSaveCommand()
            {
                return "{}";
            }

            private ApiBusinessSaveCommand GivenMissingUseProRataPricing(SetupData setup)
            {
                return new ApiBusinessSaveCommand
                {
                    name = setup.Business.Name,
                    domain = setup.Business.Domain,
                    sport = setup.Business.Sport,
                    payment = new ApiBusinessPaymentOptions
                    {
                        currency = "EUR",
                        isOnlinePaymentEnabled = false
                    }
                };
            }

            private ApiBusinessSaveCommand GivenIsOnlinePaymentEnabledButNoPaymentProvider(SetupData setup)
            {
                return new ApiBusinessSaveCommand
                {
                    name = setup.Business.Name,
                    domain = setup.Business.Domain,
                    sport = setup.Business.Sport,
                    payment = new ApiBusinessPaymentOptions
                    {
                        currency = "EUR",
                        isOnlinePaymentEnabled = true,
                        forceOnlinePayment = false,
                        merchantAccountIdentifier = "olaf@coachseek.com",
                        useProRataPricing = false
                    }
                };
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
                        merchantAccountIdentifier = "olaf@coachseek.com",
                        useProRataPricing = false
                    }
                };
            }

            private void ThenReturnUseProRataPricingRequiredError(ApiResponse response)
            {
                AssertSingleError(response,
                                  ErrorCodes.UseProRataPricingRequired,
                                  "The UseProRataPricing field is required.");
            }

            private void ThenReturnPaymentProviderRequiredWhenOnlineBookingIsEnabledError(ApiResponse response)
            {
                AssertSingleError(response, 
                                  ErrorCodes.PaymentProviderRequiredWhenOnlineBookingIsEnabled,
                                  "When Online Payment is enabled then an Online Payment Provider must be specified.");
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
            public void GivenInvalidSubdomainFormat_WhenTryUpdateBusiness_ThenReturnsInvalidSubdomainFormatError()
            {
                var setup = RegisterBusiness();

                var command = GivenInvalidSubdomainFormat(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenReturnsInvalidSubdomainFormatError(response, command, setup);
            }

            [Test]
            public void GivenSubdomainAlreadyExists_WhenTryUpdateBusiness_ThenReturnsDuplicateSubdomainError()
            {
                var setup = RegisterBusiness();

                var command = GivenSubdomainAlreadyExists(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenReturnsDuplicateSubdomainError(response, command, setup);
            }

            [Test]
            public void GivenSubdomainIsReserved_WhenTryUpdateBusiness_ThenReturnsDuplicateSubdomainError()
            {
                var setup = RegisterBusiness();

                var command = GivenSubdomainIsReserved(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenReturnsDuplicateSubdomainError(response, command, setup);
            }

            [Test]
            public void GivenValidBusinessSaveCommandWithoutSport_WhenTryUpdateBusiness_ThenUpdateTheBusinessWithoutSport()
            {
                var setup = RegisterBusiness();

                var command = GivenValidBusinessSaveCommandWithoutSport(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenUpdateTheBusinessWithoutSport(response, command, setup);
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

            [Test]
            public void GivenValidBusinessSaveCommandWithOnlinePaymentOn_WhenTryUpdateBusiness_ThenUpdateTheBusinessWithPaymentOptionsSet()
            {
                var setup = RegisterBusiness();

                var command = GivenValidBusinessSaveCommandWithOnlinePaymentOn(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenUpdateTheBusinessWithPaymentOptionsSet(response, command, setup);
            }

            [Test]
            public void GivenHaveLotsOfChanges_WhenTryUpdateBusiness_ThenUpdateTheBusinessWithChanges()
            {
                var setup = RegisterBusiness();

                var command = GivenHaveLotsOfChanges(setup);
                var response = WhenTryUpdateBusiness(command, setup);
                ThenUpdateTheBusinessWithChanges(response, command, setup);
            }


            private ApiBusinessSaveCommand CreateValidBusinessSaveCommand(SetupData setup)
            {
                return new ApiBusinessSaveCommand
                {
                    name = setup.Business.Name,
                    domain = setup.Business.Domain,
                    sport = setup.Business.Sport,
                    payment = new ApiBusinessPaymentOptions
                    {
                        currency = "AUD",
                        isOnlinePaymentEnabled = false,
                        forceOnlinePayment = false,
                        paymentProvider = "PayPal",
                        merchantAccountIdentifier = "olaf@coachseek.com",
                        useProRataPricing = false
                    }
                };
            }

            private ApiBusinessSaveCommand GivenEmptyPayment(SetupData setup)
            {
                var command = CreateValidBusinessSaveCommand(setup);
                command.payment = new ApiBusinessPaymentOptions();
                return command;
            }

            private ApiBusinessSaveCommand GivenInvalidSubdomainFormat(SetupData setup)
            {
                var command = CreateValidBusinessSaveCommand(setup);
                command.domain = "my new shiny domain";
                return command;
            }

            private ApiBusinessSaveCommand GivenSubdomainIsReserved(SetupData setup)
            {
                var command = CreateValidBusinessSaveCommand(setup);
                command.domain = "tennis";
                return command;
            }

            private ApiBusinessSaveCommand GivenValidBusinessSaveCommandWithoutSport(SetupData setup)
            {
                var command = CreateValidBusinessSaveCommand(setup);
                command.sport = null;
                return command;
            }

            private ApiBusinessSaveCommand GivenSubdomainAlreadyExists(SetupData setup)
            {
                var command = CreateValidBusinessSaveCommand(setup);
                command.domain = RegisterBusiness().Business.Domain;
                return command;
            }

            private ApiBusinessSaveCommand GivenInvalidCurrency(SetupData setup)
            {
                var command = CreateValidBusinessSaveCommand(setup);
                command.payment.currency = "QQQ";
                return command;
            }

            private ApiBusinessSaveCommand GivenInvalidPaymentProvider(SetupData setup)
            {
                var command = CreateValidBusinessSaveCommand(setup);
                command.payment.paymentProvider = "ABC";
                return command;
            }

            private ApiBusinessSaveCommand GivenNoMerchantAccountIdentifier(SetupData setup)
            {
                var command = CreateValidBusinessSaveCommand(setup);
                command.payment.merchantAccountIdentifier = null;
                return command;
            }

            private ApiBusinessSaveCommand GivenInvalidMerchantAccountIdentifierFormat(SetupData setup)
            {
                var command = CreateValidBusinessSaveCommand(setup);
                command.payment.merchantAccountIdentifier = "abc123";
                return command;
            }

            private ApiBusinessSaveCommand GivenValidBusinessSaveCommandWithoutPaymentProvider(SetupData setup)
            {
                var command = CreateValidBusinessSaveCommand(setup);
                command.payment = new ApiBusinessPaymentOptions
                {
                    currency = "EUR",
                    isOnlinePaymentEnabled = false,
                    useProRataPricing = false
                };
                return command;
            }

            private ApiBusinessSaveCommand GivenValidBusinessSaveCommandWithOnlinePaymentOn(SetupData setup)
            {
                var command = CreateValidBusinessSaveCommand(setup);
                command.payment.isOnlinePaymentEnabled = true;
                return command;
            }

            private ApiBusinessSaveCommand GivenHaveLotsOfChanges(SetupData setup)
            {
                return new ApiBusinessSaveCommand
                {
                    name = Random.RandomString,
                    domain = Random.RandomString.ToUpper(),
                    sport = "martial-arts",
                    payment = new ApiBusinessPaymentOptions
                    {
                        currency = "EUR",
                        isOnlinePaymentEnabled = true,
                        forceOnlinePayment = true,
                        paymentProvider = "PayPal",
                        merchantAccountIdentifier = "olaf@coachseek.com",
                        useProRataPricing = false
                    }
                };
            }


            private void ThenReturnsRequiredPaymentOptionErrors(ApiResponse response)
            {
                AssertMultipleErrors(response, new[,] { { "currency-required", "The Currency field is required.", null },
                                                        { "isonlinepaymentenabled-required", "The IsOnlinePaymentEnabled field is required.", null },
                                                        { "useproratapricing-required", "The UseProRataPricing field is required.", null } });
            }

            private void ThenReturnsInvalidSubdomainFormatError(ApiResponse response, ApiBusinessSaveCommand command, SetupData setup)
            {
                AssertSingleError(response,
                                  ErrorCodes.SubdomainFormatInvalid,
                                  string.Format("The subdomain '{0}' is not in a valid format.", command.domain),
                                  command.domain);
            }

            private void ThenReturnsDuplicateSubdomainError(ApiResponse response, ApiBusinessSaveCommand command, SetupData setup)
            {
                AssertSingleError(response,
                                  ErrorCodes.SubdomainDuplicate,
                                  string.Format("The subdomain '{0}' already exists.", command.domain),
                                  command.domain);
            }

            private void ThenUpdateTheBusinessWithoutSport(ApiResponse response, ApiBusinessSaveCommand command, SetupData setup)
            {
                var responseBusiness = AssertSuccessResponse<BusinessData>(response);

                Assert.That(responseBusiness.id, Is.EqualTo(setup.Business.Id));
                Assert.That(responseBusiness.name, Is.EqualTo(command.name));
                Assert.That(responseBusiness.domain, Is.EqualTo(command.domain));
                Assert.That(responseBusiness.sport, Is.Null);
                Assert.That(responseBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(responseBusiness.payment.isOnlinePaymentEnabled, Is.EqualTo(command.payment.isOnlinePaymentEnabled));
                Assert.That(responseBusiness.payment.forceOnlinePayment, Is.EqualTo(command.payment.forceOnlinePayment));
                Assert.That(responseBusiness.payment.paymentProvider, Is.EqualTo(command.payment.paymentProvider));
                Assert.That(responseBusiness.payment.merchantAccountIdentifier, Is.EqualTo(command.payment.merchantAccountIdentifier));
                Assert.That(responseBusiness.payment.useProRataPricing, Is.EqualTo(command.payment.useProRataPricing));

                var getResponse = AuthenticatedGet<BusinessData>("Business", setup);
                var getBusiness = (BusinessData)getResponse.Payload;

                Assert.That(getBusiness.id, Is.EqualTo(setup.Business.Id));
                Assert.That(getBusiness.name, Is.EqualTo(command.name));
                Assert.That(getBusiness.domain, Is.EqualTo(command.domain));
                Assert.That(getBusiness.sport, Is.Null);
                Assert.That(getBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(getBusiness.payment.isOnlinePaymentEnabled, Is.EqualTo(command.payment.isOnlinePaymentEnabled));
                Assert.That(getBusiness.payment.forceOnlinePayment, Is.EqualTo(command.payment.forceOnlinePayment));
                Assert.That(getBusiness.payment.paymentProvider, Is.EqualTo(command.payment.paymentProvider));
                Assert.That(getBusiness.payment.merchantAccountIdentifier, Is.EqualTo(command.payment.merchantAccountIdentifier));
                Assert.That(getBusiness.payment.useProRataPricing, Is.EqualTo(command.payment.useProRataPricing));
            }

            private void ThenReturnsCurrencyNotSupportedError(ApiResponse response)
            {
                AssertSingleError(response, ErrorCodes.CurrencyNotSupported, "Currency 'QQQ' is not supported.", "QQQ");
            }

            private void ThenReturnsPaymentProviderNotSupportedError(ApiResponse response)
            {
                AssertSingleError(response, ErrorCodes.PaymentProviderNotSupported, "Payment provider 'ABC' is not supported.", "ABC");
            }

            private void ThenReturnsMissingMerchantAccountIdentifierError(ApiResponse response)
            {
                AssertSingleError(response, 
                                  "merchantaccountidentifier-required", 
                                  "The MerchantAccountIdentifier field is required.");
            }

            private void ThenReturnsMissingMerchantAccountIdentifierFormatError(ApiResponse response)
            {
                AssertSingleError(response, 
                                  ErrorCodes.MerchantAccountIdentifierFormatInvalid,
                                  "The MerchantAccountIdentifier field is not in a valid format.");
            }

            private void ThenUpdateTheBusinessWithoutPaymentProvider(ApiResponse response, ApiBusinessSaveCommand command, SetupData setup)
            {
                var responseBusiness = AssertSuccessResponse<BusinessData>(response);

                Assert.That(responseBusiness.id, Is.EqualTo(setup.Business.Id));
                Assert.That(responseBusiness.name, Is.EqualTo(command.name));
                Assert.That(responseBusiness.domain, Is.EqualTo(setup.Business.Domain));
                Assert.That(responseBusiness.sport, Is.EqualTo(setup.Business.Sport));
                Assert.That(responseBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(responseBusiness.payment.useProRataPricing, Is.EqualTo(command.payment.useProRataPricing));
                Assert.That(responseBusiness.payment.paymentProvider, Is.Null);

                var getResponse = AuthenticatedGet<BusinessData>("Business", setup);
                var getBusiness = (BusinessData) getResponse.Payload;

                Assert.That(getBusiness.id, Is.EqualTo(setup.Business.Id));
                Assert.That(getBusiness.name, Is.EqualTo(command.name));
                Assert.That(getBusiness.domain, Is.EqualTo(setup.Business.Domain));
                Assert.That(getBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(getBusiness.payment.useProRataPricing, Is.EqualTo(command.payment.useProRataPricing));
                Assert.That(getBusiness.payment.paymentProvider, Is.Null);
            }

            private void ThenUpdateTheBusinessWithPaymentOptionsSet(ApiResponse response, ApiBusinessSaveCommand command, SetupData setup)
            {
                var responseBusiness = AssertSuccessResponse<BusinessData>(response);

                Assert.That(responseBusiness.id, Is.EqualTo(setup.Business.Id));
                Assert.That(responseBusiness.name, Is.EqualTo(command.name));
                Assert.That(responseBusiness.domain, Is.EqualTo(command.domain));
                Assert.That(responseBusiness.sport, Is.EqualTo(command.sport));
                Assert.That(responseBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(responseBusiness.payment.isOnlinePaymentEnabled, Is.EqualTo(command.payment.isOnlinePaymentEnabled));
                Assert.That(responseBusiness.payment.forceOnlinePayment, Is.EqualTo(command.payment.forceOnlinePayment));
                Assert.That(responseBusiness.payment.paymentProvider, Is.EqualTo(command.payment.paymentProvider));
                Assert.That(responseBusiness.payment.merchantAccountIdentifier, Is.EqualTo(command.payment.merchantAccountIdentifier));
                Assert.That(responseBusiness.payment.useProRataPricing, Is.EqualTo(command.payment.useProRataPricing));

                var getResponse = AuthenticatedGet<BusinessData>("Business", setup);
                var getBusiness = (BusinessData)getResponse.Payload;

                Assert.That(getBusiness.id, Is.EqualTo(setup.Business.Id));
                Assert.That(getBusiness.name, Is.EqualTo(command.name));
                Assert.That(getBusiness.domain, Is.EqualTo(command.domain));
                Assert.That(getBusiness.sport, Is.EqualTo(command.sport));
                Assert.That(getBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(getBusiness.payment.isOnlinePaymentEnabled, Is.EqualTo(command.payment.isOnlinePaymentEnabled));
                Assert.That(getBusiness.payment.forceOnlinePayment, Is.EqualTo(command.payment.forceOnlinePayment));
                Assert.That(getBusiness.payment.paymentProvider, Is.EqualTo(command.payment.paymentProvider));
                Assert.That(getBusiness.payment.merchantAccountIdentifier, Is.EqualTo(command.payment.merchantAccountIdentifier));
                Assert.That(getBusiness.payment.useProRataPricing, Is.EqualTo(command.payment.useProRataPricing));
            }

            private void ThenUpdateTheBusinessWithChanges(ApiResponse response, ApiBusinessSaveCommand command, SetupData setup)
            {
                var responseBusiness = AssertSuccessResponse<BusinessData>(response);

                Assert.That(responseBusiness.id, Is.EqualTo(setup.Business.Id));
                Assert.That(responseBusiness.name, Is.EqualTo(command.name));
                Assert.That(responseBusiness.domain, Is.EqualTo(command.domain.ToLowerInvariant()));
                Assert.That(responseBusiness.sport, Is.EqualTo(command.sport));
                Assert.That(responseBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(responseBusiness.payment.isOnlinePaymentEnabled, Is.EqualTo(command.payment.isOnlinePaymentEnabled));
                Assert.That(responseBusiness.payment.forceOnlinePayment, Is.EqualTo(command.payment.forceOnlinePayment));
                Assert.That(responseBusiness.payment.paymentProvider, Is.EqualTo(command.payment.paymentProvider));
                Assert.That(responseBusiness.payment.merchantAccountIdentifier, Is.EqualTo(command.payment.merchantAccountIdentifier));
                Assert.That(responseBusiness.payment.useProRataPricing, Is.EqualTo(command.payment.useProRataPricing));

                var getResponse = AuthenticatedGet<BusinessData>("Business", setup);
                var getBusiness = (BusinessData)getResponse.Payload;

                Assert.That(getBusiness.id, Is.EqualTo(setup.Business.Id));
                Assert.That(getBusiness.name, Is.EqualTo(command.name));
                Assert.That(getBusiness.domain, Is.EqualTo(command.domain.ToLowerInvariant()));
                Assert.That(getBusiness.sport, Is.EqualTo(command.sport));
                Assert.That(getBusiness.payment.currency, Is.EqualTo(command.payment.currency));
                Assert.That(getBusiness.payment.isOnlinePaymentEnabled, Is.EqualTo(command.payment.isOnlinePaymentEnabled));
                Assert.That(getBusiness.payment.forceOnlinePayment, Is.EqualTo(command.payment.forceOnlinePayment));
                Assert.That(getBusiness.payment.paymentProvider, Is.EqualTo(command.payment.paymentProvider));
                Assert.That(getBusiness.payment.merchantAccountIdentifier, Is.EqualTo(command.payment.merchantAccountIdentifier));
                Assert.That(getBusiness.payment.useProRataPricing, Is.EqualTo(command.payment.useProRataPricing));
            }
        }


        private ApiResponse WhenTryUpdateBusiness(ApiBusinessSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryUpdateBusiness(json, setup);
        }

        private ApiResponse WhenTryUpdateBusiness(string json, SetupData setup)
        {
            return AuthenticatedPost<BusinessData>(json, RelativePath, setup);
        }

        private ApiResponse WhenTryUpdateBusinessAnonymously(ApiBusinessSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryUpdateBusinessAnonymously(json, setup);
        }

        private ApiResponse WhenTryUpdateBusinessAnonymously(string json, SetupData setup)
        {
            return BusinessAnonymousPost<BusinessData>(json, RelativePath, setup);
        }


        private void ThenReturnNoDataError(ApiResponse response)
        {
            AssertSingleError(response, ErrorCodes.DataRequired, "Please post us some data!");
        }

        private void ThenReturnRootRequiredErrorResponse(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "name-required", "The Name field is required.", null },
                                                    { "domain-required", "The Domain field is required.", null },
                                                    { "payment-required", "The Payment field is required.", null } });
        }
    }
}
