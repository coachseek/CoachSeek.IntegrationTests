using System;
using Coachseek.API.Client.Models;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Session;
using CoachSeek.Common;
using NUnit.Framework;

namespace CoachSeek.Api.Tests.Integration.Tests.Booking
{
    public class BookingGetTests : BaseBookingTests
    {
        [TestFixture]
        public class AnonymousBookingGetTests : BookingGetTests
        {
            [Test]
            public void GivenNoBusinessDomain_WhenTryGetStandaloneSessionBookingByIdAnonymously_ThenReturnNotAuthorised()
            {
                var setup = RegisterBusiness();
                RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

                var businessDomain = GivenNoBusinessDomain();
                var response = WhenTryGetStandaloneSessionBookingByIdAnonymously(setup.FredOnAaronOrakeiMiniRed14To15.Id, businessDomain);
                ThenReturnUnauthorised(response);
            }

            [Test]
            public void GivenInvalidBusinessDomain_WhenTryGetStandaloneSessionBookingByIdAnonymously_ThenReturnNotAuthorised()
            {
                var setup = RegisterBusiness();
                RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

                var businessDomain = GivenInvalidBusinessDomain();
                var response = WhenTryGetStandaloneSessionBookingByIdAnonymously(setup.FredOnAaronOrakeiMiniRed14To15.Id, businessDomain);
                ThenReturnUnauthorised(response);
            }

            [Test]
            public void GivenValidBusinessDomain_WhenTryGetStandaloneSessionBookingByIdAnonymously_ThenReturnStandaloneSessionBooking()
            {
                var setup = RegisterBusiness();
                RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

                var businessDomain = GivenValidBusinessDomain(setup);
                var response = WhenTryGetStandaloneSessionBookingByIdAnonymously(setup.FredOnAaronOrakeiMiniRed14To15.Id, businessDomain);
                ThenReturnStandaloneSessionBooking(response, setup);
            }

            [Test]
            public void  GivenValidBusinessDomain_WhenTryGetCourseBookingByIdAnonymously_ThenReturnCourseBooking()
            {
                var setup = RegisterBusiness();
                RegisterFredOnTwoCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(setup);

                var businessDomain = GivenValidBusinessDomain(setup);
                var response = WhenTryGetCourseBookingByIdAnonymously(setup.FredOnAaronOrakeiHolidayCamp9To15For3Days.Id, businessDomain);
                ThenReturnCourseBooking(response, setup);
            }


            private string GivenValidBusinessDomain(SetupData setup)
            {
                return setup.Business.Domain;
            }


            private ApiResponse WhenTryGetStandaloneSessionBookingByIdAnonymously(Guid bookingId, string businessDomain)
            {
                return BusinessAnonymousGet<SingleSessionBookingData>(RelativePath, bookingId, businessDomain);
            }

            private ApiResponse WhenTryGetCourseBookingByIdAnonymously(Guid bookingId, string businessDomain)
            {
                return BusinessAnonymousGet<CourseBookingData>(RelativePath, bookingId, businessDomain);
            }


            private void ThenReturnUnauthorised(ApiResponse response)
            {
                AssertUnauthorised(response);
            }
        }


        [TestFixture]
        public class AuthenticatedBookingGetTests : BookingGetTests
        {
            [Test]
            public void GivenInvalidBookingId_WhenTryGetBookingById_ThenReturnNotFound()
            {
                var setup = RegisterBusiness();

                var bookingId = GivenInvalidBookingId();
                var response = WhenTryGetStandaloneSessionBookingById(bookingId, setup);
                ThenReturnNotFound(response);
            }

            [Test]
            public void GivenValidStandaloneSessionBookingId_WhenTryGetStandaloneSessionBookingById_ThenReturnStandaloneSessionBooking()
            {
                var setup = RegisterBusiness();

                var bookingId = GivenValidStandaloneSessionBookingId(setup);
                var response = WhenTryGetStandaloneSessionBookingById(bookingId, setup);
                ThenReturnStandaloneSessionBooking(response, setup);
            }

            [Test]
            public void GivenValidCourseBookingId_WhenTryGetCourseBookingById_ThenReturnCourseBooking()
            {
                var setup = RegisterBusiness();

                var bookingId = GivenValidCourseBookingId(setup);
                var response = WhenTryGetCourseBookingById(bookingId, setup);
                ThenReturnCourseBooking(response, setup);
            }
        }


        private Guid GivenInvalidBookingId()
        {
            return Guid.NewGuid();
        }

        private Guid GivenValidStandaloneSessionBookingId(SetupData setup)
        {
            RegisterFredOnStandaloneAaronOrakeiMiniRed14To15(setup);

            return setup.FredOnAaronOrakeiMiniRed14To15.Id;
        }

        private Guid GivenValidCourseBookingId(SetupData setup)
        {
            RegisterFredOnTwoCourseSessionsInAaronOrakeiHolidayCamp9To15For3Days(setup);

            return setup.FredOnAaronOrakeiHolidayCamp9To15For3Days.Id;
        }


        private ApiResponse WhenTryGetStandaloneSessionBookingById(Guid bookingId, SetupData setup)
        {
            return AuthenticatedGet<SingleSessionBookingData>(RelativePath, bookingId, setup.Business.UserName, setup.Business.Password);
        }

        private ApiResponse WhenTryGetCourseBookingById(Guid bookingId, SetupData setup)
        {
            return AuthenticatedGet<CourseBookingData>(RelativePath, bookingId, setup.Business.UserName, setup.Business.Password);
        }


        private void ThenReturnStandaloneSessionBooking(ApiResponse response, SetupData setup)
        {
            var booking = AssertSuccessResponse<SingleSessionBookingData>(response);

            AssertGetSingleSessionBooking(booking, setup.AaronOrakeiMiniRed14To15, setup.Fred);
        }

        private void ThenReturnCourseBooking(ApiResponse response, SetupData setup)
        {
            var booking = AssertSuccessResponse<CourseBookingData>(response);

            Assert.That(booking.id, Is.InstanceOf<Guid>());

            Assert.That(booking.course.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Id));
            Assert.That(booking.course.name, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Description));

            Assert.That(booking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(booking.customer.name, Is.EqualTo(string.Format("{0} {1}", setup.Fred.FirstName, setup.Fred.LastName)));

            Assert.That(booking.price, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Pricing.sessionPrice * booking.sessionBookings.Count));
            Assert.That(booking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));

            Assert.That(booking.sessionBookings.Count, Is.EqualTo(2));

            var firstSessionBooking = booking.sessionBookings[0];
            Assert.That(firstSessionBooking.id, Is.InstanceOf<Guid>());
            Assert.That(firstSessionBooking.session.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id));
            Assert.That(firstSessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(firstSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            Assert.That(firstSessionBooking.hasAttended, Is.Null);

            var secondSessionBooking = booking.sessionBookings[1];
            Assert.That(secondSessionBooking.id, Is.InstanceOf<Guid>());
            Assert.That(secondSessionBooking.session.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id));
            Assert.That(secondSessionBooking.customer.id, Is.EqualTo(setup.Fred.Id));
            Assert.That(secondSessionBooking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            Assert.That(secondSessionBooking.hasAttended, Is.Null);

            //var firstBookedSession = booking.bookedSessions[0];
            //Assert.That(firstBookedSession.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Id));
            //Assert.That(firstBookedSession.name, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Description));
            //Assert.That(firstBookedSession.date, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Timing.startDate));
            //Assert.That(firstBookedSession.startTime, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Timing.startTime));
            //Assert.That(firstBookedSession.studentCapacity, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[0].Booking.studentCapacity));

            //var secondBookedSession = booking.bookedSessions[1];
            //Assert.That(secondBookedSession.id, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Id));
            //Assert.That(secondBookedSession.name, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Description));
            //Assert.That(secondBookedSession.date, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Timing.startDate));
            //Assert.That(secondBookedSession.startTime, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Timing.startTime));
            //Assert.That(secondBookedSession.studentCapacity, Is.EqualTo(setup.AaronOrakeiHolidayCamp9To15For3Days.Sessions[2].Booking.studentCapacity));
        }

        protected void AssertGetSingleSessionBooking(SingleSessionBookingData booking, 
                                                     ExpectedStandaloneSession session, 
                                                     ExpectedCustomer expectedCustomer)
        {
            Assert.That(booking.id, Is.InstanceOf<Guid>());
            Assert.That(booking.parentId, Is.Null);

            Assert.That(booking.session.id, Is.EqualTo(session.Id));
            Assert.That(booking.session.name, Is.EqualTo(session.Description));

            Assert.That(booking.customer.id, Is.EqualTo(expectedCustomer.Id));
            Assert.That(booking.customer.name, Is.EqualTo(string.Format("{0} {1}", expectedCustomer.FirstName, expectedCustomer.LastName)));

            Assert.That(booking.price, Is.EqualTo(session.Pricing.sessionPrice));
            Assert.That(booking.paymentStatus, Is.EqualTo(Constants.PAYMENT_STATUS_PENDING_INVOICE));
            Assert.That(booking.hasAttended, Is.Null);
        }
    }
}
