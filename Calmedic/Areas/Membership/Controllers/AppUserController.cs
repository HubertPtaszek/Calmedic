using Calmedic.Application;
using Calmedic.Dictionaries;
using Calmedic.Utils;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.Membership.Controllers
{
    [Area(AreaNames.Membership_Area)]
    public class AppUserController : AppController
    {
        #region Dependencies

        private IWebHostEnvironment _webHostEnvironment;
        private readonly IAppUserService _appUserService;

        #endregion Dependencies

        public AppUserController(IWebHostEnvironment webHostEnvironment, IAppUserService appUserService)
        {
            _webHostEnvironment = webHostEnvironment;
            _appUserService = appUserService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            AppUserAddVM model = new AppUserAddVM(); //todo
            return View(model);
        }

        public ActionResult Details(int id)
        {
            AppUserDetailsVM model = _appUserService.GetAppUserDetailsVM(id);
            return View(model);
        }

        [HttpGet]
        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult GetData(DataSourceLoadOptions loadOptions)
        {
            var data = _appUserService.GetUsers(loadOptions);
            return CustomJson(data);
        }
    }
}