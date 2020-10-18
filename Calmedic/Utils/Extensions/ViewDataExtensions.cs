using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calmedic.Utils
{
    public static class ViewDataExtensions
    {
        public static IDictionary<string, object> Model(this ViewDataDictionary viewData)
        {
            return viewData.ModelState.ToDictionary(modelState => modelState.Key,
                modelState => modelState.Value.RawValue != null ? (modelState.Value.RawValue ?? "No data") : "No value");
        }

        public static IDictionary<string, string> ModelErrors(this ViewDataDictionary viewData)
        {
            return viewData.ModelState.Where(modelState => modelState.Value.Errors.Any())
                .ToDictionary(modelState => modelState.Key,
                              modelState => modelState.Value.Errors.First().ErrorMessage);
        }
    }
}