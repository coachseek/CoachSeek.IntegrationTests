using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Coach
{
    public abstract class ExpectedCoach
    {
        public Guid Id { get; set; }

        public abstract string FirstName { get; }
        public abstract string LastName { get; }
        public abstract string Name { get; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public ApiWeeklyWorkingHours WorkingHours { get; private set; }


        protected ExpectedCoach(string email, string phone, ApiWeeklyWorkingHours workingHours)
        {
            Email = email;
            Phone = phone.ToUpperInvariant();
            WorkingHours = workingHours;
        }
    }
}
