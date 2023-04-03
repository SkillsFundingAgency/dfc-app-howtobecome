﻿using System.Collections.Generic;

namespace DFC.App.JobProfiles.HowToBecome.FunctionalTests.Model.ContentType
{
    public class HowToBecomeData
    {
        public List<RouteEntry> RouteEntries { get; set; }

        public FurtherInformationModel FurtherInformation { get; set; }

        public FurtherRoutes FurtherRoutes { get; set; }

        public string IntroText { get; set; }

        public List<Registration> Registrations { get; set; }
    }
}