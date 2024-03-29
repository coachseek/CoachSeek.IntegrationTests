﻿using System;

namespace CoachSeek.Api.Tests.Integration.Models.Expectations.Location
{
    public abstract class ExpectedLocation
    {
        public Guid Id { get; set; }

        public virtual string Name { get; private set; }
    }
}
