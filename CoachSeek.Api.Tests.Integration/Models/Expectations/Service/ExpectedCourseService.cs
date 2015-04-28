namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Service
{
    public abstract class ExpectedCourseService : ExpectedService
    {
        protected ExpectedCourseService(string colour, 
                                        int sessionCount, 
                                        string repeatFrequency, 
                                        int? duration = null, 
                                        decimal? sessionPrice = null, 
                                        decimal? coursePrice = null, 
                                        int? studentCapacity = null, 
                                        bool? isOnlineBookable = null)
            : base(colour, sessionCount, repeatFrequency, duration, sessionPrice, coursePrice, studentCapacity, isOnlineBookable)
        { }
    }
}
