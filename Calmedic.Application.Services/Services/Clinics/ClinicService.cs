using Calmedic.Data;

namespace Calmedic.Application
{
    public class ClinicService : ServiceBase, IClinicService
    {
        #region Dependencies

        public IClinicRepository ClinicRepository { get; set; }
        public IClinicUserRepository ClinicUserRepository { get; set; }

        #endregion Dependencies
    }
}