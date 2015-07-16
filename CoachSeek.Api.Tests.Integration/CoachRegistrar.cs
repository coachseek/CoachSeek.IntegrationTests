using Coachseek.API.Client.Models;
using Coachseek.API.Client.Services;
using CoachSeek.Api.Tests.Integration.Clients;
using CoachSeek.Api.Tests.Integration.Models;
using CoachSeek.Api.Tests.Integration.Models.Expectations;
using CoachSeek.Api.Tests.Integration.Models.Expectations.Coach;

namespace CoachSeek.Api.Tests.Integration
{
    public static class CoachRegistrar
    {
        public static void RegisterCoach(ExpectedCoach coach, ExpectedBusiness business)
        {
            var json = CreateNewCoachSaveCommand(coach);
            var response = PostCoach(business, json);
            if (response.Payload != null)
                coach.Id = ((CoachData)response.Payload).id;
        }

        private static string CreateNewCoachSaveCommand(ExpectedCoach expectedCoach)
        {
            var command = new ApiCoachSaveCommand
            {
                firstName = expectedCoach.FirstName,
                lastName = expectedCoach.LastName,
                email = expectedCoach.Email,
                phone = expectedCoach.Phone,
                workingHours = expectedCoach.WorkingHours
            };

            return JsonSerialiser.Serialise(command);
        }

        private static ApiResponse PostCoach(ExpectedBusiness business, string json)
        {
            return new TestAuthenticatedApiClient().Post<CoachData>(json, business.UserName, business.Password, "Coaches");
        }
    }
}
