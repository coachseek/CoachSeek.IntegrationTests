namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Coach
{
    public class CoachSteve : ExpectedCoach
    {
        public override string FirstName { get { return "Steve"; } }
        public override string LastName { get { return "Fergusson"; } }
        public override string Name { get { return "Steve Fergusson"; } }


        public CoachSteve()
            : base(Random.RandomEmail, Random.RandomString, ApiWeeklyWorkingHours.CreateStandardWorkingHours())
        { }
    }
}
