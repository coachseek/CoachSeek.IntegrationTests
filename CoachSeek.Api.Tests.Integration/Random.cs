using System;

namespace CoachSeek.Api.Tests.Integration
{
    public static class Random
    {
        public static string RandomEmail
        {
            get
            {
                return string.Format("{0}@{1}.com", RandomString, RandomString).ToLower();
            }
        }

        public static string RandomString
        {
            get { return Guid.NewGuid().ToString().ToLower().Replace("-", ""); }
        }
    }
}
