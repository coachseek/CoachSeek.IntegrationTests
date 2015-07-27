using System;
using System.Collections.Generic;
using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Booking;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Coach;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Course;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Location;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Service;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Session;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests
{
    public abstract class ScheduleTests : WebIntegrationTest
    {
        protected override string RelativePath
        {
            get { return "Sessions"; }
        }


        protected void RegisterTestLocations(SetupData setup)
        {
            RegisterLocationOrakei(setup);
            RegisterLocationRemuera(setup);
        }

        protected void RegisterLocationOrakei(SetupData setup)
        {
            if (setup.Orakei != null)
                return;
            var orakei = new LocationOrakei();
            LocationRegistrar.RegisterLocation(orakei, setup.Business);
            setup.Orakei = orakei;
        }

        protected void RegisterLocationRemuera(SetupData setup)
        {
            if (setup.Remuera != null)
                return;
            var remuera = new LocationRemuera();
            LocationRegistrar.RegisterLocation(remuera, setup.Business);
            setup.Remuera = remuera;
        }

        protected void RegisterTestCoaches(SetupData setup)
        {
            RegisterCoachAaron(setup);
            RegisterCoachBobby(setup);
        }

        protected void RegisterCoachAaron(SetupData setup)
        {
            if (setup.Aaron != null)
                return;
            var aaron = new CoachAaron();
            CoachRegistrar.RegisterCoach(aaron, setup.Business);
            setup.Aaron = aaron;
        }

        protected void RegisterCoachBobby(SetupData setup)
        {
            if (setup.Bobby != null)
                return;
            var bobby = new CoachBobby();
            CoachRegistrar.RegisterCoach(bobby, setup.Business);
            setup.Bobby = bobby;
        }

        protected void RegisterTestServices(SetupData setup)
        {
            RegisterServiceMiniRed(setup);
            RegisterServiceHolidayCamp(setup);
        }

        protected void RegisterServiceMiniRed(SetupData setup)
        {
            if (setup.MiniRed != null)
                return;
            var miniRed = new ServiceMiniRed();
            ServiceRegistrar.RegisterService(miniRed, setup.Business);
            setup.MiniRed = miniRed;
        }

        protected void RegisterServiceHolidayCamp(SetupData setup)
        {
            if (setup.HolidayCamp != null)
                return;
            var holidayCamp = new ServiceHolidayCamp();
            ServiceRegistrar.RegisterService(holidayCamp, setup.Business);
            setup.HolidayCamp = holidayCamp;
        }

        protected void RegisterTestSessions(SetupData setup)
        {
            RegisterStandaloneAaronOrakeiMiniRed14To15(setup);
            RegisterStandaloneAaronOrakeiMiniRed16To17(setup);
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

        protected void RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(SetupData setup, int studentCapacity = 3)
        {
            if (setup.AaronOrakeiHolidayCamp9To15For3Days != null)
                return;

            RegisterCoachAaron(setup);
            RegisterLocationOrakei(setup);
            RegisterServiceHolidayCamp(setup);

            var aaronOrakeiHolidayCamp9To15For3Days = new CourseAaronOrakeiHolidayCamp9To15For3Days(setup.Aaron.Id,
                                                                                                    setup.Orakei.Id,
                                                                                                    setup.HolidayCamp.Id,
                                                                                                    GetDateFormatNumberOfDaysOut(14),
                                                                                                    studentCapacity);
            RegisterTestCourse(aaronOrakeiHolidayCamp9To15For3Days, setup);
            setup.AaronOrakeiHolidayCamp9To15For3Days = aaronOrakeiHolidayCamp9To15For3Days;
        }

        protected void RegisterCourseBobbyRemueraMiniRed9To10For3Weeks(SetupData setup, bool isOnlineBookable = true)
        {
            if (setup.BobbyRemueraMiniRed9To10For3Weeks != null)
                return;

            RegisterCoachBobby(setup);
            RegisterLocationRemuera(setup);
            RegisterServiceMiniRed(setup);

            var bobbyRemueraMiniRed9To10For3Weeks = new CourseBobbyRemueraMiniRed9To10For3Weeks(setup.Bobby.Id,
                                                                                                setup.Remuera.Id,
                                                                                                setup.MiniRed.Id,
                                                                                                GetDateFormatNumberOfDaysOut(10),
                                                                                                isOnlineBookable);
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

        public void RegisterTestCustomers(SetupData setup)
        {
            RegisterCustomerFred(setup);
            RegisterCustomerWilma(setup);
            RegisterCustomerBarney(setup);
            RegisterCustomerBambam(setup);
        }

        protected void RegisterCustomerFred(SetupData setup)
        {
            if (setup.Fred != null)
                return;
            var fred = new CustomerFred();
            CustomerRegistrar.RegisterCustomer(fred, setup.Business);
            setup.Fred = fred;
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

            var fredOnAllCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = new ExpectedCourseBooking(courseSessionIds,
                                                                                                        setup.Fred.Id);
            RegisterTestBooking(fredOnAllCourseSessionInAaronOrakeiHolidayCamp9To15For3Days, setup);
            setup.FredOnAaronOrakeiHolidayCamp9To15For3Days = fredOnAllCourseSessionInAaronOrakeiHolidayCamp9To15For3Days;

            var sessionBookingOne = new ExpectedBooking(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id, setup.Fred.Id)
            {
                Id = fredOnAllCourseSessionInAaronOrakeiHolidayCamp9To15For3Days.SessionBookingIds[0]
            };
            var sessionBookingTwo = new ExpectedBooking(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id, setup.Fred.Id)
            {
                Id = fredOnAllCourseSessionInAaronOrakeiHolidayCamp9To15For3Days.SessionBookingIds[1]
            };
            var sessionBookingThree = new ExpectedBooking(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id, setup.Fred.Id)
            {
                Id = fredOnAllCourseSessionInAaronOrakeiHolidayCamp9To15For3Days.SessionBookingIds[2]
            };

            setup.FredOnFirstCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = sessionBookingOne;
            setup.FredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = sessionBookingTwo;
            setup.FredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = sessionBookingThree;
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

            var fredOnAaronOrakeiHolidayCamp9To15For3Days = new ExpectedCourseBooking(courseSessionIds, setup.Fred.Id);
            RegisterTestBooking(fredOnAaronOrakeiHolidayCamp9To15For3Days, setup);
            setup.FredOnAaronOrakeiHolidayCamp9To15For3Days = fredOnAaronOrakeiHolidayCamp9To15For3Days;
        }

        protected void RegisterFredOnFirstCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(SetupData setup, int studentCapacity = 3)
        {
            if (setup.FredOnFirstCourseSessionInAaronOrakeiHolidayCamp9To15For3Days != null)
                return;

            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup, studentCapacity);
            RegisterCustomerFred(setup);

            var fredOnFirstCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = new ExpectedBooking(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id,
                                                                                                    setup.Fred.Id);
            RegisterTestBooking(fredOnFirstCourseSessionInAaronOrakeiHolidayCamp9To15For3Days, setup);
            setup.FredOnFirstCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = fredOnFirstCourseSessionInAaronOrakeiHolidayCamp9To15For3Days;
        }

        protected void RegisterFredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days(SetupData setup)
        {
            if (setup.FredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days != null)
                return;

            RegisterCourseAaronOrakeiHolidayCamp9To15For3Days(setup);
            RegisterCustomerFred(setup);

            var fredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = new ExpectedBooking(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[1].Id,
                                                                                                     setup.Fred.Id);
            RegisterTestBooking(fredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days, setup);
            setup.FredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days = fredOnSecondCourseSessionInAaronOrakeiHolidayCamp9To15For3Days;
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
            var response = PostCourseBooking(json, setup);
            if (response.Payload != null)
            {
                var courseBooking = (CourseBookingData) response.Payload;
                booking.Id = courseBooking.id;
                foreach(var sessionBooking in courseBooking.sessionBookings)
                    booking.SessionBookingIds.Add(sessionBooking.id);
            }
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



        protected ApiResponse WhenTryCreateSession(ApiSessionSaveCommand command, SetupData setup)
        {
            return WhenPostSession(JsonSerialiser.Serialise(command), setup);
        }

        protected ApiResponse WhenTryUpdateSession(ApiSessionSaveCommand command, SetupData setup)
        {
            return WhenPostSession(JsonSerialiser.Serialise(command), setup);
        }

        protected ApiResponse WhenTryCreateCourse(ApiSessionSaveCommand command, SetupData setup)
        {
            return WhenPostCourse(JsonSerialiser.Serialise(command), setup);
        }

        protected ApiResponse WhenTryUpdateCourse(ApiSessionSaveCommand command, SetupData setup)
        {
            return WhenPostCourse(JsonSerialiser.Serialise(command), setup);
        }


        private string CreateSessionSaveCommandJson(ExpectedStandaloneSession session)
        {
            return JsonSerialiser.Serialise(CreateSessionSaveCommand(session));
        }

        private string CreateCourseSaveCommandJson(ExpectedCourse course)
        {
            return JsonSerialiser.Serialise(CreateCourseSaveCommand(course));
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
            return JsonSerialiser.Serialise(CreateBookingSaveCommand(booking));
        }

        private string CreateBookingSaveCommandJson(ExpectedCourseBooking booking)
        {
            return JsonSerialiser.Serialise(CreateBookingSaveCommand(booking));
        }

        protected ApiSessionSaveCommand CreateSessionSaveCommand(ExpectedSingleSession session)
        {
            return new ApiSessionSaveCommand(session);
        }

        protected ApiSessionSaveCommand CreateCourseSaveCommand(ExpectedCourse course)
        {
            return new ApiSessionSaveCommand(course);
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

        protected void AssertSessionClashError(ApiResponse response, Guid sessionId)
        {
            var error = AssertSingleError(response, "This session clashes with one or more sessions.");
            Assert.That(error.code, Is.EqualTo("clashing-session"));
            Assert.That(error.data, Is.StringContaining(string.Format("{{{0}}}", sessionId)));
        }

        protected string GetFormattedDateToday()
        {
            return GetDateFormatNumberOfDaysOut(0);
        }

        protected string GetDateFormatNumberOfDaysOut(int numberOfDays)
        {
            var today = DateTime.Today;
            var daysFromToday = today.AddDays(numberOfDays);

            return daysFromToday.ToString("yyyy-MM-dd");
        }


        protected string GivenNoBusinessDomain()
        {
            return null;
        }

        protected string GivenInvalidBusinessDomain()
        {
            return Random.RandomString;
        }

    }
}
