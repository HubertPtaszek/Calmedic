using Calmedic.Domain;
using Calmedic.Utils;
using System.Collections.Generic;

namespace Calmedic.Data
{
    public interface IClinicRepository : IRepository<Clinic>
    {
        List<SelectModelBinder<int>> GetClinicsToSelect();
    }
}