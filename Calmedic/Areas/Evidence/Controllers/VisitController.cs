using Calmedic.Application;
using Calmedic.Dictionaries;
using Calmedic.Utils;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Calmedic.Areas.Evidence.Controllers
{
    [Area(AreaNames.Evidence_Area)]
    public class VisitController : AppController
    {
        #region Dependencies

        private readonly IVisitService _visitService;

        #endregion Dependencies

        public VisitController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Clinic, AppRoleType.Reception })]
        public IActionResult Index()
        {
            return View(new VisitListVM());
        }

        [HttpGet]
        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Clinic, AppRoleType.Reception })]
        public ActionResult GetData(int clinicId, DateTime? selectedDate = null)
        {
            var data = _visitService.GetVisits();
            if (data != null)
                return CustomJson(data);
            else
                return CustomJson(new object());
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Clinic, AppRoleType.Reception })]
        public ActionResult Add()
        {
            return CustomJson(true);
        }
    }
}