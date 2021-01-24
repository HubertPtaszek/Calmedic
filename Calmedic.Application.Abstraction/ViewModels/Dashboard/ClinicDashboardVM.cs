using System.Collections.Generic;

namespace Calmedic.Application
{
    public class ClinicDashboardVM
    {
        public ClinicDashboardVM()
        {
            PatientsWidget = new List<PatientsWidgetVM>();
            VisitsWidget = new List<VisitsWidgetVM>();
        }

        public List<PatientsWidgetVM> PatientsWidget { get; set; }
        public List<VisitsWidgetVM> VisitsWidget { get; set; }
    }
}