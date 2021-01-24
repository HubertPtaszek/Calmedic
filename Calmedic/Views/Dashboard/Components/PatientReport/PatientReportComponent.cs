using Calmedic.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calmedic.Core.Views.Dashboard.Components.PatientReport
{
    [ViewComponent(Name = "PatientReport")]
    public class PatientReportComponent : ViewComponent
    {
        public PatientReportComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("PatientReport");
        }
    }
}