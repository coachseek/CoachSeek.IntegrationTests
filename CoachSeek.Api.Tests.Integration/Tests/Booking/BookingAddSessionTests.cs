using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class BookingAddSessionTests : BaseBookingAddSessionTests
    {
        [SetUp]
        public void Setup()
        {
            FullySetupNewTestBusiness();
        }


        [Test]
        public void GivenNonExistentSession_WhenTryBookSession_ThenReturnNonExistentSessionError()
        {
            var command = GivenNonExistentSession();
            var response = WhenTryBookSession(command);
            ThenReturnNonExistentSessionError(response);
        }

        [Test]
        public void GivenNonExistentCustomer_WhenTryBookSession_ThenReturnNonExistentCustomerError()
        {
            var command = GivenNonExistentCustomer();
            var response = WhenTryBookSession(command);
            ThenReturnNonExistentCustomerError(response);
        }

        [Test]
        public void GivenNonExistentSessionAndCustomer_WhenTryBookSession_ThenReturnNonExistentSessionErrorOnly()
        {
            var command = GivenNonExistentSessionAndCustomer();
            var response = WhenTryBookSession(command);
            ThenReturnNonExistentSessionError(response);
        }

        [Test]
        public void GivenThisCustomerIsAlreadyBookedOntoThisSession_WhenTryBookSession_ThenReturnDuplicateBookingError()
        {
            var command = GivenThisCustomerIsAlreadyBookedOntoThisSession();
            var response = WhenTryBookSession(command);
            ThenReturnDuplicateBookingError(response);
        }

        [Test]
        public void GivenSessionIsNotOnlineBookable_WhenTryBookSession_ThenCreateSessionBooking()
        {
            var command = GivenSessionIsNotOnlineBookable();
            var response = WhenTryBookSession(command);
            ThenCreateSessionBooking(response, AaronOrakei16To17, Wilma);
        }

        [Test]
        public void GivenSessionIsOnlineBookable_WhenTryBookSession_ThenCreateSessionBooking()
        {
            var command = GivenSessionIsOnlineBookable();
            var response = WhenTryBookSession(command);
            ThenCreateSessionBooking(response, AaronOrakei14To15, Wilma, 3);
        }
    }
}
