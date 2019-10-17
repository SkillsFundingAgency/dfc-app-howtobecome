using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Enums;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels.PatchContentTypeModels;
using System;

namespace DFC.App.JobProfiles.HowToBecome.MessageFunctionApp.AutoMapperProfile
{
    public class HowToBecomeSegmentModelProfile : Profile
    {
        public HowToBecomeSegmentModelProfile()
        {
            CreateMap<SitefinityHowToBecomeMessage, EntryRoutes>()
                .ForMember(d => d.CommonRoutes, s => s.MapFrom(a => a.RouteEntries))
                .ForMember(d => d.DirectApplication, s => s.MapFrom(a => a.FurtherRoutes.DirectApplication))
                .ForMember(d => d.OtherRoutes, s => s.MapFrom(a => a.FurtherRoutes.OtherRoutes))
                .ForMember(d => d.Volunteering, s => s.MapFrom(a => a.FurtherRoutes.Volunteering))
                .ForMember(d => d.Work, s => s.MapFrom(a => a.FurtherRoutes.Work));

            CreateMap<JobProfileMessage, HowToBecomeSegmentModel>()
                .ForMember(d => d.Data, s => s.MapFrom(a => a))
                .ForMember(d => d.DocumentId, s => s.MapFrom(a => a.JobProfileId))
                .ForMember(d => d.SocLevelTwo, s => s.MapFrom(a => a.SocCodeId))
                .ForMember(d => d.Etag, s => s.Ignore());

            CreateMap<SitefinityFurtherMoreInformation, MoreInformation>();
            CreateMap<SitefinityHowToBecomeMessage, MoreInformation>()
                .ForAllMembers(o => o.MapFrom(s => s.FurtherMoreInformation));

            CreateMap<SitefinityRegistrations, Registration>()
                .ForMember(d => d.Description, s => s.MapFrom(a => a.Info));

            CreateMap<SitefinityHowToBecomeMessage, HowToBecomeSegmentDataModel>()
                .ForMember(d => d.EntryRouteSummary, s => s.MapFrom(a => a.IntroText));

            CreateMap<JobProfileMessage, HowToBecomeSegmentDataModel>()
                .ForMember(d => d.TitlePrefix, s => s.MapFrom((message, model) =>
                {
                    var canBeParsed = Enum.TryParse<TitlePrefix>(message.DynamicTitlePrefix, out var result);
                    return canBeParsed ? result : TitlePrefix.AsDefined;
                }))
                .ForMember(d => d.Title, s => s.MapFrom(a => a.Title))
                .ForMember(d => d.LastReviewed, s => s.MapFrom(o => o.LastModified))
                .ForMember(d => d.Registrations, o => o.MapFrom(s => s.HowToBecomeData.Registrations))
                .ForMember(d => d.MoreInformation, o => o.MapFrom(s => s.HowToBecomeData.FurtherMoreInformation))
                .ForMember(d => d.EntryRouteSummary, o => o.MapFrom(s => s.HowToBecomeData.IntroText))
                .ForMember(d => d.SegmentName, s => s.Ignore())
                .ForAllOtherMembers(o => o.MapFrom(s => s.HowToBecomeData));

            CreateMap<SitefinityMoreInformationLinks, AdditionalInformation>()
                .ForMember(d => d.Link, s => s.MapFrom(a => a.Url));

            CreateMap<Data.ServiceBusModels.SitefinityEntryRequirement, Data.Models.DataModels.EntryRequirement>()
                .ForMember(d => d.Description, s => s.MapFrom(a => a.Info));

            CreateMap<SitefinityRouteEntries, CommonRoutes>()
                .ForMember(d => d.RouteName, s => s.MapFrom(a => Enum.Parse<RouteName>(a.RouteName.ToString()))) // parse this properly
                .ForMember(d => d.Subject, s => s.MapFrom(a => a.RouteSubjects))
                .ForMember(d => d.FurtherInformation, s => s.MapFrom(a => a.FurtherRouteInformation))
                .ForMember(d => d.EntryRequirementPreface, s => s.MapFrom(a => a.RouteRequirement))
                .ForMember(d => d.AdditionalInformation, s => s.MapFrom(a => a.MoreInformationLinks));

            CreateMap<SitefinityFurtherRoutes, EntryRoutes>();

            CreateMap<PatchLinksServiceBusModel, PatchLinksModel>();

            CreateMap<PatchRequirementsServiceBusModel, PatchRequirementsModel>();
            CreateMap<PatchSimpleClassificationServiceBusModel, PatchSimpleClassificationModel>();
            CreateMap<PatchRegistrationsServiceBusModel, PatchRegistrationModel>();
        }
    }
}