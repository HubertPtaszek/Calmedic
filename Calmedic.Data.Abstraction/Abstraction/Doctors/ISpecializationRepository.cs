using Calmedic.Domain;
using Calmedic.Utils;
using System.Collections.Generic;

namespace Calmedic.Data
{
    public interface ISpecializationRepository : IRepository<Specialization>
    {
        List<SelectModelBinder<int>> GetSpecializationsToSelect();
    }
}