using Calmedic.Data;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Calmedic.Application
{
    public class DashboardService : ServiceBase, IDashboardService
    {
        #region Dependencies
        public IVisitRepository VisitRepository { get; set; }
        public IClinicRepository ClinicRepository { get; set; }
        public IPatientRepository PatientRepository { get; set; }
        public IDoctorRepository DoctorRepository { get; set; }
        public DashboardConverter DashboardConverter { get; set; }
        #endregion

        public virtual ClinicDashboardVM GetClinicDashboardVM(HttpContext httpContext)
        {
            AppUserData userData = httpContext.Session.GetObject<AppUserData>("AppUserData");
            ClinicDashboardVM model = new ClinicDashboardVM();
            List<PatientsWidgetDTO> patients = PatientRepository.GetNewestPatient();
            model.PatientsWidget = DashboardConverter.ToPatientsWidgetVM(patients);
            if (userData.ClinicId.HasValue)
            {
                List<VisitsWidgetDTO> visits = VisitRepository.GetCommingVisits(userData.ClinicId.Value);
                model.VisitsWidget = DashboardConverter.ToVisitsWidgetVM(visits);
                List<DoctorReportDTO> doctorReports = VisitRepository.GetTopDoctorsByVisitCount(userData.ClinicId.Value);
                model.DoctorReport = DashboardConverter.ToDoctorReportVM(doctorReports);
                List<PatientReportDTO> patientReports = new List<PatientReportDTO>();
                model.PatientReport = DashboardConverter.ToPatientReportVM(patientReports);
                List<VisitReportDTO> visitReports = new List<VisitReportDTO>();
                model.VisitReport = DashboardConverter.ToVisitReportVM(visitReports);
            }
            return model;
        }
    }
}
