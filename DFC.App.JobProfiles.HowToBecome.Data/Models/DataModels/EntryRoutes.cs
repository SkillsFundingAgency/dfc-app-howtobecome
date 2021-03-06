﻿using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels
{
    public class EntryRoutes
    {
        public IList<CommonRoutes> CommonRoutes { get; set; }

        public string Work { get; set; }

        public string Volunteering { get; set; }

        public string DirectApplication { get; set; }

        public string OtherRoutes { get; set; }
    }
}