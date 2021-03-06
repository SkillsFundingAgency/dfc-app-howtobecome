﻿using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.ApiModels
{
    public class EntryRoutesApiModel
    {
        public CommonRouteApiModel University { get; set; }

        public CommonRouteApiModel College { get; set; }

        public CommonRouteApiModel Apprenticeship { get; set; }

        public List<string> Work { get; set; }

        public List<string> Volunteering { get; set; }

        public List<string> DirectApplication { get; set; }

        public List<string> OtherRoutes { get; set; }
    }
}