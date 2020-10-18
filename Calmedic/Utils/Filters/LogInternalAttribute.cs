using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Calmedic.Utils
{
    public class LogInternalAttribute : ActionFilterAttribute
    {
        public string Message { get; set; }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Logger logger = LogManager.GetLogger("InternalLogger");
            var user = filterContext.HttpContext.User;
            var accountId = user == null ? null : user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = user == null ? null : user.Identity?.Name;
            var message = "";

            var controller = filterContext.Controller as Controller;
            if (controller != null)
            {
                message = FormatLogHelper.FormatErrorMessage(controller, accountId, message, userName);
            }
            logger.Info(message);
        }
    }
}