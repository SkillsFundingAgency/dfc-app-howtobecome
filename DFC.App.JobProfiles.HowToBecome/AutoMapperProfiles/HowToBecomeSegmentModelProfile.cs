using AutoMapper;
using DFC.App.JobProfiles.HowToBecome.Data.Models;
using DFC.App.JobProfiles.HowToBecome.ViewModels;
using Microsoft.AspNetCore.Html;

namespace DFC.App.JobProfiles.HowToBecome.AutoMapperProfiles
{
    public class HowToBecomeSegmentModelProfile : Profile
    {
        public HowToBecomeSegmentModelProfile()
        {
            CreateMap<HowToBecomeSegmentDataModel, DocumentDataViewModel>()
                .ForMember(d => d.Markup, s => s.MapFrom(a => new HtmlString(a.Markup)));

            CreateMap<HowToBecomeSegmentModel, DocumentViewModel>();

            CreateMap<HowToBecomeSegmentModel, IndexDocumentViewModel>();
        }
    }
}