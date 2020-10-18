using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Calmedic.Utils
{
    public class FormatLogHelper
    {
        public static string FormatErrorMessage(Controller controller, string accountId = null, string message = null, string userName = null)
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine(string.Format("Date: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            errorMessage.AppendLine(string.Format("Action: {0}", controller.RouteData.Action()));
            errorMessage.AppendLine(string.Format("Controller: {0}", controller.RouteData.Controller()));
            errorMessage.AppendLine(string.Format("Area: {0}", controller.RouteData.Area() ?? string.Empty));
            errorMessage.AppendLine(string.Format("HttpMethod: {0}", controller.Request.Method));
            errorMessage.AppendLine(string.Format("Url: {0}", controller.Request.GetEncodedUrl()));

            if (!accountId.IsNullOrEmpty())
            {
                errorMessage.AppendLine(string.Format("UserId: {0}", accountId));
            }
            if (!userName.IsNullOrEmpty())
            {
                errorMessage.AppendLine(string.Format("UserName: {0}", userName));
            }
            var routeData = controller.RouteData.RouteValues();
            if (routeData.Any())
            {
                errorMessage.AppendLine(string.Format("Route data:"));
                foreach (var value in routeData)
                {
                    errorMessage.AppendLine(string.Format("{0}: {1}", value.Key, value.Value));
                }
            }
            var model = controller.ViewData.Model();
            if (model.Keys.Any())
            {
                errorMessage.AppendLine(string.Format("Model data:"));
                foreach (var key in model.Keys)
                {
                    var value = model[key] is IEnumerable<string>
                        ? string.Join(", ", model[key] as IEnumerable<string>)
                        : model[key];
                    errorMessage.AppendLine(string.Format("{0}: {1}", key, value));
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                errorMessage.AppendLine("Message " + message);
            }

            return errorMessage.ToString();
        }
    }
}