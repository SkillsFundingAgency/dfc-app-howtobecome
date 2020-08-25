﻿using System;

namespace DFC.App.JobProfiles.HowToBecome.Tests.API.IntegrationTests.Model.ContentType
{
    public class Location
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Uri Url { get; set; }

        public bool IsNegative { get; set; }
    }
}