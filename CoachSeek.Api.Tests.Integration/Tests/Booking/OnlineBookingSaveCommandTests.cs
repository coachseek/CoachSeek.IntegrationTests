using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class OnlineBookingSaveCommandTests : BaseBookingAddTests
    {
        [SetUp]
        public void Setup()
        {
            FullySetupNewTestBusiness();
        }


        [Test]
        public void GivenNoBookingSaveCommand_WhenTryBookOnlineSession_ThenReturnNoDataError()
        {
            var command = GivenNoBookingSaveCommand();
            var response = WhenTryBookOnlineSession(command);
            AssertSingleError(response, "Please post us some data!");
        }

        [Test]
        public void GivenEmptyBookingSaveCommand_WhenTryBookOnlineSession_ThenReturnMultipleErrors()
        {
            var command = GivenEmptyBookingSaveCommand();
            var response = WhenTryBookOnlineSession(command);
            AssertMultipleErrors(response, new[,] { { "The session field is required.", "booking.session" },
                                                        { "The customer field is required.", "booking.customer" } });
        }


        protected string GivenNoBookingSaveCommand()
        {
            return "";
        }

        protected string GivenEmptyBookingSaveCommand()
        {
            return "{}";
        }
    }
}
