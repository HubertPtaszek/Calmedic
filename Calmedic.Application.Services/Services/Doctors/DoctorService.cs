using Calmedic.Data;

namespace Calmedic.Application
{
    public class DoctorService : ServiceBase, IDoctorService
    {
        #region Dependencies

        public IDoctorRepository DoctorRepository { get; set; }
        public IClinicRepository ClinicRepository { get; set; }

        #endregion Dependencies
    }
}