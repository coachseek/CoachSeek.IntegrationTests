using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
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
            AssertSingleError(response, "Please post us some data!");
        }

        [Test]
        public void GivenEmptyBookingSaveCommand_WhenTryBookSession_ThenReturnMultipleErrors()
        {
            var setup = RegisterBusiness();

            var command = GivenEmptyBookingSaveCommand();
            var response = WhenTryBookSession(command, setup);
            AssertMultipleErrors(response, new[,] { { "The sessions field is required.", "booking.sessions" },
                                                    { "The customer field is required.", "booking.customer" } });
        }

        [Test]
        public void GivenNoSessionsOnBookingSaveCommand_WhenTryBookSession_ThenReturnNoSessionsErrors()
        {
            var setup = RegisterBusiness();
            RegisterCustomerWilma(setup);

            var command = GivenNoSessionsOnBookingSaveCommand(setup);
            var response = WhenTryBookSession(command, setup);
            AssertSingleError(response, "A booking must have at least one session.");
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


        private ApiResponse WhenTryBookSessionAnonymously(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonConvert.SerializeObject(command);
            return new TestBusinessAnonymousApiClient().Post<BookingData>(json,
                                                                          setup.Business.Domain,
                                                                          RelativePath);
        }
    }
}
