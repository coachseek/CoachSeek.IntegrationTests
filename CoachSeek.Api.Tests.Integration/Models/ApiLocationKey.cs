﻿using System;

namespace CoachSeek.Api.Tests.Integration.Models
{
    public class ApiLocationKey
    {
        public Guid? id { get; set; }
        public string name { get; set; }

        public ApiLocationKey()
        { }

        public ApiLocationKey(Guid? id)
        {
            this.id = id;
        }
    }
}
