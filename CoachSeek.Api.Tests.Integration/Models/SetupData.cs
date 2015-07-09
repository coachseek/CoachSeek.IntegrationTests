using CoachSeek.Api.Tests.Integration.Models.Expectations;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Booking;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Coach;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Course;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Customer;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Location;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Service;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Session;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class SetupData
    {
        public ExpectedBusiness Business { get; private set; }
        public LocationOrakei Orakei { get; set; }
        public LocationRemuera Remuera { get; set; }
        public CoachAaron Aaron { get; set; }
        public CoachBobby Bobby { get; set; }
        public ServiceMiniRed MiniRed { get; set; }
        public ServiceHolidayCamp HolidayCamp { get; set; }
        public CustomerFred Fred { get; set; }
        public CustomerWilma Wilma { get; set; }
        public CustomerBarney Barney { get; set; }
        public CustomerBamBam BamBam { get; set; }
        public StandaloneAaronOrakeiMiniRed14To15 AaronOrakeiMiniRed14To15 { get; set; }
        public StandaloneAaronOrakeiMiniRed16To17 AaronOrakeiMiniRed16To17 { get; set; }
        public CourseBobbyRemueraMiniRed9To10For3Weeks BobbyRemueraMiniRed9To10For3Weeks { get; set; }
        public CourseAaronOrakeiHolidayCamp9To15For3Days AaronOrakeiHolidayCamp9To15For3Days { get; set; }
        public BookingFredOnStandaloneAaronOrakeiMiniRed14To15 FredOnAaronOrakeiMiniRed14To15 { get; set; }
        public BookingWilmaOnStandaloneAaronOrakeiMiniRed14To15 WilmaOnAaronOrakeiMiniRed14To15 { get; set; }
        public BookingBarneyOnStandaloneAaronOrakeiMiniRed14To15 BarneyOnAaronOrakeiMiniRed14To15 { get; set; }
        public BookingWilmaOnStandaloneAaronOrakeiMiniRed16To17 WilmaOnAaronOrakeiMiniRed16To17 { get; set; }
        public ExpectedBooking FredOnAaronOrakeiHolidayCamp9To15For3Days { get; set; }
        public ExpectedBooking FredOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days { get; set; }
        public ExpectedBooking WilmaOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days { get; set; }
        public ExpectedBooking BarneyOnLastCourseSessionInAaronOrakeiHolidayCamp9To15For3Days { get; set; }


        public SetupData(RandomBusiness business)
        {
            Business = business;
        }

        public SetupData(RegistrationData registration, string password)
        {
            Business = new ExpectedBusiness(registration.business.name,
                                            registration.business.payment.currency,
                                            registration.business.payment.isOnlinePaymentEnabled,
                                            registration.business.payment.forceOnlinePayment,
                                            registration.business.payment.paymentProvider,
                                            registration.business.payment.merchantAccountIdentifier,
                                            registration.admin.firstName,
                                            registration.admin.lastName,
                                            registration.admin.email,
                                            password)
            {
                Id = registration.business.id,
                Domain = registration.business.domain
            };
        }
    }
}
