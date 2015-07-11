using System;
using System.Collections.Generic;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Booking;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Coach;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Course;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Location;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Service;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Session;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    public abstract class ScheduleTests : WebIntegrationTest
    {
        protected Guid AaronRemuera9To10For4WeeksCourseId { get; set; }
        protected Guid[] AaronRemuera9To10For4WeeksSessionIds { get; set; }
        protected Guid BobbyRemueraHolidayCampFor3DaysCourseId { get; set; }
        protected Guid[] BobbyRemueraHolidayCampFor3DaysSessionIds { get; set; }
        protected Guid AaronOrakeiMiniBlueFor2DaysCourseId { get; set; }
        protected Guid[] AaronOrakeiMiniBlueFor2DaysSessionIds { get; set; }

        protected Guid FredOnAaronOrakei14To15SessionId { get; set; }
        protected Guid BarneyOnAaronOrakei14To15SessionId { get; set; }
        protected Guid FredOnBobbyRemueraHolidayCampFor3DaysCourseId { get; set; }
        protected Guid FredOnAaronOrakeiMiniBlueFor2DaysCourseId { get; set; }
        protected Guid BarneyOnAaronOrakeiMiniBlueFor2DaysOnTheSecondDayId { get; set; }


        protected LocationOrakei Orakei { get; set; }
        protected LocationRemuera Remuera { get; set; }

        protected ServiceMiniRed MiniRed { get; set; }
        protected ServiceMiniBlue MiniBlue { get; set; }
        protected ServiceMiniGreen MiniGreen { get; set; }
        protected ServiceHolidayCamp HolidayCamp { get; set; }

        protected StandaloneAaronOrakeiMiniRed14To15 AaronOrakeiMiniRed14To15 { get; set; }
        protected StandaloneAaronOrakeiMiniRed16To17 AaronOrakeiMiniRed16To17 { get; set; }
        protected StandaloneBobbyRemuera12To13 BobbyRemuera12To13 { get; set; }

        protected CustomerFred Fred { get; set; }
        protected CustomerBarney Barney { get; set; }
        protected CustomerWilma Wilma { get; set; }
        protected CustomerBamBam BamBam { get; set; }

        protected BookingFredOnStandaloneAaronOrakeiMiniRed14To15 FredOnStandaloneAaronOrakeiMiniRed14To15 { get; set; }


        protected override string RelativePath
        {
            get { return "Sessions"; }
        }

        protected string AaronOrakei14To15SessionDescription
        {
            get
            {
                return string.Format("Mini Red at Orakei Tennis Club with Aaron Smith on {0} at 14:00",
                                     GetFormattedDateOneWeekOut());
            }
        }

        protected string AaronOrakei16To17SessionDescription
        {
            get
            {
                return string.Format("Mini Red at Orakei Tennis Club with Aaron Smith on {0} at 16:00",
                                     GetFormattedDateOneWeekOut());
            }
        }


        protected void FullySetupNewTestBusiness()
        {
            RegisterTestBusiness();
            FullySetupExistingTestBusiness();
        }

        protected void FullySetupExistingTestBusiness()
        {
            RegisterTestLocations();
            RegisterTestCoaches();
            RegisterTestServices();
            RegisterTestSessions();
            RegisterTestCourses();
            RegisterTestCustomers();
            BookCustomersOntoStandaloneSessions();
            //BookCustomersOntoCourses();
            //BookCustomersOntoCourseSessions();
        }


        protected void RegisterTestLocations()
        {
            RegisterLocationOrakei();
            RegisterLocationRemuera();
        }

        protected void RegisterTestLocations(SetupData setup)
        {
            RegisterLocationOrakei(setup);
            RegisterLocationRemuera(setup);
        }

        protected void RegisterLocationOrakei()
        {
            if (Orakei != null)
                return;
            Orakei = new LocationOrakei();
            LocationRegistrar.RegisterLocation(Orakei, Business);
        }

        protected void RegisterLocationOrakei(SetupData setup)
        {
            if (setup.Orakei != null)
                return;
            var orakei = new LocationOrakei();
            LocationRegistrar.RegisterLocation(orakei, setup.Business);
            setup.Orakei = orakei;
        }

        protected void RegisterLocationRemuera()
        {
            if (Remuera != null)
                return;
            Remuera = new LocationRemuera();
            LocationRegistrar.RegisterLocation(Remuera, Business);
        }

        protected void RegisterLocationRemuera(SetupData setup)
        {
            if (setup.Remuera != null)
                return;
            var remuera = new LocationRemuera();
            LocationRegistrar.RegisterLocation(remuera, setup.Business);
            setup.Remuera = remuera;
        }

        protected void RegisterTestCoaches()
        {
            RegisterCoachAaron();
            RegisterCoachBobby();
        }

        protected void RegisterCoachAaron()
        {
            if (Aaron != null)
                return;
            Aaron = new CoachAaron();
            CoachRegistrar.RegisterCoach(Aaron, Business);
        }

        protected void RegisterCoachAaron(SetupData setup)
        {
            if (setup.Aaron != null)
                return;
            var aaron = new CoachAaron();
            CoachRegistrar.RegisterCoach(aaron, setup.Business);
            setup.Aaron = aaron;
        }

        protected void RegisterCoachBobby()
        {
            if (Bobby != null)
                return;
            Bobby = new CoachBobby();
            CoachRegistrar.RegisterCoach(Bobby, Business);
        }

        protected void RegisterCoachBobby(SetupData setup)
        {
            if (setup.Bobby != null)
                return;
            var bobby = new CoachBobby();
            CoachRegistrar.RegisterCoach(bobby, setup.Business);
            setup.Bobby = bobby;
        }

        protected void RegisterTestCoaches(SetupData setup)
        {
            RegisterCoachAaron(setup);
            RegisterCoachBobby(setup);
        }

        protected void RegisterTestServices()
        {
            RegisterServiceMiniRed();
            RegisterServiceMiniBlue();
            RegisterServiceHolidayCamp();
        }

        protected void RegisterTestServices(SetupData setup)
        {
            RegisterServiceMiniRed(setup);
            RegisterServiceHolidayCamp(setup);
        }

        protected void RegisterServiceMiniRed()
        {
            if (MiniRed != null)
                return;
            MiniRed = new ServiceMiniRed();
            ServiceRegistrar.RegisterService(MiniRed, Business);
        }

        protected void RegisterServiceMiniRed(SetupData setup)
        {
            if (setup.MiniRed != null)
                return;
            var miniRed = new ServiceMiniRed();
            ServiceRegistrar.RegisterService(miniRed, setup.Business);
            setup.MiniRed = miniRed;
        }

        protected void RegisterServiceMiniBlue()
        {
            if (MiniBlue != null)
                return;
            MiniBlue = new ServiceMiniBlue();
            ServiceRegistrar.RegisterService(MiniBlue, Business);
        }

        protected void RegisterServiceHolidayCamp()
        {
            if (HolidayCamp != null)
                return;
            HolidayCamp = new ServiceHolidayCamp();
            ServiceRegistrar.RegisterService(HolidayCamp, Business);
        }

        protected void RegisterServiceHolidayCamp(SetupData setup)
        {
            if (setup.HolidayCamp != null)
                return;
            var holidayCamp = new ServiceHolidayCamp();
            ServiceRegistrar.RegisterService(holidayCamp, setup.Business);
            setup.HolidayCamp = holidayCamp;
        }

        protected void RegisterTestSessions()
        {
            RegisterStandaloneAaronOrakei14To15();
            RegisterStandaloneAaronOrakei16To17();
        }

        protected void RegisterTestSessions(SetupData setup)
        {
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterStandaloneAaronOrakeiMiniRed16To17(setup);
        }

        protected void RegisterStandaloneAaronOrakei14To15()
        {
            if (AaronOrakeiMiniRed14To15 != null)
                return;

            RegisterCoachAaron();
            RegisterLocationOrakei();
            RegisterServiceMiniRed();

            AaronOrakeiMiniRed14To15 = new StandaloneAaronOrakeiMiniRed14To15(Aaron.Id, Orakei.Id, MiniRed.Id, GetDateFormatNumberOfDaysOut(21));
            RegisterTestSession(AaronOrakeiMiniRed14To15);            
        }

        protected void RegisterStandaloneAaronOrakeiMiniRed14To15(SetupData setup)
        {
            if (setup.AaronOrakeiMiniRed14To15 != null)
                return;
            
            RegisterCoachAaron(setup);
            RegisterLocationOrakei(setup);
            RegisterServiceMiniRed(setup);

            var aaronOrakeiMiniRed14To15 = new StandaloneAaronOrakeiMiniRed14To15(setup.Aaron.Id, 
                                                                                  setup.Orakei.Id,
                                                                                  setup.MiniRed.Id, 
                                                                                  GetDateFormatNumberOfDaysOut(21));
            RegisterTestSession(aaronOrakeiMiniRed14To15, setup);
            setup.AaronOrakeiMiniRed14To15 = aaronOrakeiMiniRed14To15;
        }

        protected void RegisterStandaloneAaronOrakeiMiniRed16To17(SetupData setup)
        {
            if (setup.AaronOrakeiMiniRed16To17 != null)
                return;

            RegisterCoachAaron(setup);
            RegisterLocationOrakei(setup);
            RegisterServiceMiniRed(setup);

            var aaronOrakeiMiniRed16To17 = new StandaloneAaronOrakeiMiniRed16To17(setup.Aaron.Id,
                                                                                  setup.Orakei.Id,
                                                                                  setup.MiniRed.Id,
                                                                                  GetDateFormatNumberOfDaysOut(7));
            RegisterTestSession(aaronOrakeiMiniRed16To17, setup);
            setup.AaronOrakeiMiniRed16To17 = aaronOrakeiMiniRed16To17;
        }

        protected void RegisterTestCourses(SetupData setup)
        {
            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCourseBobbyRemueraMiniRed9To10For3Weeks(setup);
        }

        protected void RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(SetupData setup)
        {
            if (setup.AaronOrakeiHolidayCamp9To15For3Days != null)
                return;

            RegisterCoachAaron(setup);
            RegisterLocationOrakei(setup);
            RegisterServiceHolidayCamp(setup);

            var aaronOrakeiHolidayCamp9To15For3Days = new CourseAaronOrakeiHolidayCamp9To15For3Days(setup.Aaron.Id,
                                                                                                    setup.Orakei.Id,
                                                                                                    setup.HolidayCamp.Id,
                                                                                                    GetDateFormatNumberOfDaysOut(14));
            RegisterTestCourse(aaronOrakeiHolidayCamp9To15For3Days, setup);
            setup.AaronOrakeiHolidayCamp9To15For3Days = aaronOrakeiHolidayCamp9To15For3Days;
        }

        protected void RegisterCourseBobbyRemueraMiniRed9To10For3Weeks(SetupData setup)
        {
            if (setup.BobbyRemueraMiniRed9To10For3Weeks != null)
                return;

            RegisterCoachBobby(setup);
            RegisterLocationRemuera(setup);
            RegisterServiceMiniRed(setup);

            var bobbyRemueraMiniRed9To10For3Weeks = new CourseBobbyRemueraMiniRed9To10For3Weeks(setup.Bobby.Id,
                                                                                                setup.Remuera.Id,
                                                                                                setup.MiniRed.Id,
                                                                                                GetDateFormatNumberOfDaysOut(10));
            RegisterTestCourse(bobbyRemueraMiniRed9To10For3Weeks, setup);
            setup.BobbyRemueraMiniRed9To10For3Weeks = bobbyRemueraMiniRed9To10For3Weeks;
        }


        protected void RegisterFullyBookedStandaloneAaronOrakeiMiniRed14To15(SetupData setup)
        {
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterWilmaOnStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterBarneyOnStandaloneAaronOrakeiMiniRed14To15(setup);
        }

        protected void RegisterFullyBookedLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(SetupData setup)
        {
            RegisterFredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterWilmaOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterBarneyOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(setup);
        }

        protected void RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(SetupData setup)
        {
            if (setup.FredOnAaronOrakeiMiniRed14To15 != null)
                return;

            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerFred(setup);

            var fredOnAaronOrakeiMiniRed14To15 = new BookingFredOnStandaloneAaronOrakeiMiniRed14To15(setup.AaronOrakeiMiniRed14To15.Id,
                                                                                                     setup.Fred.Id);
            RegisterTestBooking(fredOnAaronOrakeiMiniRed14To15, setup);
            setup.FredOnAaronOrakeiMiniRed14To15 = fredOnAaronOrakeiMiniRed14To15;
        }

        protected void RegisterWilmaOnStandaloneAaronOrakeiMiniRed14To15(SetupData setup)
        {
            if (setup.WilmaOnAaronOrakeiMiniRed14To15 != null)
                return;

            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerWilma(setup);

            var wilmaOnAaronOrakeiMiniRed14To15 = new BookingWilmaOnStandaloneAaronOrakeiMiniRed14To15(setup.AaronOrakeiMiniRed14To15.Id,
                                                                                                       setup.Wilma.Id);
            RegisterTestBooking(wilmaOnAaronOrakeiMiniRed14To15, setup);
            setup.WilmaOnAaronOrakeiMiniRed14To15 = wilmaOnAaronOrakeiMiniRed14To15;
        }

        protected void RegisterBarneyOnStandaloneAaronOrakeiMiniRed14To15(SetupData setup)
        {
            if (setup.BarneyOnAaronOrakeiMiniRed14To15 != null)
                return;

            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterCustomerBarney(setup);

            var barneyOnAaronOrakeiMiniRed14To15 = new BookingBarneyOnStandaloneAaronOrakeiMiniRed14To15(setup.AaronOrakeiMiniRed14To15.Id,
                                                                                                         setup.Barney.Id);
            RegisterTestBooking(barneyOnAaronOrakeiMiniRed14To15, setup);
            setup.BarneyOnAaronOrakeiMiniRed14To15 = barneyOnAaronOrakeiMiniRed14To15;
        }


        protected void RegisterFredOnAllCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(SetupData setup)
        {
            if (setup.FredOnAaronOrakeiHolidayCamp9To15For3Days != null)
                return;

            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var courseSessionIds = new List<Guid>
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id
            };

            var fredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = new ExpectedCourseBooking(courseSessionIds,
                                                                                                         setup.Fred.Id);
            RegisterTestBooking(fredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days, setup);
            setup.FredOnAaronOrakeiHolidayCamp9To15For3Days = fredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days;
        }

        protected void RegisterFredOnTwoCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(SetupData setup)
        {
            if (setup.FredOnAaronOrakeiHolidayCamp9To15For3Days != null)
                return;

            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var courseSessionIds = new List<Guid>
            {
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id
            };

            var fredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = new ExpectedCourseBooking(courseSessionIds,
                                                                                                         setup.Fred.Id);
            RegisterTestBooking(fredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days, setup);
            setup.FredOnAaronOrakeiHolidayCamp9To15For3Days = fredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days;
        }

        protected void RegisterFredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(SetupData setup)
        {
            if (setup.FredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days != null)
                return;

            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var fredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = new ExpectedBooking(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id,
                                                                                                   setup.Fred.Id);
            RegisterTestBooking(fredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days, setup);
            setup.FredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = fredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days;
        }

        protected void RegisterWilmaOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(SetupData setup)
        {
            if (setup.WilmaOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days != null)
                return;

            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerWilma(setup);

            var wilmaOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = new ExpectedBooking(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id,
                                                                                                    setup.Wilma.Id);
            RegisterTestBooking(wilmaOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days, setup);
            setup.WilmaOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = wilmaOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days;
        }

        protected void RegisterBarneyOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(SetupData setup)
        {
            if (setup.BarneyOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days != null)
                return;

            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerBarney(setup);

            var barneyOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = new ExpectedBooking(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id,
                                                                                                     setup.Barney.Id);
            RegisterTestBooking(barneyOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days, setup);
            setup.BarneyOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = barneyOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days;
        }


        protected void RegisterWilmaOnStandaloneAaronOrakeiMiniRed16To17(SetupData setup)
        {
            if (setup.WilmaOnAaronOrakeiMiniRed16To17 != null)
                return;

            RegisterStandaloneAaronOrakeiMiniRed16To17(setup);
            RegisterCustomerWilma(setup);

            var wilmaOnAaronOrakeiMiniRed16To17 = new BookingWilmaOnStandaloneAaronOrakeiMiniRed16To17(setup.AaronOrakeiMiniRed16To17.Id,
                                                                                                       setup.Wilma.Id);
            RegisterTestBooking(wilmaOnAaronOrakeiMiniRed16To17, setup);
            setup.WilmaOnAaronOrakeiMiniRed16To17 = wilmaOnAaronOrakeiMiniRed16To17;
        }

        protected void RegisterTestBooking(ExpectedBooking booking, SetupData setup)
        {
            var json = CreateBookingSaveCommandJson(booking);
            var response = PostBooking(json, setup);
            if (response.Payload != null)
                booking.Id = ((BookingData)response.Payload).id;
        }

        protected void RegisterTestBooking(ExpectedCourseBooking booking, SetupData setup)
        {
            var json = CreateBookingSaveCommandJson(booking);
            var response = PostBooking(json, setup);
            if (response.Payload != null)
                booking.Id = ((BookingData)response.Payload).id;
        }

        protected void RegisterStandaloneAaronOrakei16To17()
        {
            if (AaronOrakeiMiniRed16To17 != null)
                return;

            RegisterCoachAaron();
            RegisterLocationOrakei();
            RegisterServiceMiniRed();

            AaronOrakeiMiniRed16To17 = new StandaloneAaronOrakeiMiniRed16To17(Aaron.Id, Orakei.Id, MiniRed.Id, GetDateFormatNumberOfDaysOut(7));
            RegisterTestSession(AaronOrakeiMiniRed16To17);
        }

        protected void RegisterTestSession(ExpectedStandaloneSession session, SetupData setup)
        {
            var json = CreateSessionSaveCommandJson(session);
            var response = PostSession(json, setup);
            if (response.Payload != null)
                session.Id = ((SessionData)response.Payload).id;
        }

        protected void RegisterTestCourse(ExpectedCourse course, SetupData setup)
        {
            var json = CreateCourseSaveCommandJson(course);
            var response = PostCourse(json, setup);
            if (response.Payload != null)
            {
                var courseResponse = (CourseData)response.Payload;
                course.Id = courseResponse.id;

                var i = 0;
                foreach (var session in course.Sessions)
                {
                    session.ParentId = course.Id;
                    session.Id = courseResponse.sessions[i].id;

                    i++;
                }
            }
        }

        protected void RegisterTestSession(ExpectedStandaloneSession session)
        {
            var json = CreateSessionSaveCommandJson(session);
            var response = PostSession(json);
            if (response.Payload != null)
                session.Id = ((SessionData)response.Payload).id;
        }

        private void RegisterTestCourses()
        {
            RegisterAaronRemuera9To10For4Weeks();
            RegisterBobbyRemueraHolidayCampFor3Days();
            RegisterAaronOrakeiMiniBlueFor2Days();
        }

        private void RegisterAaronRemuera9To10For4Weeks()
        {
            var json = CreateSessionSaveCommandAaronRemuera9To10For4WeeksJson();
            var response = PostCourse(json);
            var course = (CourseData)response.Payload;
            if (course != null)
            {
                AaronRemuera9To10For4WeeksCourseId = course.id;
                AaronRemuera9To10For4WeeksSessionIds = new Guid[5];
                for (var i = 0; i < 4; i++)
                    AaronRemuera9To10For4WeeksSessionIds[i] = course.sessions[i].id;
            }
        }

        private void RegisterBobbyRemueraHolidayCampFor3Days()
        {
            var json = CreateSessionSaveCommandBobbyRemueraHolidayCampFor3DaysJson();
            var response = PostCourse(json);
            var course = (CourseData)response.Payload;
            if (course != null)
            {
                BobbyRemueraHolidayCampFor3DaysCourseId = course.id;
                BobbyRemueraHolidayCampFor3DaysSessionIds = new Guid[3];
                for (var i = 0; i < 3; i++)
                    BobbyRemueraHolidayCampFor3DaysSessionIds[i] = course.sessions[i].id;
            }
        }

        private void RegisterAaronOrakeiMiniBlueFor2Days()
        {
            var json = CreateSessionSaveCommandAaronOrakeiMiniBlueFor2DaysJson();
            var response = PostCourse(json);
            var course = (CourseData)response.Payload;
            if (course != null)
            {
                AaronOrakeiMiniBlueFor2DaysCourseId = course.id;
                AaronOrakeiMiniBlueFor2DaysSessionIds = new Guid[2];
                for (var i = 0; i < 2; i++)
                    AaronOrakeiMiniBlueFor2DaysSessionIds[i] = course.sessions[i].id;
            }
        }


        protected ApiResponse WhenPostSession(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<SessionData>(json,
                                                                      setup.Business.UserName,
                                                                      setup.Business.Password,
                                                                      RelativePath);
        }

        protected ApiResponse WhenPostCourse(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<CourseData>(json,
                                                                     setup.Business.UserName,
                                                                     setup.Business.Password,
                                                                     RelativePath);
        }


        protected ApiResponse WhenTryCreateSession(ApiSessionSaveCommand command, SetupData setup)
        {
            return WhenPostSession(JsonConvert.SerializeObject(command), setup);
        }

        protected ApiResponse WhenTryUpdateSession(ApiSessionSaveCommand command, SetupData setup)
        {
            return WhenPostSession(JsonConvert.SerializeObject(command), setup);
        }

        protected ApiResponse WhenTryCreateCourse(ApiSessionSaveCommand command, SetupData setup)
        {
            return WhenPostCourse(JsonConvert.SerializeObject(command), setup);
        }

        protected ApiResponse WhenTryUpdateCourse(ApiSessionSaveCommand command, SetupData setup)
        {
            return WhenPostCourse(JsonConvert.SerializeObject(command), setup);
        }


        private ApiResponse PostSession(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<SessionData>(json,
                                                                      setup.Business.UserName,
                                                                      setup.Business.Password, 
                                                                      "Sessions");
        }

        private Response PostSession(string json)
        {
            return Post<SessionData>(json, "Sessions");
        }

        private ApiResponse PostCourse(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<CourseData>(json,
                                                                     setup.Business.UserName,
                                                                     setup.Business.Password,
                                                                     "Sessions");
        }
    
        private Response PostCourse(string json)
        {
            return Post<CourseData>(json, "Sessions");
        }

        protected ApiSessionSaveCommand CreateSessionSaveCommandAaronOrakei14To15()
        {
            return new ApiSessionSaveCommand
            {
                location = new ApiLocationKey { id = Orakei.Id },
                coach = new ApiCoachKey { id = Aaron.Id },
                service = new ApiServiceKey { id = MiniRed.Id },
                timing = new ApiSessionTiming { startDate = GetDateFormatNumberOfWeeksOut(3), startTime = "14:00", duration = 60 },
                booking = new ApiSessionBooking { studentCapacity = 13, isOnlineBookable = true },
                repetition = new ApiRepetition { sessionCount = 1 },
                pricing = new ApiPricing { sessionPrice = 19.95m },
                presentation = new ApiPresentation { colour = "red" }
            };
        }

        //protected ApiSessionSaveCommand CreateSessionSaveCommand(ExpectedStandaloneSession session)
        //{
        //    return new ApiSessionSaveCommand
        //    {
        //        coach = session.Coach,
        //        location = session.Location,
        //        service = session.Service,
        //        timing = session.Timing,
        //        booking = session.Booking,
        //        repetition = session.Repetition,
        //        pricing = session.Pricing,
        //        presentation = session.Presentation
        //    };
        //}

        protected ApiSessionSaveCommand CreateCourseSaveCommand(ExpectedCourse course)
        {
            return new ApiSessionSaveCommand
            {
                coach = course.Coach,
                location = course.Location,
                service = course.Service,
                timing = course.Timing,
                booking = course.Booking,
                repetition = course.Repetition,
                pricing = course.Pricing,
                presentation = course.Presentation
            };
        }

        private string CreateSessionSaveCommandJson(ExpectedStandaloneSession session)
        {
            return JsonConvert.SerializeObject(CreateSessionSaveCommand(session));
        }

        private string CreateCourseSaveCommandJson(ExpectedCourse course)
        {
            return JsonConvert.SerializeObject(CreateCourseSaveCommand(course));
        }

        protected ApiBookingSaveCommand CreateBookingSaveCommand(ExpectedBooking booking)
        {
            return new ApiBookingSaveCommand(booking.Session.id.Value, booking.Customer.id.Value);
        }

        protected ApiBookingSaveCommand CreateBookingSaveCommand(ExpectedCourseBooking booking)
        {
            return new ApiBookingSaveCommand(booking.SessionIds, booking.Customer.id.Value);
        }

        private string CreateBookingSaveCommandJson(ExpectedBooking booking)
        {
            return JsonConvert.SerializeObject(CreateBookingSaveCommand(booking));
        }

        private string CreateBookingSaveCommandJson(ExpectedCourseBooking booking)
        {
            return JsonConvert.SerializeObject(CreateBookingSaveCommand(booking));
        }

        protected ApiSessionSaveCommand CreateSessionSaveCommand(ExpectedStandaloneSession session)
        {
            return new ApiSessionSaveCommand(session);
        }

        protected ApiSessionSaveCommand CreateSessionSaveCommandAaronRemuera9To10()
        {
            return new ApiSessionSaveCommand
            {
                location = new ApiLocationKey { id = Remuera.Id },
                coach = new ApiCoachKey { id = Aaron.Id },
                service = new ApiServiceKey { id = MiniRed.Id },
                timing = new ApiSessionTiming { startDate = GetFormattedDateOneWeekOut(), startTime = "9:00", duration = 60 },
                booking = new ApiSessionBooking { studentCapacity = 6, isOnlineBookable = true },
                repetition = new ApiRepetition { sessionCount = 1 },
                pricing = new ApiPricing { sessionPrice = 19.95m },
                presentation = new ApiPresentation { colour = "red" }
            };
        }

        protected ApiSessionSaveCommand CreateSessionSaveCommandAaronRemuera9To10For4Weeks()
        {
            var command = CreateSessionSaveCommandAaronRemuera9To10();
            command.repetition = new ApiRepetition {sessionCount = 4, repeatFrequency = "w"};

            return command;
        }

        protected ApiSessionSaveCommand CreateSessionSaveCommandBobbyRemueraHolidayCampFor3Days()
        {
            return new ApiSessionSaveCommand
            {
                location = new ApiLocationKey { id = Remuera.Id },
                coach = new ApiCoachKey { id = Bobby.Id },
                service = new ApiServiceKey { id = HolidayCamp.Id },
                timing = new ApiSessionTiming { startDate = GetDateFormatNumberOfDaysOut(2), startTime = "10:00", duration = 240 },
                booking = new ApiSessionBooking { studentCapacity = 20, isOnlineBookable = false },
                repetition = new ApiRepetition { sessionCount = 3, repeatFrequency = "d"},
                pricing = new ApiPricing { sessionPrice = 120, coursePrice = 200},
                presentation = new ApiPresentation { colour = "yellow" }
            };
        }

        protected ApiSessionSaveCommand CreateSessionSaveComandAaronOrakeiMiniBlueFor2Days()
        {
            return new ApiSessionSaveCommand
            {
                location = new ApiLocationKey { id = Orakei.Id },
                coach = new ApiCoachKey { id = Aaron.Id },
                service = new ApiServiceKey { id = MiniBlue.Id },
                timing = new ApiSessionTiming { startDate = GetDateFormatNumberOfDaysOut(1), startTime = "9:00", duration = 60 },
                booking = new ApiSessionBooking { studentCapacity = 2, isOnlineBookable = true },
                repetition = new ApiRepetition { sessionCount = 2, repeatFrequency = "d" },
                pricing = new ApiPricing { sessionPrice = 25, coursePrice = 40 },
                presentation = new ApiPresentation { colour = "blue" }
            };
        }

        private string CreateSessionSaveCommandAaronRemuera9To10For4WeeksJson()
        {
            return JsonConvert.SerializeObject(CreateSessionSaveCommandAaronRemuera9To10For4Weeks());
        }

        private string CreateSessionSaveCommandBobbyRemueraHolidayCampFor3DaysJson()
        {
            return JsonConvert.SerializeObject(CreateSessionSaveCommandBobbyRemueraHolidayCampFor3Days());
        }

        private string CreateSessionSaveCommandAaronOrakeiMiniBlueFor2DaysJson()
        {
            return JsonConvert.SerializeObject(CreateSessionSaveComandAaronOrakeiMiniBlueFor2Days());
        }

        private void RegisterTestCustomers()
        {
            RegisterCustomerFred();
            RegisterCustomerBarney();
            RegisterCustomerWilma();
        }

        public void RegisterTestCustomers(SetupData setup)
        {
            RegisterCustomerFred(setup);
            RegisterCustomerWilma(setup);
            RegisterCustomerBarney(setup);
            RegisterCustomerBambam(setup);
        }

        protected void RegisterCustomerFred()
        {
            Fred = new CustomerFred();
            CustomerRegistrar.RegisterCustomer(Fred, Business);
        }

        protected void RegisterCustomerFred(SetupData setup)
        {
            if (setup.Fred != null)
                return;
            var fred = new CustomerFred();
            CustomerRegistrar.RegisterCustomer(fred, setup.Business);
            setup.Fred = fred;
        }

        protected void RegisterCustomerBarney()
        {
            Barney = new CustomerBarney();
            CustomerRegistrar.RegisterCustomer(Barney, Business);
        }

        protected void RegisterCustomerWilma()
        {
            Wilma = new CustomerWilma();
            CustomerRegistrar.RegisterCustomer(Wilma, Business);
        }

        protected void RegisterCustomerWilma(SetupData setup)
        {
            if (setup.Wilma != null)
                return;
            var wilma = new CustomerWilma();
            CustomerRegistrar.RegisterCustomer(wilma, setup.Business);
            setup.Wilma = wilma;
        }

        protected void RegisterCustomerBarney(SetupData setup)
        {
            if (setup.Barney != null)
                return;
            var barney = new CustomerBarney();
            CustomerRegistrar.RegisterCustomer(barney, setup.Business);
            setup.Barney = barney;
        }

        protected void RegisterCustomerBambam(SetupData setup)
        {
            if (setup.BamBam != null)
                return;
            var bambam = new CustomerBamBam();
            CustomerRegistrar.RegisterCustomer(bambam, setup.Business);
            setup.BamBam = bambam;
        }

        private void BookCustomersOntoStandaloneSessions()
        {
            BookFredFlintstoneOntoAaronOrakei14To15();
            BookBarneyRubbleOntoAaronOrakei14To15();
        }

        private void BookFredFlintstoneOntoAaronOrakei14To15()
        {
            var json = CreateNewBookingSaveCommand(AaronOrakeiMiniRed14To15.Id, Fred.Id);
            var response = PostBooking(json);
            if (response.Payload != null)
                FredOnAaronOrakei14To15SessionId = ((BookingData)response.Payload).id;
        }

        private void BookBarneyRubbleOntoAaronOrakei14To15()
        {
            var json = CreateNewBookingSaveCommand(AaronOrakeiMiniRed14To15.Id, Barney.Id);
            var response = PostBooking(json);
            if (response.Payload != null)
                BarneyOnAaronOrakei14To15SessionId = ((BookingData)response.Payload).id;
        }

        private void BookCustomersOntoCourses()
        {
            BookFredFlintstoneOntoAaronOrakeiMiniBlueFor2Days();
        }

        private void BookFredFlintstoneOntoAaronOrakeiMiniBlueFor2Days()
        {
            var json = CreateNewBookingSaveCommand(AaronOrakeiMiniBlueFor2DaysCourseId, Fred.Id);
            var response = PostBooking(json);
            if (response.Payload != null)
                FredOnAaronOrakeiMiniBlueFor2DaysCourseId = ((BookingData)response.Payload).id;
        }

        private void BookCustomersOntoCourseSessions()
        {
            BookBarneyRubbleOntoAaronOrakeiMiniBlueFor2DaysOnTheSecondDay();
        }

        private void BookBarneyRubbleOntoAaronOrakeiMiniBlueFor2DaysOnTheSecondDay()
        {
            var json = CreateNewBookingSaveCommand(AaronOrakeiMiniBlueFor2DaysSessionIds[1], Barney.Id);
            var response = PostBooking(json);
            if (response.Payload != null)
                BarneyOnAaronOrakeiMiniBlueFor2DaysOnTheSecondDayId = ((BookingData)response.Payload).id;
        }

        private string CreateNewBookingSaveCommand(Guid sessionId, Guid customerId)
        {
            var booking = new ApiBookingSaveCommand
            {
                sessions = new List<ApiSessionKey>
                {
                    new ApiSessionKey { id = sessionId }
                },
                customer = new ApiCustomerKey { id = customerId }
            };

            return JsonConvert.SerializeObject(booking);
        }

        private Response PostBooking(string json)
        {
            return Post<BookingData>(json, "Bookings");
        }

        private ApiResponse PostBooking(string json, SetupData setup)
        {
            return new TestAuthenticatedApiClient().Post<BookingData>(json,
                                                                      setup.Business.UserName,
                                                                      setup.Business.Password,
                                                                      "Bookings");
        }


        protected void AssertSessionLocation(LocationKeyData location, Guid locationId, string locationName)
        {
            Assert.That(location, Is.Not.Null);
            Assert.That(location.id, Is.EqualTo(locationId));
            Assert.That(location.name, Is.EqualTo(locationName));
        }

        protected void AssertSessionCoach(CoachKeyData coach, Guid coachId, string coachName)
        {
            Assert.That(coach, Is.Not.Null);
            Assert.That(coach.id, Is.EqualTo(coachId));
            Assert.That(coach.name, Is.EqualTo(coachName));
        }

        protected void AssertSessionService(ServiceKeyData service, Guid serviceId, string serviceName)
        {
            Assert.That(service, Is.Not.Null);
            Assert.That(service.id, Is.EqualTo(serviceId));
            Assert.That(service.name, Is.EqualTo(serviceName));
        }

        protected void AssertSessionTiming(SessionTimingData timing, string startDate, string startTime, int duration)
        {
            Assert.That(timing, Is.Not.Null);
            Assert.That(timing.startDate, Is.EqualTo(startDate));
            Assert.That(timing.startTime, Is.EqualTo(startTime));
            Assert.That(timing.duration, Is.EqualTo(duration));
        }

        protected void AssertSessionBooking(SessionBookingData booking, int? studentCapacity, bool isOnlineBookable, int bookingCount = 0)
        {
            Assert.That(booking, Is.Not.Null);
            Assert.That(booking.studentCapacity, Is.EqualTo(studentCapacity));
            Assert.That(booking.isOnlineBookable, Is.EqualTo(isOnlineBookable));
            Assert.That(booking.bookings, Is.Not.Null);
            Assert.That(booking.bookings.Count, Is.EqualTo(bookingCount));
        }

        protected void AssertSessionPricing(PricingData pricing, decimal? sessionPrice, decimal? coursePrice)
        {
            Assert.That(pricing, Is.Not.Null);
            Assert.That(pricing.sessionPrice, Is.EqualTo(sessionPrice));
            Assert.That(pricing.coursePrice, Is.EqualTo(coursePrice));
        }

        protected void AssertSessionRepetition(RepetitionData repetition, int sessionCount, string repeatFrequency)
        {
            Assert.That(repetition, Is.Not.Null);
            Assert.That(repetition.sessionCount, Is.EqualTo(sessionCount));
            Assert.That(repetition.repeatFrequency, Is.EqualTo(repeatFrequency));
        }

        protected void AssertSessionPresentation(PresentationData presentation, string colour)
        {
            Assert.That(presentation, Is.Not.Null);
            Assert.That(presentation.colour, Is.EqualTo(colour));
        }


        protected string GetFormattedDateToday()
        {
            return GetDateFormatNumberOfWeeksOut(0);
        }

        protected string GetFormattedDateOneWeekOut()
        {
            return GetDateFormatNumberOfWeeksOut(1);
        }

        protected string GetFormattedDateTwoWeeksOut()
        {
            return GetDateFormatNumberOfWeeksOut(2);
        }

        protected string GetFormattedDateThreeWeeksOut()
        {
            return GetDateFormatNumberOfWeeksOut(3);
        }

        protected string GetDateFormatNumberOfWeeksOut(int numberOfWeeks)
        {
            var today = DateTime.Today;
            var weeksFromToday = today.AddDays(7 * numberOfWeeks);

            return weeksFromToday.ToString("yyyy-MM-dd");
        }

        protected string GetDateFormatNumberOfDaysOut(int numberOfDays)
        {
            var today = DateTime.Today;
            var daysFromToday = today.AddDays(numberOfDays);

            return daysFromToday.ToString("yyyy-MM-dd");
        }
    }
}
