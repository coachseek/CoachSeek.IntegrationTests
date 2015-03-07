using System;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Session
{
    [TestFixture]
    public class CourseUpdateExistingTests : ScheduleTests
    {
        [SetUp]
        public void Setup()
        {
            SetupFullTestBusiness();
        }


        [Test]
        public void GivenNonExistentCourseId_WhenPost_ThenReturnNotFoundResponse()
        {
            var command = GivenNonExistentCourseId();
            var response = WhenPost(command);
            AssertNotFound(response);
        }

        [Test]
        public void GivenWantTurnCourseIntoSession_WhenPost_ThenReturnsCannotUpdateCourseError()
        {
            var command = GivenWantToTurnCourseIntoSession();
            var response = WhenPost(command);
            ThenReturnsCannotUpdateCourseError(response);
        }

        [Test]
        public void GivenWantToChangeRepetitionOfCourse_WhenPost_ThenReturnsCannotUpdateCourseError()
        {
            var command = GivenWantToChangeRepetitionOfCourse();
            var response = WhenPost(command);
            ThenReturnsCannotUpdateCourseError(response);
        }

        [Test]
        public void GivenWantToUpdateCourse_WhenPost_ThenReturnsCannotUpdateCourseError()
        {
            var command = GivenWantToUpdateCourse();
            var response = WhenPost(command);
            ThenReturnsCannotUpdateCourseError(response);
        }


        private ApiSessionSaveCommand GivenNonExistentCourseId()
        {
            var course = CreateSessionSaveCommandAaronRemuera9To10For8Weeks();
            course.id = Guid.NewGuid();

            return course;
        }

        private ApiSessionSaveCommand GivenWantToTurnCourseIntoSession()
        {
            var courseCommand = CreateSessionSaveCommandAaronRemuera9To10For8Weeks();

            courseCommand.id = AaronRemuera9To10For8WeeksCourseId;
            courseCommand.repetition = new ApiRepetition { sessionCount = 1 };

            return courseCommand;
        }

        private ApiSessionSaveCommand GivenWantToChangeRepetitionOfCourse()
        {
            var courseCommand = CreateSessionSaveCommandAaronRemuera9To10For8Weeks();

            courseCommand.id = AaronRemuera9To10For8WeeksCourseId;
            courseCommand.repetition = new ApiRepetition { sessionCount = 5, repeatFrequency = "d" };

            return courseCommand;
        }

        private ApiSessionSaveCommand GivenWantToUpdateCourse()
        {
            var courseCommand = CreateSessionSaveCommandAaronRemuera9To10For8Weeks();

            courseCommand.id = AaronRemuera9To10For8WeeksCourseId;
            courseCommand.location = new ApiLocationKey { id = RemueraId };

            return courseCommand;
        }

        private void ThenReturnsCannotUpdateRepetitionOfCourseError(Response response)
        {
            AssertSingleError(response, "Cannot change the repetition of a course.");
        }

        private void ThenReturnsCannotUpdateCourseError(Response response)
        {
            AssertSingleError(response, "Course updates are not working yet.");
        }
    }
}
