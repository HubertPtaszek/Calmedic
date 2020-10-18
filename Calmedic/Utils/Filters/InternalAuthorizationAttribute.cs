using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Calmedic.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calmedic.Utils
{
    public class InternalAuthorizationAttribute : Attribute, IAuthorizationFilter, IOrderedFilter
    {
        private static object _lockObject = new object();
        public int Order { get; set; } = 0;

        public InternalAuthorizationAttribute()
        {

        }

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            MainContext context = filterContext.HttpContext.RequestServices.GetService(typeof(MainContext)) as MainContext;
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
                if (controllerActionDescriptor != null && !controllerActionDescriptor.MethodInfo.GetCustomAttributes(false).ToList().Any(x => x is AllowAnonymousAttribute))
                {
                    filterContext.Result = new UnauthorizedResult();
                    return;
                }
                return;
            }
            AppUserData userData = UserHelper.GetUserData(filterContext.HttpContext);

            context.PersonId = userData.Id;
            context.Roles = userData.Roles;
        }
    }
}