namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiBookingSetAttendanceCommand
    {
        public string commandName { get { return "BookingSetAttendance"; } }
        public bool? hasAttended { get; set; }
    }
}
