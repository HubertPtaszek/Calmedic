using Calmedic.Domain;
using DevExtreme.AspNet.Data;

namespace Calmedic.Data
{
    public interface IPatientRepository : IRepository<Patient>
    {
        object GetPatients(DataSourceLoadOptionsBase loadOptions);
    }
}