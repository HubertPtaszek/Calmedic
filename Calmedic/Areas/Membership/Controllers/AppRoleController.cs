using Calmedic.Application;
using Calmedic.Dictionaries;
using Calmedic.Utils;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.Membership.Controllers
{
    [Area(AreaNames.Membership_Area)]
    public class AppRoleController : AppController
    {
        #region Dependencies

        private readonly IAppRoleService _appRoleService;

        #endregion Dependencies

        public AppRoleController(IAppRoleService appRoleService)
        {
            _appRoleService = appRoleService;
        }

        [HttpGet]
        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult GetData(DataSourceLoadOptions loadOptions)
        {
            var data = _appRoleService.GetRoles(loadOptions);
            return CustomJson(data);
        }

        [HttpGet]
        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult GetRoleUsers(DataSourceLoadOptions loadOptions, int roleId)
        {
            var data = _appRoleService.GetRoleUsers(loadOptions, roleId);
            return CustomJson(data);
        }

        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult Details(int id)
        {
            AppRoleDetailsVM model = _appRoleService.GetAppRoleDetailsVM(id);
            return View(model);
        }

        [HttpGet]
        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult GetUsersforAssign(DataSourceLoadOptions loadOptions, int roleId)
        {
            var data = _appRoleService.GetUsersforAssign(loadOptions, roleId);
            return CustomJson(data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult AssignUserToRole(int userId, int roleId)
        {
            _appRoleService.AddUserToRole(userId, roleId);
            return CustomJson(true);
        }

        [HttpDelete, ValidateAntiForgeryToken]
        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult UnassignUserFromRole(int userId, int roleId)
        {
            _appRoleService.RemoveUserFromRole(userId, roleId);
            return CustomJson(true);
        }
    }
}