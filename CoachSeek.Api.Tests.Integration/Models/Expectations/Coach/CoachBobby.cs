namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Coach
{
    public class CoachBobby : ExpectedCoach
    {
        public override string FirstName { get { return "Bobby"; } }
        public override string LastName { get { return "Smith"; } }
        public override string Name { get { return "Bobby Smith"; } }


        public CoachBobby()
            : base(Random.RandomEmail, Random.RandomString, ApiWeeklyWorkingHours.CreateStandardWorkingHours())
        { }
    }
}
