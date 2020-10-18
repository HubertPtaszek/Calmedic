using Calmedic.Domain;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace Calmedic.Utils
{  
    public abstract class PresentationController : Controller
    {
        public MainContext MainContext { get; set; }

        private static object _lockObject = new object();
     
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl-PL");
            base.OnActionExecuting(filterContext);

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        protected JsonResult CustomJson(object data, string dateTimeFormat = DateTimeFormats.IsoDateTimeFormat)
        {
            var settings = new JsonSerializerSettings();
            settings.DateFormatString = dateTimeFormat;
            JsonResult result = Json(data, settings);
            return result;
        }

    }
}