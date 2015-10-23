using System;
using System.Net;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Coach
{
    public class CoachPostTests : CoachTests
    {
        [TestFixture]
        public class CoachCommandTests : CoachPostTests
        {
            [Test]
            public void GivenNoCoachSaveCommand_WhenTryPost_ThenReturnNoDataError()
            {
                var setup = RegisterBusiness();

                var command = GivenNoCoachSaveCommand();
                var response = WhenTryPost(command, setup);
                ThenReturnNoDataError(response);
            }

            [Test]
            public void GivenEmptyCoachSaveCommand_WhenTryPost_ThenReturnRootRequiredError()
            {
                var setup = RegisterBusiness();

                var command = GivenEmptyCoachSaveCommand();
                var response = WhenTryPost(command, setup);
                ThenReturnRootRequiredError(response);
            }


            private string GivenNoCoachSaveCommand()
            {
                return "";
            }

            private string GivenEmptyCoachSaveCommand()
            {
                return "{}";
            }
        }


        [TestFixture]
        public class CoachNewTests : CoachPostTests
        {
            [Test]
            public void GivenMissingWorkingHoursProperties_WhenTryPost_ThenReturnMissingWorkingHoursPropertyError()
            {
                var setup = RegisterBusiness();

                var command = GivenMissingWorkingHoursProperties();
                var response = WhenTryPost(command, setup);
                ThenReturnMissingWorkingHoursPropertyError(response);
            }

            [Test]
            public void GivenInvalidWorkingHoursProperties_WhenTryPost_ThenReturnInvalidWorkingHoursPropertyError()
            {
                var setup = RegisterBusiness();

                var command = GivenInvalidWorkingHoursProperties();
                var response = WhenTryPost(command, setup);
                ThenReturnInvalidWorkingHoursPropertyError(response);
            }

            [Test]
            public void GivenNewCoachWithAnAlreadyExistingCoachName_WhenTryPost_ThenReturnDuplicateCoachError()
            {
                var setup = RegisterBusiness();
                RegisterCoachAaron(setup);

                var command = GivenNewCoachWithAnAlreadyExistingCoachName(setup);
                var response = WhenTryPost(command, setup);
                ThenReturnDuplicateCoachError(response);
            }

            [Test]
            public void GivenValidNewCoach_WhenTryPost_ThenReturnNewCoachResponse()
            {
                var setup = RegisterBusiness();

                var command = GivenValidNewCoach();
                var response = WhenTryPost(command, setup);
                ThenReturnNewCoachResponse(response);
            }


            private string GivenMissingWorkingHoursProperties()
            {
                var command = new ApiCoachSaveCommand
                {
                    firstName = Random.RandomString,
                    lastName = Random.RandomString,
                    email = Random.RandomEmail,
                    phone = Random.RandomString,
                    workingHours = new ApiWeeklyWorkingHours
                    {
                        monday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                        thursday = new ApiDailyWorkingHours(),
                        friday = new ApiDailyWorkingHours(true, "10:00", "18:00"),
                        saturday = new ApiDailyWorkingHours()
                    }
                };

                return JsonSerialiser.Serialise(command);
            }

            private string GivenInvalidWorkingHoursProperties()
            {
                var command = new ApiCoachSaveCommand
                {
                    firstName = Random.RandomString,
                    lastName = Random.RandomString,
                    email = Random.RandomEmail,
                    phone = Random.RandomString,
                    workingHours = new ApiWeeklyWorkingHours
                    {
                        monday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                        tuesday = new ApiDailyWorkingHours(),
                        wednesday = new ApiDailyWorkingHours(true, "-4:17", "23:77"),
                        thursday = new ApiDailyWorkingHours(),
                        friday = new ApiDailyWorkingHours(true, "hello", "world"),
                        saturday = new ApiDailyWorkingHours(),
                        sunday = new ApiDailyWorkingHours()
                    }
                };

                return JsonSerialiser.Serialise(command);
            }

            private string GivenNewCoachWithAnAlreadyExistingCoachName(SetupData setup)
            {
                var command = new ApiCoachSaveCommand
                {
                    firstName = setup.Aaron.FirstName,
                    lastName = setup.Aaron.LastName,
                    email = Random.RandomEmail,
                    phone = Random.RandomString,
                    workingHours = SetupStandardWorkingHours()
                };

                return JsonSerialiser.Serialise(command);
            }

            private string GivenValidNewCoach()
            {
                var command = new ApiCoachSaveCommand
                {
                    firstName = "Carl",
                    lastName = "Carson",
                    email = "Carl@CoachMaster.com",
                    phone = "021 69 69 69",
                    workingHours = SetupStandardWorkingHours()
                };

                command.workingHours.sunday = new ApiDailyWorkingHours(false, "10:30", "15:45");

                return JsonSerialiser.Serialise(command);
            }
        }

        [TestFixture]
        public class CoachExistingTests : CoachPostTests
        {
            [Test]
            public void GivenNonExistentCoachId_WhenTryPost_ThenReturnInvalidCoachIdErrorResponse()
            {
                var setup = RegisterBusiness();

                var command = GivenNonExistentCoachId();
                var response = WhenTryPost(command, setup);
                ThenReturnInvalidCoachIdError(response);
            }

            [Test]
            public void GivenWantToUpdateExistingCoach_WhenTryPost_ThenReturnUpdatedCoachResponse()
            {
                var setup = RegisterBusiness();
                RegisterCoachAaron(setup);

                var command = GivenWantToUpdateExistingCoach(setup);
                var response = WhenTryPost(command, setup);
                ThenReturnUpdatedCoachResponse(response, setup);
            }


            private string GivenNonExistentCoachId()
            {
                var command = new ApiCoachSaveCommand
                {
                    id = Guid.Empty,
                    firstName = Random.RandomString,
                    lastName = Random.RandomString,
                    email = Random.RandomEmail,
                    phone = Random.RandomString,
                    workingHours = SetupStandardWorkingHours()
                };

                return JsonSerialiser.Serialise(command);
            }

            private string GivenWantToUpdateExistingCoach(SetupData setup)
            {
                var command = new ApiCoachSaveCommand
                {
                    id = setup.Aaron.Id,
                    firstName = "Adam",
                    lastName = "Ant",
                    email = "Adam@ant.net",
                    phone = "021 0123456",
                    workingHours = SetupWeekendWorkingHours()
                };

                return JsonSerialiser.Serialise(command);
            }


            private void ThenReturnUpdatedCoachResponse(ApiResponse response, SetupData setup)
            {
                Assert.That(response, Is.Not.Null);
                AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

                Assert.That(response.Payload, Is.InstanceOf<CoachData>());
                var coach = (CoachData)response.Payload;

                Assert.That(coach.id, Is.EqualTo(setup.Aaron.Id));
                Assert.That(coach.firstName, Is.EqualTo("Adam"));
                Assert.That(coach.lastName, Is.EqualTo("Ant"));
                Assert.That(coach.email, Is.EqualTo("adam@ant.net"));
                Assert.That(coach.phone, Is.EqualTo("021 0123456"));

                Assert.That(coach.workingHours, Is.Not.Null);
                AssertStandardWeekendDay(coach.workingHours.monday);
                AssertStandardWeekendDay(coach.workingHours.tuesday);
                AssertStandardWeekendDay(coach.workingHours.wednesday);
                AssertStandardWeekendDay(coach.workingHours.thursday);
                AssertStandardWeekendDay(coach.workingHours.friday);
                AssertStandardWorkingDay(coach.workingHours.saturday);
                AssertStandardWorkingDay(coach.workingHours.sunday);
            }
        }


        protected ApiResponse WhenTryPost(string json, SetupData setup)
        {
            return AuthenticatedPost<CoachData>(json, RelativePath, setup);
        }


        private void ThenReturnNoDataError(ApiResponse response)
        {
            AssertSingleError(response, ErrorCodes.DataRequired, "Please post us some data!");
        }

        private void ThenReturnRootRequiredError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "firstname-required", "The FirstName field is required.", null },
                                                    { "lastname-required", "The LastName field is required.", null },
                                                    { "email-required", "The Email field is required.", null },
                                                    { "phone-required", "The Phone field is required.", null },
                                                    { "workinghours-required", "The WorkingHours field is required.", null } });
        }

        private void ThenReturnMissingWorkingHoursPropertyError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { "tuesday-required", "The Tuesday field is required.", null },
                                                    { "wednesday-required", "The Wednesday field is required.", null },
                                                    { "sunday-required", "The Sunday field is required.", null } });
        }

        private void ThenReturnInvalidWorkingHoursPropertyError(ApiResponse response)
        {
            AssertMultipleErrors(response, new[,] { { ErrorCodes.DailyWorkingHoursInvalid, "Wednesday working hours are not valid.", "Wednesday", null },
                                                    { ErrorCodes.DailyWorkingHoursInvalid, "Friday working hours are not valid.", "Friday", null } });
        }

        private void ThenReturnInvalidCoachIdError(ApiResponse response)
        {
            AssertSingleError(response, ErrorCodes.CoachInvalid, "This coach does not exist.", Guid.Empty.ToString());
        }

        private void ThenReturnDuplicateCoachError(ApiResponse response)
        {
            AssertSingleError(response, ErrorCodes.CoachDuplicate, "Coach 'Aaron Smith' already exists.", "Aaron Smith");
        }

        private void ThenReturnNewCoachResponse(ApiResponse response)
        {
            Assert.That(response, Is.Not.Null);
            AssertStatusCode(response.StatusCode, HttpStatusCode.OK);

            Assert.That(response.Payload, Is.InstanceOf<CoachData>());
            var coach = (CoachData)response.Payload;

            Assert.That(coach.id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(coach.firstName, Is.EqualTo("Carl"));
            Assert.That(coach.lastName, Is.EqualTo("Carson"));
            Assert.That(coach.email, Is.EqualTo("carl@coachmaster.com"));
            Assert.That(coach.phone, Is.EqualTo("021 69 69 69"));

            Assert.That(coach.workingHours, Is.Not.Null);
            AssertStandardWorkingDay(coach.workingHours.monday);
            AssertStandardWorkingDay(coach.workingHours.tuesday);
            AssertStandardWorkingDay(coach.workingHours.wednesday);
            AssertStandardWorkingDay(coach.workingHours.thursday);
            AssertStandardWorkingDay(coach.workingHours.friday);
            AssertStandardWeekendDay(coach.workingHours.saturday);
            AssertWorkingHours(coach.workingHours.sunday, false, "10:30", "15:45");
        }

        private void AssertStandardWorkingDay(DailyWorkingHoursData workingDay)
        {
            Assert.That(workingDay, Is.Not.Null);
            Assert.That(workingDay.isAvailable, Is.True);
            Assert.That(workingDay.startTime, Is.EqualTo("9:00"));
            Assert.That(workingDay.finishTime, Is.EqualTo("17:00"));
        }

        private void AssertStandardWeekendDay(DailyWorkingHoursData workingDay)
        {
            Assert.That(workingDay, Is.Not.Null);
            Assert.That(workingDay.isAvailable, Is.False);
            Assert.That(workingDay.startTime, Is.Null);
            Assert.That(workingDay.finishTime, Is.Null);
        }

        private void AssertWorkingHours(DailyWorkingHoursData workingDay, 
                                        bool isAvailable, 
                                        string startTime, 
                                        string finishTime)
        {
            Assert.That(workingDay, Is.Not.Null);
            Assert.That(workingDay.isAvailable, Is.EqualTo(isAvailable));
            Assert.That(workingDay.startTime, Is.EqualTo(startTime));
            Assert.That(workingDay.finishTime, Is.EqualTo(finishTime));
        }
    }
}
