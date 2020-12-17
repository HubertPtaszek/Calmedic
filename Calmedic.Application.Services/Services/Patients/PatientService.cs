using Calmedic.Data;

namespace Calmedic.Application
{
    public class PatientService : ServiceBase, IPatientService
    {
        #region Dependencies

        public IPatientRepository PatientRepository { get; set; }

        #endregion Dependencies
    }
}