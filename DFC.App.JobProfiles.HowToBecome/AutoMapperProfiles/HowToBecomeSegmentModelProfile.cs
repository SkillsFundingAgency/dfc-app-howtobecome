using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using DFC.App.JobProfiles.HowToBecome.ViewModels;
using Microsoft.AspNetCore.Html;

namespace DFC.App.JobProfiles.HowToBecome.AutoMapperProfiles
{
    public class HowToBecomeSegmentModelProfile : Profile
    {
        public HowToBecomeSegmentModelProfile()
        {
            CreateMap<AdditionalInformation, ViewModels.DataModels.AdditionalInformation>();

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