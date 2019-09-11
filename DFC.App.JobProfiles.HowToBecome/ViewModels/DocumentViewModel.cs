using Microsoft.AspNetCore.Html;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.JobProfiles.HowToBecome.ViewModels
{
    public class DocumentViewModel
    {
        [Display(Name = "Document Id")]
        public Guid? DocumentId { get; set; }

        [Display(Name = "Canonical Name")]
        public string CanonicalName { get; set; }

        [Display(Name = "Canonical Name")]
        public string Title { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public HtmlString Markup { get; set; }

        public DocumentDataViewModel Data { get; set; }
    }
}