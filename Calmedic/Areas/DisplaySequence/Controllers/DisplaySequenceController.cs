using Calmedic.Application;
using Calmedic.Dictionaries;
using Calmedic.Utils;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.DisplaySequence.Controllers
{
    [Area(AreaNames.DisplaySequence_Area)]
    public class DisplaySequenceController : AppController
    {
        #region Dependencies

        private readonly IDisplaySequenceService _displaySequenceService;

        #endregion Dependencies

        public DisplaySequenceController(IDisplaySequenceService displaySequenceService)
        {
            _displaySequenceService = displaySequenceService;
        }

        [HttpGet]
        [AppRoleAuthorization(AppRoleType.Clinic)]
        public ActionResult GetData(DataSourceLoadOptions loadOptions)
        {
            var data = _displaySequenceService.GetDisplaySequence(loadOptions);
            return CustomJson(data);
        }

        [AppRoleAuthorization(AppRoleType.Clinic)]
        public ActionResult Index()
        {
            return View(new DisplaySequenceListVM());
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AppRoleAuthorization(AppRoleType.Clinic)]
        public ActionResult SetElementLower(int elementId)
        {
            _displaySequenceService.SetElementLower(elementId);
            return CustomJson(true);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AppRoleAuthorization(AppRoleType.Clinic)]
        public ActionResult SetElementHigher(int elementId)
        {
            _displaySequenceService.SetElementHigher(elementId);
            return CustomJson(true);
        }
    }
}