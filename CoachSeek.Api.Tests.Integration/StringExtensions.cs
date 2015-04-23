namespace CoachSeek.Api.Tests.Integration
{
    public static class StringExtensions
    {
        public static string Capitalise(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
