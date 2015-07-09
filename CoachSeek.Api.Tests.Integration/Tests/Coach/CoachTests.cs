using CoachSeek.Api.Tests.Integration.Models;

namespace CoachSeek.Api.Tests.Integration.Tests.Coach
{
    public abstract class CoachTests : ScheduleTests
    {
        protected override string RelativePath
        {
            get { return "Coaches"; }
        }


        protected ApiWeeklyWorkingHours SetupStandardWorkingHours()
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

        protected ApiWeeklyWorkingHours SetupWeekendWorkingHours()
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
