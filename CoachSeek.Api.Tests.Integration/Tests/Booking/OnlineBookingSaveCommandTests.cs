﻿using NUnit.Framework;

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
            AssertSingleError(response, "Please post us some data!");
        }

        [Test]
        public void GivenEmptyBookingSaveCommand_WhenTryBookOnlineSession_ThenReturnMultipleErrors()
        {
            var setup = RegisterBusiness();

            var command = GivenEmptyBookingSaveCommand();
            var response = WhenTryOnlineBookSession(command, setup);
            AssertMultipleErrors(response, new[,] { { null, "The sessions field is required.", null, "booking.sessions" },
                                                    { null, "The customer field is required.", null, "booking.customer" } });
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
