using Calmedic.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calmedic.Core.Areas.Membership.Views.Clinic.Components.ClinicDoctorsList
{
    [ViewComponent(Name = "ClinicDoctorsList")]
    public class ClinicDoctorsListComponent : ViewComponent
    {
        public ClinicDoctorsListComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(ClinicDetailsVM model)
        {
            return View("ClinicDoctorsList", model);
        }
    }
}