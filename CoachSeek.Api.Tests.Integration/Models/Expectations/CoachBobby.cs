namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class CoachBobby : ExpectedCoach
    {
        public override string FirstName { get { return "Bobby"; } }
        public override string LastName { get { return "Smith"; } }
        public override string Name { get { return "Bobby Smith"; } }


        public CoachBobby()
            : base(Random.RandomEmail, Random.RandomString, ExpectedWeeklyWorkingHours.CreateStandardWorkingHours())
        { }
    }
}
