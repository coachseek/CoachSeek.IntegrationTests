namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Service
{
    public abstract class ExpectedStandaloneService : ExpectedService
    {
        protected ExpectedStandaloneService(string colour, int? duration = null, decimal? price = null, int? studentCapacity = null, bool? isOnlineBookable = null)
            : base(colour, 1, null, duration, price, null, studentCapacity, isOnlineBookable)
        { }
    }
}
