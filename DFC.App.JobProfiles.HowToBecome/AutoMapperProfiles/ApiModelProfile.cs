using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.ApiModels;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace DFC.App.JobProfiles.HowToBecome.AutoMapperProfiles
{
    public class HtmlStringFormatter : IValueConverter<string, List<string>>
    {
        public List<string> Convert(string sourceMember, ResolutionContext context)
        {
            return new List<string> { sourceMember };
        }
    }

    public class ApiModelProfile : Profile
    {
        public ApiModelProfile()
        {
            CreateMap<HowToBecomeSegmentDataModel, HowToBecomeApiModel>()
                .ForMember(d => d.EntryRouteSummary, opt => opt.ConvertUsing(new HtmlStringFormatter()))
                .ForMember(d => d.MoreInformation, s => s.MapFrom(a => a))
                ;

            CreateMap<HowToBecomeSegmentDataModel, MoreInformationApiModel>()
                .ForMember(d => d.CareerTips, opt => opt.ConvertUsing(new HtmlStringFormatter(), a => a.MoreInformation.CareerTips))
                .ForMember(d => d.FurtherInformation, opt => opt.ConvertUsing(new HtmlStringFormatter(), a => a.MoreInformation.FurtherInformation))
                .ForMember(d => d.ProfessionalAndIndustryBodies, opt => opt.ConvertUsing(new HtmlStringFormatter(), a => a.MoreInformation.ProfessionalAndIndustryBodies))
                .ForMember(d => d.Registrations, s => s.MapFrom(a => a.Registrations.Select(i => i.Description)))
                ;

            CreateMap<EntryRoutes, EntryRoutesApiModel>()
                .ForMember(d => d.University, s => s.MapFrom(a => a.CommonRoutes.FirstOrDefault(f => f.RouteName == Data.Enums.RouteName.University)))
                .ForMember(d => d.College, s => s.MapFrom(a => a.CommonRoutes.FirstOrDefault(f => f.RouteName == Data.Enums.RouteName.College)))
                .ForMember(d => d.Apprenticeship, s => s.MapFrom(a => a.CommonRoutes.FirstOrDefault(f => f.RouteName == Data.Enums.RouteName.Apprenticeship)))
                .ForMember(d => d.Work, opt => opt.ConvertUsing(new HtmlStringFormatter()))
                .ForMember(d => d.Volunteering, opt => opt.ConvertUsing(new HtmlStringFormatter()))
                .ForMember(d => d.DirectApplication, opt => opt.ConvertUsing(new HtmlStringFormatter()))
                .ForMember(d => d.OtherRoutes, opt => opt.ConvertUsing(new HtmlStringFormatter()))
                ;

            CreateMap<CommonRoutes, CommonRouteApiModel>()
                .ForMember(d => d.RelevantSubjects, opt => opt.ConvertUsing(new HtmlStringFormatter(), s => s.Subject))
                .ForMember(d => d.FurtherInformation, opt => opt.ConvertUsing(new HtmlStringFormatter()))
                .ForMember(d => d.EntryRequirements, s => s.MapFrom(a => a.EntryRequirements.Select(i => i.Description)))
                .ForMember(d => d.AdditionalInformation, s => s.MapFrom(a => a.AdditionalInformation.Select(i => $"[{i.Text} | {i.Link}]")))
                ;
        }
    }
}