using Calmedic.Domain;
using Calmedic.EntityFramework;
using Calmedic.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Calmedic.Data
{
    public class SpecializationRepository : Repository<Specialization, MainDatabaseContext>, ISpecializationRepository
    {
        public SpecializationRepository(MainDatabaseContext context) : base(context)
        { }

        public List<SelectModelBinder<int>> GetSpecializationsToSelect()
        {
            List<SelectModelBinder<int>> result = _dbset.Select(x => new SelectModelBinder<int>()
            {
                Value = x.Id,
                Text = x.DisplayName
            }).OrderBy(x => x.Text).ToList();
            return result;
        }
    }
}