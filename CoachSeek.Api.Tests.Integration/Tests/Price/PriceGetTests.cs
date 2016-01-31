using System;
using System.Collections.Generic;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Price
{
    [TestFixture]
    public class PriceGetTests : ScheduleTests
    {
        protected override string RelativePath { get { return "Pricing/Enquiry"; } }

        private void UpdateSessionPriceOfLastSessionOfAaronOrakeiHolidayCamp9To15For3Days(SetupData setup)
        {
            var command = new ApiSessionSaveCommand(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2])
            {
                id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id,
                pricing = {sessionPrice = 75}
            };
            PostSession(JsonSerialiser.Serialise(command), setup);
        }

        [Test]
        public void GivenNoSessions_WhenTryGetPrice_ThenReturnPricingEnquiryMustHaveAtLeastOneSessionError()
        {
            var setup = RegisterBusiness();

            var command = GivenNoSessions();
            var response = WhenTryGetPrice(command, setup);
            ThenReturnPricingEnquiryMustHaveAtLeastOneSessionError(response);
        }

        [Test]
        public void GivenSingleNonExistentSession_WhenTryGetPrice_ThenReturnNonExistentSessionError()
        {
            var setup = RegisterBusiness();

            var command = GivenSingleNonExistentSession();
            var response = WhenTryGetPrice(command, setup);
            ThenReturnNonExistentSessionError(response, command.sessions[0].id.GetValueOrDefault());
        }

        [Test]
        public void GivenStandaloneSession_WhenTryGetPrice_ThenReturnStandaloneSessionPrice()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);

            var command = GivenStandaloneSession(setup);
            var response = WhenTryGetPrice(command, setup);
            ThenReturnStandaloneSessionPrice(response);
        }


        [Test]
        public void GivenCourseSessionsWithNonExistentSession_WhenTryGetPrice_ThenReturnSessionNotInCourseError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenCourseSessionsWithNonExistentSession(setup);
            var response = WhenTryGetPrice(command, setup);
            ThenReturnSessionNotInCourseError(response, 
                command.sessions[1].id.GetValueOrDefault(), 
                setup.AaronOrakeiHolidayCamp9To15For3Days.Id);
        }

        [Test]
        public void GivenDuplicateCourseSession_WhenTryGetPrice_ThenReturnDuplicateSessionError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenDuplicateCourseSession(setup);
            var response = WhenTryGetPrice(command, setup);
            ThenReturnDuplicateSessionError(response, setup, setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id);
        }

        [Test]
        public void GivenAllSessionsInACourseAndHaveCoursePrice_WhenTryGetPrice_ThenReturnCoursePrice()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenAllSessionsInACourseAndHaveCoursePrice(setup);
            var response = WhenTryGetPrice(command, setup);
            ThenReturnCoursePrice(response, setup);
        }

        [Test]
        public void GivenAllSessionsInACourseAndHaveSessionPriceOnly_WhenTryGetPrice_ThenReturnSumOfSessionPrices()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup, 3, 50, null);
            UpdateSessionPriceOfLastSessionOfAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenAllSessionsInACourseAndHaveSessionPriceOnly(setup);
            var response = WhenTryGetPrice(command, setup);
            ThenReturnSumOfSessionPrices(response, setup);
        }

        [Test]
        public void GivenSomeSessionsInACourseAndHaveSessionPrice_WhenTryGetPrice_ThenReturnSessionPriceTimesSessionNumber()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);

            var command = GivenSomeSessionsInACourseAndHaveSessionPrice(setup);
            var response = WhenTryGetPrice(command, setup);
            ThenReturnSessionPriceTimesSessionNumber(response, setup);
        }

        [Test]
        public void GivenSomeSessionsInACourseAndHaveCoursePriceOnlyAndUseProRataPricingIsOn_WhenTryGetPrice_ThenReturnProRataCoursePrice()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup, 3, null, 200);

            var command = GivenSomeSessionsInACourseAndHaveCoursePriceOnlyAndUseProRataPricingIsOn(setup);
            var response = WhenTryGetPrice(command, setup);
            ThenReturnProRataCoursePrice(response, setup);
        }

        [Test]
        public void GivenSomeSessionsInACourseAndHaveCoursePriceOnlyAndUseProRataPricingIsOff_WhenTryGetPrice_ThenReturnCoursePrice()
        {
            var setup = RegisterBusiness();
            UpdateBusinessSetUseProRataPricingIsFalse(setup);
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup, 3, null, 120);

            var command = GivenSomeSessionsInACourseAndHaveCoursePriceOnlyAndUseProRataPricingIsOff(setup);
            var response = WhenTryGetPrice(command, setup);
            ThenReturnCoursePrice(response, setup);
        }

        private void UpdateBusinessSetUseProRataPricingIsFalse(SetupData setup)
        {
            var business = setup.Business;
            var payment = business.Payment;
            var command = new ApiBusinessSaveCommand
            {
                domain = business.Domain,
                name = business.Name,
                payment = new ApiBusinessPaymentOptions
                {
                    currency = payment.currency,
                    forceOnlinePayment = payment.forceOnlinePayment,
                    isOnlinePaymentEnabled = payment.isOnlinePaymentEnabled,
                    merchantAccountIdentifier = payment.merchantAccountIdentifier,
                    paymentProvider = payment.paymentProvider,
                    useProRataPricing = false
                }
            };
            var json = JsonSerialiser.Serialise(command);
            AuthenticatedPost<BusinessData>(json, "Business", setup);
        }


        private ApiPriceGetCommand GivenNoSessions()
        {
            return new ApiPriceGetCommand
            {
                sessions = new List<ApiSessionKey>()
            };
        }

        private ApiPriceGetCommand GivenSingleNonExistentSession()
        {
            return new ApiPriceGetCommand
            {
                sessions = new[] { new ApiSessionKey { id = Guid.NewGuid() } }
            };
        }

        private ApiPriceGetCommand GivenStandaloneSession(SetupData setup)
        {
            return new ApiPriceGetCommand
            {
                sessions = new[] { new ApiSessionKey { id = setup.AaronOrakeiMiniRed14To15.Id } }
            };
        }

        private ApiPriceGetCommand GivenCourseSessionsWithNonExistentSession(SetupData setup)
        {
            return new ApiPriceGetCommand
            {
                sessions = new[]
                {
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id },
                    new ApiSessionKey { id = Guid.NewGuid() },
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id }
                }
            };
        }

        private ApiPriceGetCommand GivenDuplicateCourseSession(SetupData setup)
        {
            return new ApiPriceGetCommand
            {
                sessions = new[]
                {
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id },
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id },
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id },
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id }
                }
            };
        }

        private ApiPriceGetCommand GivenAllSessionsInACourseAndHaveCoursePrice(SetupData setup)
        {
            return new ApiPriceGetCommand
            {
                sessions = new[]
                {
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id },
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id },
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id }
                }
            };
        }

        private ApiPriceGetCommand GivenAllSessionsInACourseAndHaveSessionPriceOnly(SetupData setup)
        {
            return new ApiPriceGetCommand
            {
                sessions = new[]
                {
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id },
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id },
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id }
                }
            };
        }

        private ApiPriceGetCommand GivenSomeSessionsInACourseAndHaveSessionPrice(SetupData setup)
        {
            return new ApiPriceGetCommand
            {
                sessions = new[]
                {
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id },
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id }
                }
            };
        }

        private ApiPriceGetCommand GivenSomeSessionsInACourseAndHaveCoursePriceOnlyAndUseProRataPricingIsOn(SetupData setup)
        {
            return new ApiPriceGetCommand
            {
                sessions = new[]
                {
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id },
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id }
                }
            };
        }

        private ApiPriceGetCommand GivenSomeSessionsInACourseAndHaveCoursePriceOnlyAndUseProRataPricingIsOff(SetupData setup)
        {
            return new ApiPriceGetCommand
            {
                sessions = new[]
                {
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id },
                    new ApiSessionKey { id = setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id }
                }
            };
        }


        private ApiResponse WhenTryGetPrice(ApiPriceGetCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return BusinessAnonymousPost<decimal>(json, RelativePath, setup);
        }


        private void ThenReturnPricingEnquiryMustHaveAtLeastOneSessionError(ApiResponse response)
        {
            AssertSingleError(response,
                              ErrorCodes.PricingSessionRequired,
                              "A pricing enquiry must have at least one session.");
        }

        protected void ThenReturnNonExistentSessionError(ApiResponse response, Guid sessionId)
        {
            AssertSingleError(response,
                              ErrorCodes.SessionInvalid,
                              "This session does not exist.",
                              sessionId.ToString());
        }


        private void ThenReturnStandaloneSessionPrice(ApiResponse response)
        {
            var price = AssertSuccessResponse<decimal>(response);

            Assert.That(price, Is.EqualTo(19.95m));
        }

        private void ThenReturnCoursePrice(ApiResponse response, SetupData setup)
        {
            var price = AssertSuccessResponse<decimal>(response);

            Assert.That(price, Is.EqualTo(120m));
        }

        private void ThenReturnSumOfSessionPrices(ApiResponse response, SetupData setup)
        {
            var price = AssertSuccessResponse<decimal>(response);

            Assert.That(price, Is.EqualTo(50 + 50 + 75));
        }

        private void ThenReturnSessionPriceTimesSessionNumber(ApiResponse response, SetupData setup)
        {
            var price = AssertSuccessResponse<decimal>(response);

            Assert.That(price, Is.EqualTo(2 * 50));
        }

        private void ThenReturnProRataCoursePrice(ApiResponse response, SetupData setup)
        {
            var price = AssertSuccessResponse<decimal>(response);
            // Note: We're checking that the rounding is on the session price not the booking price.
            var expectedSessionPrice = Math.Round(200m/3, 2);
            Assert.That(price, Is.EqualTo(expectedSessionPrice * 2));
        }
    }
}
