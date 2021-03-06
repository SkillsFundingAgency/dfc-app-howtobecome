﻿using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using DFC.App.JobProfiles.HowToBecome.Data.Models.PatchModels;
using DFC.App.JobProfiles.HowToBecome.Data.ServiceBusModels;
using DFC.App.JobProfiles.HowToBecome.ViewModels;
using Microsoft.AspNetCore.Html;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobProfiles.HowToBecome.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class HowToBecomeSegmentModelProfile : Profile
    {
        public HowToBecomeSegmentModelProfile()
        {
            // Service Bus maps
            CreateMap<PatchLinksModel, AdditionalInformation>()
               .ForMember(d => d.Link, s => s.MapFrom(a => a.Url.ToString()));

            CreateMap<PatchRequirementsModel, EntryRequirement>()
               .ForMember(d => d.Description, s => s.MapFrom(a => a.Info));

            CreateMap<PatchRegistrationModel, Registration>()
               .ForMember(d => d.Description, s => s.MapFrom(a => a.Info));

            CreateMap<HowToBecomeSegmentModel, RefreshJobProfileSegmentServiceBusModel>()
                .ForMember(d => d.JobProfileId, s => s.MapFrom(a => a.DocumentId))
                .ForMember(d => d.Segment, s => s.MapFrom(a => HowToBecomeSegmentDataModel.SegmentName));

            // View Models maps
            CreateMap<EntryRequirement, ViewModels.DataModels.EntryRequirement>();

            CreateMap<AdditionalInformation, ViewModels.DataModels.AdditionalInformation>();

            CreateMap<Registration, ViewModels.DataModels.Registration>();

            CreateMap<MoreInformation, ViewModels.DataModels.MoreInformation>()
                .ForMember(d => d.ProfessionalAndIndustryBodies, s => s.MapFrom(a => new HtmlString(a.ProfessionalAndIndustryBodies)))
                .ForMember(d => d.CareerTips, s => s.MapFrom(a => new HtmlString(a.CareerTips)))
                .ForMember(d => d.FurtherInformation, s => s.MapFrom(a => new HtmlString(a.FurtherInformation)));

            CreateMap<CommonRoutes, ViewModels.DataModels.CommonRoutes>()
                .ForMember(d => d.Subject, s => s.MapFrom(a => new HtmlString(a.Subject)))
                .ForMember(d => d.FurtherInformation, s => s.MapFrom(a => new HtmlString(a.FurtherInformation)));

            CreateMap<EntryRoutes, ViewModels.DataModels.EntryRoutes>()
                .ForMember(d => d.Work, s => s.MapFrom(a => new HtmlString(a.Work)))
                .ForMember(d => d.Volunteering, s => s.MapFrom(a => new HtmlString(a.Volunteering)))
                .ForMember(d => d.DirectApplication, s => s.MapFrom(a => new HtmlString(a.DirectApplication)))
                .ForMember(d => d.OtherRoutes, s => s.MapFrom(a => new HtmlString(a.OtherRoutes)));

            CreateMap<HowToBecomeSegmentDataModel, DocumentDataViewModel>()
                .ForMember(d => d.Title, s => s.MapFrom(a => new HtmlString(a.Title)))
                .ForMember(d => d.EntryRouteSummary, s => s.MapFrom(a => new HtmlString(a.EntryRouteSummary)));

            CreateMap<HowToBecomeSegmentModel, DocumentViewModel>();

            CreateMap<HowToBecomeSegmentModel, IndexDocumentViewModel>();
        }
    }
}