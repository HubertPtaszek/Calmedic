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

        public List<DoctorReportVM> DoctorReport { get; set; }
        public List<PatientReportVM> PatientReportVM { get; set; }
        public List<VisitReportVM> VisitReport { get; set; }
        public List<PatientsWidgetVM> PatientsWidget { get; set; }
        public List<VisitsWidgetVM> VisitsWidget { get; set; }
    }
}