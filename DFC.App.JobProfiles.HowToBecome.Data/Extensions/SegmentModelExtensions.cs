using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using System.Linq;

namespace DFC.App.JobProfiles.HowToBecome.Data
{
    public static class SegmentModelExtensions
    {
        public static CommonRoutes GetExistingCommonRoute(this HowToBecomeSegmentModel existingSegmentModel, RouteName routeName)
        {
            return existingSegmentModel
                ?.Data
                .EntryRoutes
                .CommonRoutes
                .SingleOrDefault(e => e.RouteName == routeName);
        }
    }
}