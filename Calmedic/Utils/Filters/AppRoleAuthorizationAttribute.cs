using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Calmedic.Dictionaries;
using Calmedic.Resources.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calmedic.Utils
{
    public class AppRoleAuthorizationAttribute : Attribute, IAuthorizationFilter, IOrderedFilter
    {
        private AppRoleType[] _appRoleTypes { get; set; }

        public int Order { get; set; } = 10;

        public AppRoleAuthorizationAttribute(params AppRoleType[] appRoleTypes)
        {
            _appRoleTypes = appRoleTypes;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            IList<AppRoleType> userFunctionalities = UserHelper.GetUserRoles(context.HttpContext);

            if (!_appRoleTypes.Where(x => userFunctionalities.Contains(x)).Any())
            {
                string message = String.Format(ErrorResource.AccessDenied, string.Join(", ", _appRoleTypes.Select(x => x.GetDisplayName())));
                throw new FunctionalityAuthorizationException(message);//rename
            }
        }
    }
}