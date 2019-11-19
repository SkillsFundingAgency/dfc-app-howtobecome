﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace DFC.App.JobProfiles.HowToBecome.Views.Tests.Extensions
{
    public static class IViewComponentResultExtensions
    {
        public static T ViewDataModelAs<T>(this IViewComponentResult viewComponentResult)
        {
            var componentResult = viewComponentResult as ViewViewComponentResult;

            var viewComponentModel = (T)componentResult?.ViewData.Model;

            return viewComponentModel;
        }
    }
}