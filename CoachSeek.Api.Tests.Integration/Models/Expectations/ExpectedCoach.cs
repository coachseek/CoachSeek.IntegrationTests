using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations
{
    public abstract class ExpectedCoach
    {
        public Guid Id { get; set; }

        public abstract string FirstName { get; }
        public abstract string LastName { get; }
        public abstract string Name { get; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public ExpectedWeeklyWorkingHours WorkingHours { get; private set; }


        protected ExpectedCoach() 
        { }

        protected ExpectedCoach(string email, string phone, ExpectedWeeklyWorkingHours workingHours)
        {
            Email = email;
            Phone = phone.ToUpperInvariant();
            WorkingHours = workingHours;
        }
    }
}
