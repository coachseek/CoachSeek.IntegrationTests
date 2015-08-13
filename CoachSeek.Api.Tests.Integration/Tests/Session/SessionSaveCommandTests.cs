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
            AssertMultipleErrors(response, new[,] { { null, "The service field is required.", null, "session.service" },
                                                    { null, "The location field is required.", null, "session.location" },
                                                    { null, "The coach field is required.", null, "session.coach" },
                                                    { null, "The timing field is required.", null, "session.timing" },
                                                    { null, "The booking field is required.", null, "session.booking" },
                                                    { null, "The pricing field is required.", null, "session.pricing" },
                                                    { null, "The repetition field is required.", null, "session.repetition" },
                                                    { null, "The presentation field is required.", null, "session.presentation" } });
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
