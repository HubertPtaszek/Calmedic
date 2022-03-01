using Calmedic.Domain;
using Calmedic.Utils;
using DevExtreme.AspNet.Data;
using System.Collections.Generic;

namespace Calmedic.Data
{
    public interface IClinicRepository : IRepository<Clinic>
    {
        List<SelectModelBinder<int>> GetClinicsToSelect();

        object GetClinics(DataSourceLoadOptionsBase loadOptions);

        object GetClinicDocotrs(DataSourceLoadOptionsBase loadOptions, int clinicId);

        object GetClinicsForUser(DataSourceLoadOptionsBase loadOptions, ClinicUser clinicUser);
    }
}