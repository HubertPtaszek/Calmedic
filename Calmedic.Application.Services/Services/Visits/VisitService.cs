using Calmedic.Data;
using Calmedic.Domain;

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

        public virtual object GetVisits()
        {
            return VisitRepository.GetVisits(1, null);
        }

        public virtual void Add()
        {
            Visit visit = new Visit() {
                ClinicId = 1,
                DateFrom = new System.DateTime(2021, 1, 28, 14, 0, 0),
                DateTo = new System.DateTime(2021, 1, 28, 14, 30, 0),
                DoctorId = 2,
                Description = "",
                PatientId = 1,
                Status = Dictionaries.VisitStatus.Waiting
            };
            VisitRepository.Add(visit);
        }
    }
}
