namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiWeeklyWorkingHours
    {
        public ApiDailyWorkingHours monday { get; set; }
        public ApiDailyWorkingHours tuesday { get; set; }
        public ApiDailyWorkingHours wednesday { get; set; }
        public ApiDailyWorkingHours thursday { get; set; }
        public ApiDailyWorkingHours friday { get; set; }
        public ApiDailyWorkingHours saturday { get; set; }
        public ApiDailyWorkingHours sunday { get; set; }
    }
}
