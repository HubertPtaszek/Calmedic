using NLog;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace Calmedic.Utils
{
    [InternalAuthorization]
    public abstract class AppController : Controller
    {
        public MainContext MainContext { get; set; }
      
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

        private Logger logger = LogManager.GetCurrentClassLogger();

        [NonAction]
        protected void LogInfo(string msg)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            logger.Info(string.Format(Loggers.LogFormat, DateTime.Now.ToDateTimeStringSafe(), this.GetType().Name, sf.GetMethod().Name, msg));
        }

        [NonAction]
        protected void LogError(string msg, Exception ex)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            logger.Error(ex, string.Format(Loggers.LogFormat, DateTime.Now.ToDateTimeStringSafe(), this.GetType().Name, sf.GetMethod().Name, msg));
        }

        [NonAction]
        protected string RenderViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = this.ControllerContext.ActionDescriptor.ActionName;
            }

            this.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = this.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(this.ControllerContext, viewName, false);

                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }

                ViewContext viewContext = new ViewContext(
                    this.ControllerContext,
                    viewResult.View,
                    this.ViewData,
                    this.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                Task.Run(() => viewResult.View.RenderAsync(viewContext)).Wait();

                return writer.GetStringBuilder().ToString();
            }
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