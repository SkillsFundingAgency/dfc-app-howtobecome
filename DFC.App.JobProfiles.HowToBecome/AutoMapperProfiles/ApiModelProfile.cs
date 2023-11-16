using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.ApiModels;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.Data.Models.DataModels;
using DFC.HtmlToDataTranslator.Contracts;
using DFC.HtmlToDataTranslator.Services;
using DFC.HtmlToDataTranslator.ValueConverters;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DFC.App.JobProfiles.HowToBecome.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class ApiModelProfile : Profile
    {
        private readonly IHtmlToDataTranslator htmlTranslator;

        public ApiModelProfile()
        {
            htmlTranslator = new HtmlAgilityPackDataTranslator();
            var htmlToStringConverter = new HtmlToStringValueConverter(htmlTranslator);

            CreateMap<HowToBecomeSegmentDataModel, HowToBecomeApiModel>()
                .ForMember(d => d.EntryRouteSummary, opt => opt.ConvertUsing(htmlToStringConverter))
                .ForMember(d => d.MoreInformation, s => s.MapFrom(a => a))
                .ForMember(d => d.RealStory, s => s.MapFrom(a => a.RealStory))
                ;

            CreateMap<HowToBecomeSegmentDataModel, MoreInformationApiModel>()
                .ForMember(d => d.CareerTips, opt => opt.ConvertUsing(htmlToStringConverter, a => a.MoreInformation.CareerTips))
                .ForMember(d => d.FurtherInformation, opt => opt.ConvertUsing(htmlToStringConverter, a => a.MoreInformation.FurtherInformation))
                .ForMember(d => d.ProfessionalAndIndustryBodies, opt => opt.ConvertUsing(htmlToStringConverter, a => a.MoreInformation.ProfessionalAndIndustryBodies))
                .ForMember(d => d.Registrations, s => s.MapFrom(ConvertRegistrationsToList))
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
                .ForMember(d => d.EntryRequirements, opt => opt.MapFrom(ConvertEntryRequirementsToList))
                .ForMember(d => d.AdditionalInformation, opt => opt.MapFrom(a => a.AdditionalInformation.Select(i => $"[{i.Text} | {i.Link}]")))
                ;
        }

        private List<string> ConvertEntryRequirementsToList(CommonRoutes source, CommonRouteApiModel destination)
        {
            return ConvertToList(source.EntryRequirements.Select(x => x.Description).ToList());
        }

        private List<string> ConvertRegistrationsToList(HowToBecomeSegmentDataModel source, MoreInformationApiModel destination)
        {
            return ConvertToList(source.Registrations.Select(x => x.Description).ToList());
        }

        private List<string> ConvertToList(List<string> source)
        {
            var result = new List<string>();
            foreach (var item in source)
            {
                result.AddRange(htmlTranslator.Translate(item));
            }

            return result;
        }
    }
}