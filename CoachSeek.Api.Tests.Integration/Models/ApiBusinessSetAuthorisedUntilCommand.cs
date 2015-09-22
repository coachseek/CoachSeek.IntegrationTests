using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiBusinessSetAuthorisedUntilCommand
    {
        public string commandName { get { return "BusinessSetAuthorisedUntil"; } }
        public DateTime authorisedUntil { get; set; }
    }
}
