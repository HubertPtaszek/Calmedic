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
    }
}