using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class OnlineBookingSaveCommandTests : BaseBookingAddTests
    {
        [Test]
        public void GivenNoBookingSaveCommand_WhenTryBookOnlineSession_ThenReturnNoDataError()
        {
            var setup = RegisterBusiness();

            var command = GivenNoBookingSaveCommand();
            var response = WhenTryOnlineBookSession(command, setup);
            AssertSingleError(response, ErrorCodes.DataMissing, "Please post us some data!");
        }

        [Test]
        public void GivenEmptyBookingSaveCommand_WhenTryBookOnlineSession_ThenReturnMultipleErrors()
        {
            var setup = RegisterBusiness();

            var command = GivenEmptyBookingSaveCommand();
            var response = WhenTryOnlineBookSession(command, setup);
            AssertMultipleErrors(response, new[,] { { "sessions-required", "The Sessions field is required.", null },
                                                    { "customer-required", "The Customer field is required.", null } });
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
