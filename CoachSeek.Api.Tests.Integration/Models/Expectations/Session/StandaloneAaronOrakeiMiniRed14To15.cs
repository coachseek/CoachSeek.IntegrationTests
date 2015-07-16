﻿using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Session
{
    public class StandaloneAaronOrakeiMiniRed14To15 : ExpectedStandaloneSession
    {
        public StandaloneAaronOrakeiMiniRed14To15(Guid coachId, Guid locationId, Guid serviceId, string startDate)
            : base(coachId, locationId, serviceId, startDate, "14:00", 60, 3, true, 19.95m, "red")
        { }
    }
}