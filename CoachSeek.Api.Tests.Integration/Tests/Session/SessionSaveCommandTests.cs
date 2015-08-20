﻿using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class SessionSaveCommandTests : ScheduleTests
    {
        [Test]
        public void GivenNoSessionSaveCommand_WhenPostSession_ThenReturnNoDataError()
        {
            var setup = RegisterBusiness();

            var command = GivenNoSessionSaveCommand();
            var response = WhenPostSession(command, setup);
            AssertSingleError(response, ErrorCodes.DataMissing, "Please post us some data!");
        }

        [Test]
        public void GivenEmptySessionSaveCommand_WhenPostSession_ThenReturnMultipleErrors()
        {
            var setup = RegisterBusiness();

            var command = GivenEmptySessionSaveCommand();
            var response = WhenPostSession(command, setup);
            AssertMultipleErrors(response, new[,] { { "service-required", "The Service field is required.", null, null },
                                                    { "location-required", "The Location field is required.", null, null },
                                                    { "coach-required", "The Coach field is required.", null, null },
                                                    { "timing-required", "The Timing field is required.", null, null },
                                                    { "booking-required", "The Booking field is required.", null, null },
                                                    { "pricing-required", "The Pricing field is required.", null, null },
                                                    { "repetition-required", "The Repetition field is required.", null, null },
                                                    { "presentation-required", "The Presentation field is required.", null, null } });
        }


        private string GivenNoSessionSaveCommand()
        {
            return "";
        }

        private string GivenEmptySessionSaveCommand()
        {
            return "{}";
        }
    }
}
