using Calmedic.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calmedic.Core.Views.Dashboard.Components.DoctorReport
{
    [ViewComponent(Name = "DoctorReport")]
    public class DoctorReportComponent : ViewComponent
    {
        public DoctorReportComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("DoctorReport");
        }
    }
}