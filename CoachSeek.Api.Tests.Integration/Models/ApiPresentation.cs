namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiPresentation
    {
        public string colour { get; set; }

        public ApiPresentation() { }

        public ApiPresentation(string colour)
        {
            this.colour = colour;
        }
    }
}
