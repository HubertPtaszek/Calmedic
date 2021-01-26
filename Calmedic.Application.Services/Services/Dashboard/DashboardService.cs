using Calmedic.Data;
using Microsoft.AspNetCore.Http;

namespace Calmedic.Application
{
    public class DashboardService : ServiceBase, IDashboardService
    {
        #region Dependencies
        public IVisitRepository VisitRepository { get; set; }
        public IClinicRepository ClinicRepository { get; set; }
        public IPatientRepository PatientRepository { get; set; }
        public IDoctorRepository DoctorRepository { get; set; }
        public DashboardConverter VisitConverter { get; set; }
        #endregion

        public virtual ClinicDashboardVM GetClinicDashboardVM(HttpContext httpContext)
        {
            ClinicDashboardVM model = new ClinicDashboardVM();
            return model;
        }
    }
}
