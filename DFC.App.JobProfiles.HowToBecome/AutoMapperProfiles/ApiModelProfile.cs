using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.ApiModels;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using DFC.HtmlToDataTranslator.ValueConverters;
using System.Linq;

namespace DFC.App.JobProfiles.HowToBecome.AutoMapperProfiles
{
    public class ApiModelProfile : Profile
    {
        public ApiModelProfile()
        {
            var htmlToStringConverter = new HtmlToStringValueConverter();

            CreateMap<HowToBecomeSegmentDataModel, HowToBecomeApiModel>()
                .ForMember(d => d.EntryRouteSummary, opt => opt.ConvertUsing(htmlToStringConverter))
                .ForMember(d => d.MoreInformation, s => s.MapFrom(a => a))
                ;

            CreateMap<HowToBecomeSegmentDataModel, MoreInformationApiModel>()
                .ForMember(d => d.CareerTips, opt => opt.ConvertUsing(htmlToStringConverter, a => a.MoreInformation.CareerTips))
                .ForMember(d => d.FurtherInformation, opt => opt.ConvertUsing(htmlToStringConverter, a => a.MoreInformation.FurtherInformation))
                .ForMember(d => d.ProfessionalAndIndustryBodies, opt => opt.ConvertUsing(htmlToStringConverter, a => a.MoreInformation.ProfessionalAndIndustryBodies))
                .ForMember(d => d.Registrations, s => s.MapFrom(a => a.Registrations.Select(i => i.Description)))
                ;

            CreateMap<EntryRoutes, EntryRoutesApiModel>()
                .ForMember(d => d.University, s => s.MapFrom(a => a.CommonRoutes.FirstOrDefault(f => f.RouteName == Data.Enums.RouteName.University)))
                .ForMember(d => d.College, s => s.MapFrom(a => a.CommonRoutes.FirstOrDefault(f => f.RouteName == Data.Enums.RouteName.College)))
                .ForMember(d => d.Apprenticeship, s => s.MapFrom(a => a.CommonRoutes.FirstOrDefault(f => f.RouteName == Data.Enums.RouteName.Apprenticeship)))
                .ForMember(d => d.Work, opt => opt.ConvertUsing(htmlToStringConverter))
                .ForMember(d => d.Volunteering, opt => opt.ConvertUsing(htmlToStringConverter))
                .ForMember(d => d.DirectApplication, opt => opt.ConvertUsing(htmlToStringConverter))
                .ForMember(d => d.OtherRoutes, opt => opt.ConvertUsing(htmlToStringConverter))
                ;

            CreateMap<CommonRoutes, CommonRouteApiModel>()
                .ForMember(d => d.RelevantSubjects, opt => opt.ConvertUsing(htmlToStringConverter, s => s.Subject))
                .ForMember(d => d.FurtherInformation, opt => opt.ConvertUsing(htmlToStringConverter))
                .ForMember(d => d.EntryRequirements, s => s.MapFrom(a => a.EntryRequirements.Select(i => i.Description)))
                .ForMember(d => d.AdditionalInformation, s => s.MapFrom(a => a.AdditionalInformation.Select(i => $"[{i.Text} | {i.Link}]")))
                ;
        }
    }
}