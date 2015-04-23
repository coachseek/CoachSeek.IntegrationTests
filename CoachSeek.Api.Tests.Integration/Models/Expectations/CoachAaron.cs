namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public class CoachAaron : ExpectedCoach
    {
        public override string FirstName { get { return "Aaron"; } }
        public override string LastName { get { return "Smith"; } }
        public override string Name { get { return "Aaron Smith"; } }


        public CoachAaron()
            : base(Random.RandomEmail, Random.RandomString, ExpectedWeeklyWorkingHours.CreateStandardWorkingHours())
        { }
    }
}
