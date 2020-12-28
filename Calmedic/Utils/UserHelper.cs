using Microsoft.AspNetCore.Http;
using Calmedic.Application;
using Calmedic.Resources.Shared;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using Calmedic.Dictionaries;

namespace Calmedic.Utils
{
    public class UserHelper
    {
        public static AppUserData GetUserData(HttpContext httpContext)
        {
            string appIdentityUserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (appIdentityUserId.IsNullOrEmpty())
                throw new AuthorizationException(ErrorResource.NoLoginAdded);
            AppUserData userData = httpContext.Session.GetObject<AppUserData>(SessionVariableNames.AppUserData);
            if (userData == null || userData.AppIdentityUserId != appIdentityUserId || userData.ValidDate < DateTime.Now)
            {
                IAppUserService userService = httpContext.RequestServices.GetService(typeof(IAppUserService)) as IAppUserService;
                userData = userService.GetUserDataByAppIdentityUserId(appIdentityUserId);
                if (userData == null)
                    throw new AuthorizationException(ErrorResource.NoLoginAdded);
                httpContext.Session.SetObject(SessionVariableNames.AppUserData, userData);
            }
            return userData;
        }

        public static bool UserHaveRole(HttpContext httpContext, params AppRoleType[] roles)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }
            IList<AppRoleType> userRoles = GetUserRoles(httpContext);
            foreach (AppRoleType role in roles)
            {
                if (userRoles.Contains(role))
                {
                    return true;
                }
            }
            return false;
        }

        public static IList<AppRoleType> GetUserRoles(HttpContext httpContext)
        {
            AppUserData userData = GetUserData(httpContext);
            IList<AppRoleType> userRoles = userData.Roles;
            return userRoles;
        }
    }
}