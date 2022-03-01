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
        private readonly IAppMailMessageService _appMailMessageService;

        #endregion Dependencies

        public AppUserController(IWebHostEnvironment webHostEnvironment, IAppUserService appUserService, IAppMailMessageService appMailMessageService)
        {
            _webHostEnvironment = webHostEnvironment;
            _appUserService = appUserService;
            _appMailMessageService = appMailMessageService;
        }

        [AppRoleAuthorization(AppRoleType.Administrator)]
        public IActionResult Index()
        {
            return View();
        }

        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult Add()
        {
            AppUserAddVM model = _appUserService.GetAppUserAddVM();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult Add(AppUserAddVM model)
        {
            if (ModelState.IsValid)
            {
                int id = _appUserService.Add(model);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = "__userId__", code = "__code__" }, protocol: Request.Scheme);
                _appMailMessageService.AddCreateConfirmationMessage(id, callbackUrl);
                return CustomJson(id);
            }
            return CustomJson(null);
        }

        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult Details(int id)
        {
            AppUserDetailsVM model = _appUserService.GetAppUserDetailsVM(id);
            return View(model);
        }

        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult Edit(int id)
        {
            AppUserEditVM model = new AppUserEditVM();//todo
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