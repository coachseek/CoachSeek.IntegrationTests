using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Coach;
using Newtonsoft.Json;

namespace CoachSeek.Api.Tests.Integration
{
    public static class CoachRegistrar
    {
        public static void RegisterCoach(ExpectedCoach coach, ExpectedBusiness business)
        {
            var json = CreateNewCoachSaveCommand(coach);
            var response = PostCoach(business, json);
            coach.Id = ((CoachData)response.Payload).id;
        }

        private static string CreateNewCoachSaveCommand(ExpectedCoach expectedCoach)
        {
            var coach = new ApiCoachSaveCommand
            {
                firstName = expectedCoach.FirstName,
                lastName = expectedCoach.LastName,
                email = expectedCoach.Email,
                phone = expectedCoach.Phone,
                workingHours = expectedCoach.WorkingHours
            };

            return JsonConvert.SerializeObject(coach);
        }

        private static Response PostCoach(ExpectedBusiness business, string json)
        {
            return WebClient.AuthenticatedPost<CoachData>(business.UserName, business.Password, "Coaches", json);
        }
    }
}
