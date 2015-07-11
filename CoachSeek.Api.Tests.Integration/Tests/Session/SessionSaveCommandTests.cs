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
            AssertSingleError(response, "Please post us some data!");
        }

        [Test]
        public void GivenEmptySessionSaveCommand_WhenPostSession_ThenReturnMultipleErrors()
        {
            var setup = RegisterBusiness();

            var command = GivenEmptySessionSaveCommand();
            var response = WhenPostSession(command, setup);
            AssertMultipleErrors(response, new[,] { { "The service field is required.", "session.service" },
                                                    { "The location field is required.", "session.location" },
                                                    { "The coach field is required.", "session.coach" },
                                                    { "The timing field is required.", "session.timing" },
                                                    { "The booking field is required.", "session.booking" },
                                                    { "The pricing field is required.", "session.pricing" },
                                                    { "The repetition field is required.", "session.repetition" },
                                                    { "The presentation field is required.", "session.presentation" } });
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
