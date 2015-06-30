namespace CoachSeek.Api.Tests.Integration.Models
{
    class ApiBookingSetPaymentStatusCommand
    {
        public string commandName { get { return "BookingSetPaymentStatus"; } }
        public string paymentStatus { get; set; }
    }
}
