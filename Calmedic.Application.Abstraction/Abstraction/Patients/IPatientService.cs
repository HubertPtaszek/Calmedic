using DevExtreme.AspNet.Data;

namespace Calmedic.Application
{
    public interface IPatientService : IService
    {
        object GetPatients(DataSourceLoadOptionsBase loadOptions);
        PatientDetailsVM GetPatientDetailsVM(int id);
        int Add(PatientAddVM model);
    }
}
