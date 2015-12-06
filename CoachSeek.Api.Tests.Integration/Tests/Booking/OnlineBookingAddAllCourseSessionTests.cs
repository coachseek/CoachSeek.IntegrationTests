using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    [TestFixture]
    public class OnlineBookingAddAllCourseSessionTests : BaseBookingAddSessionTests
    {
        [Test]
        public void GivenDuplicateSessions_WhenTryOnlineBookAllCourseSessions_ThenReturnDuplicateSessionError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenDuplicateSessions(setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenReturnDuplicateSessionError(response, setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id);
        }

        [Test]
        public void GivenASessionIsStandalone_WhenTryOnlineBookAllCourseSessions_ThenReturnSessionNotInCourseError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerFred(setup);

            var command = GivenASessionIsStandalone(setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiMiniRed14To15.Id);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenReturnSessionNotInCourseError(response, 
                                              setup.AaronOrakeiMiniRed14To15.Id,
                                              setup.AaronOrakeiHolidayCamp9To15For3Days.Id);
        }

        [Test]
        public void GivenASessionBelongsToADifferentCourse_WhenTryOnlineBookAllCourseSessions_ThenReturnSessionNotInCourseError()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCourseBobbyRemueraMiniRed9To10For3Weeks(setup);
            RegisterCustomerFred(setup);

            var command = GivenASessionBelongsToADifferentCourse(setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[0].Id);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenReturnSessionNotInCourseError(response,
                                              setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[0].Id,
                                              setup.AaronOrakeiHolidayCamp9To15For3Days.Id);
        }

        [Test]
        public void GivenACourseSessionIsFull_WhenTryOnlineBookAllCourseSessions_ThenReturnCourseSessionFullError()
        {
            var setup = RegisterBusiness();
            RegisterFullyBookedLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerBambam(setup);

            var command = GivenACourseSessionIsFull(setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenReturnCourseSessionFullError(response, command.sessions[2].id.GetValueOrDefault());
        }

        [Test]
        public void GivenTheCourseIsNotOnlineBookable_WhenTryOnlineBookAllCourseSessions_ThenReturnCourseSessionIsNotOnlineBookableError()
        {
            var setup = RegisterBusiness();
            RegisterCourseBobbyRemueraMiniRed9To10For3Weeks(setup, false);
            RegisterCustomerFred(setup);

            var command = GivenTheCourseIsNotOnlineBookable(setup, 
                setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[0].Id,
                setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[1].Id,
                setup.BobbyRemueraMiniRed9To10For3Weeks.Sessions[2].Id);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenReturnCourseSessionIsNotOnlineBookableError(response, setup.BobbyRemueraMiniRed9To10For3Weeks.Id);
        }

        [Test]
        public void GivenTheCourseIsOnlineBookable_WhenTryOnlineBookAllCourseSessions_ThenCreateAllCourseSessionBookings()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenTheCourseIsOnlineBookable(setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenCreateAllCourseSessionBookings(response, setup);
        }

        [Test]
        public void GivenValidAllCourseSessionsOutOfOrder_WhenTryOnlineBookAllCourseSessions_ThenCreateAllCourseSessionBookings()
        {
            var setup = RegisterBusiness();
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var command = GivenValidAllCourseSessionsOutOfOrder(setup,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id);
            var response = WhenTryOnlineBookAllCourseSessions(command, setup);
            ThenCreateAllCourseSessionBookings(response, setup);
        }


        private ApiResponse WhenTryOnlineBookAllCourseSessions(ApiBookingSaveCommand command, SetupData setup)
        {
            var json = JsonSerialiser.Serialise(command);
            return WhenTryOnlineBookCourse(json, setup);
        }

        private void ThenCreateAllCourseSessionBookings(ApiResponse response, SetupData setup)
        {
            ThenCreateAllCourseSessionBookings(response, setup, true);
        }
    }
}
