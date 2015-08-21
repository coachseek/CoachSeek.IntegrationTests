using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class BookingSaveCommandTests : BaseBookingAddTests
    {
        [Test]
        public void GivenNoBookingSaveCommand_WhenTryBookSession_ThenReturnNoDataErrorResponse()
        {
            var setup = RegisterBusiness();

            var command = GivenNoBookingSaveCommand();
            var response = WhenTryBookSession(command, setup);
            AssertSingleError(response, ErrorCodes.DataMissing, "Please post us some data!");
        }

        [Test]
        public void GivenEmptyBookingSaveCommand_WhenTryBookSession_ThenReturnMultipleErrors()
        {
            var setup = RegisterBusiness();

            var command = GivenEmptyBookingSaveCommand();
            var response = WhenTryBookSession(command, setup);
            AssertMultipleErrors(response, new[,] { { "sessions-required", "The Sessions field is required.", null },
                                                    { "customer-required", "The Customer field is required.", null } });
        }

        [Test]
        public void GivenNoSessionsOnBookingSaveCommand_WhenTryBookSession_ThenReturnNoSessionsErrors()
        {
            var setup = RegisterBusiness();
            RegisterCustomerWilma(setup);

            var command = GivenNoSessionsOnBookingSaveCommand(setup);
            var response = WhenTryBookSession(command, setup);
            AssertSingleError(response, ErrorCodes.BookingSessionRequired, "A booking must have at least one session.", null);
        }

        [Test]
        public void GivenValidBookingSaveCommand_WhenTryBookSessionAnonymously_ThenReturnUnauthorised()
        {
            var setup = RegisterBusiness();
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerWilma(setup);

            var command = GivenValidBookingSaveCommand(setup);
            var response = WhenTryBookSessionAnonymously(command, setup);
            AssertUnauthorised(response);
        }


        protected string GivenNoBookingSaveCommand()
        {
            return "";
        }

        protected string GivenEmptyBookingSaveCommand()
        {
            return "{}";
        }

        private ApiBookingSaveCommand GivenNoSessionsOnBookingSaveCommand(SetupData setup)
        {
            return new ApiBookingSaveCommand {customer = new ApiCustomerKey {id = setup.Wilma.Id}};
        }

        private ApiBookingSaveCommand GivenValidBookingSaveCommand(SetupData setup)
        {
            return new ApiBookingSaveCommand(setup.AaronOrakeiMiniRed14To15.Id, setup.Wilma.Id);
        }
    }
}
