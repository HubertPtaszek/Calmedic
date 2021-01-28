using Calmedic.Domain;
using DevExtreme.AspNet.Data;
using System.Collections.Generic;

namespace Calmedic.Data
{
    public interface IPatientRepository : IRepository<Patient>
    {
        object GetPatients(DataSourceLoadOptionsBase loadOptions);
        List<PatientsWidgetDTO> GetNewestPatient();
    }
}