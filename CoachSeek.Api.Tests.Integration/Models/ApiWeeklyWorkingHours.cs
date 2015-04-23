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


        public static ApiWeeklyWorkingHours CreateStandardWorkingHours()
        {
            return new ApiWeeklyWorkingHours
            {
                monday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                tuesday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                wednesday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                thursday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                friday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                saturday = new ApiDailyWorkingHours(),
                sunday = new ApiDailyWorkingHours()
            };
        }

        public static ApiWeeklyWorkingHours CreateWeekendWorkingHours()
        {
            return new ApiWeeklyWorkingHours
            {
                monday = new ApiDailyWorkingHours(),
                tuesday = new ApiDailyWorkingHours(),
                wednesday = new ApiDailyWorkingHours(),
                thursday = new ApiDailyWorkingHours(),
                friday = new ApiDailyWorkingHours(),
                saturday = new ApiDailyWorkingHours(true, "9:00", "17:00"),
                sunday = new ApiDailyWorkingHours(true, "9:00", "17:00")
            };
        }
    }
}
