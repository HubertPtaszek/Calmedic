using Calmedic.Data;

namespace Calmedic.Application
{
    public class VisitService : ServiceBase, IVisitService
    {
        #region Dependencies
        public IVisitRepository VisitRepository { get; set; }
        public IClinicRepository ClinicRepository { get; set; }
        public IPatientRepository PatientRepository { get; set; }
        public IDoctorRepository DoctorRepository { get; set; }
        public VisitConverter VisitConverter { get; set; }
        #endregion
    }
}
