using Calmedic.Application;
using Calmedic.Dictionaries;
using Calmedic.Utils;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.Evidence.Controllers
{
    [Area(AreaNames.Evidence_Area)]
    public class PatientController : AppController
    {
        #region Dependencies

        private readonly IPatientService _patientService;

        #endregion Dependencies

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Clinic, AppRoleType.Reception, AppRoleType.Doctor })]
        public ActionResult GetData(DataSourceLoadOptions loadOptions)
        {
            var data = _patientService.GetPatients(loadOptions);
            return CustomJson(data);
        }

        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Clinic, AppRoleType.Reception, AppRoleType.Doctor })]
        public ActionResult Index()
        {
            return View(new PatientListVM());
        }

        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Clinic, AppRoleType.Reception })]
        public ActionResult Add()
        {
            return View(new PatientAddVM());
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Clinic, AppRoleType.Reception })]
        public ActionResult Add(PatientAddVM model)
        {
            if (ModelState.IsValid)
            {
                int id = _patientService.Add(model);
                return RedirectToAction("Details", new { id });
            }
            return View(model);
        }

        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Clinic, AppRoleType.Reception })]
        public ActionResult Details(int id)
        {
            PatientDetailsVM model = _patientService.GetPatientDetailsVM(id);
            return View(model);
        }

        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Clinic, AppRoleType.Reception })]
        public ActionResult Edit()
        {
            return View(new PatientEditVM());
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Clinic, AppRoleType.Reception })]
        public ActionResult Edit(PatientEditVM model)
        {
            if (ModelState.IsValid)
            {
                int id = 0;//  _patientService.Edit(model);
                return RedirectToAction("Details", new { id });
            }
            return View(model);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Clinic, AppRoleType.Reception })]
        public ActionResult Delete(int id)
        {
            //_patientService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}