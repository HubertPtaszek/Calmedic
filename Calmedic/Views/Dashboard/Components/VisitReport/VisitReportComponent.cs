using Calmedic.Application;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calmedic.Core.Views.Dashboard.Components.VisitReport
{
    [ViewComponent(Name = "VisitReport")]
    public class VisitReportComponent : ViewComponent
    {
        public VisitReportComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("VisitReport");
        }
    }
}