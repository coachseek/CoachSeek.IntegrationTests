using CoachSeek.Api.Tests.Integration.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class BookingSaveCommandTests : BaseBookingAddTests
    {
        [SetUp]
        public void Setup()
        {
            RegisterTestBusiness();
        }


        [Test]
        public void GivenNoBookingSaveCommand_WhenTryBookSession_ThenReturnNoDataErrorResponse()
        {
            var command = GivenNoBookingSaveCommand();
            var response = WhenTryBookSession(command);
            AssertSingleError(response, "Please post us some data!");
        }

        [Test]
        public void GivenEmptyBookingSaveCommand_WhenTryBookSession_ThenReturnMultipleErrors()
        {
            var command = GivenEmptyBookingSaveCommand();
            var response = WhenTryBookSession(command);
            AssertMultipleErrors(response, new[,] { { "The session field is required.", "booking.session" },
                                                        { "The customer field is required.", "booking.customer" } });
        }

        [Test]
        public void GivenValidBookingSaveCommand_WhenTryBookAnonymously_ThenReturnUnauthorised()
        {
            FullySetupExistingTestBusiness();
            var command = GivenValidBookingSaveCommand();
            var response = WhenTryBookAnonymously(command);
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

        private string GivenValidBookingSaveCommand()
        {
            var command = new ApiBookingSaveCommand
            {
                session = new ApiSessionKey { id = AaronOrakei14To15.Id },
                customer = new ApiCustomerKey { id = Wilma.Id }
            };

            return JsonConvert.SerializeObject(command);
        }


        private Response WhenTryBookAnonymously(string json)
        {
            return PostAnonymouslyToBusiness<BookingData>(json);
        }
    }
}
