namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class ExpectedWeeklyWorkingHours
    {
        public ExpectedDailyWorkingHours Monday { get; set; }
        public ExpectedDailyWorkingHours Tuesday { get; set; }
        public ExpectedDailyWorkingHours Wednesday { get; set; }
        public ExpectedDailyWorkingHours Thursday { get; set; }
        public ExpectedDailyWorkingHours Friday { get; set; }
        public ExpectedDailyWorkingHours Saturday { get; set; }
        public ExpectedDailyWorkingHours Sunday { get; set; }


        public static ExpectedWeeklyWorkingHours CreateStandardWorkingHours()
        {
            return new ExpectedWeeklyWorkingHours
            {
                Monday = new ExpectedDailyWorkingHours(true, "9:00", "17:00"),
                Tuesday = new ExpectedDailyWorkingHours(true, "9:00", "17:00"),
                Wednesday = new ExpectedDailyWorkingHours(true, "9:00", "17:00"),
                Thursday = new ExpectedDailyWorkingHours(true, "9:00", "17:00"),
                Friday = new ExpectedDailyWorkingHours(true, "9:00", "17:00"),
                Saturday = new ExpectedDailyWorkingHours(),
                Sunday = new ExpectedDailyWorkingHours()
            };
        }

        public static ExpectedWeeklyWorkingHours CreateWeekendWorkingHours()
        {
            return new ExpectedWeeklyWorkingHours
            {
                Monday = new ExpectedDailyWorkingHours(),
                Tuesday = new ExpectedDailyWorkingHours(),
                Wednesday = new ExpectedDailyWorkingHours(),
                Thursday = new ExpectedDailyWorkingHours(),
                Friday = new ExpectedDailyWorkingHours(),
                Saturday = new ExpectedDailyWorkingHours(true, "9:00", "17:00"),
                Sunday = new ExpectedDailyWorkingHours(true, "9:00", "17:00")
            };
        }


        public ApiWeeklyWorkingHours ToApi()
        {
            return new ApiWeeklyWorkingHours
            {
                monday = Monday.ToApi(),
                tuesday = Tuesday.ToApi(),
                wednesday = Wednesday.ToApi(),
                thursday = Thursday.ToApi(),
                friday = Friday.ToApi(),
                saturday = Saturday.ToApi(),
                sunday = Sunday.ToApi()
            };
        }
    }
}
