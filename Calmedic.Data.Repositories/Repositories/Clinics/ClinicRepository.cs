using Calmedic.Domain;
using Calmedic.EntityFramework;
using Calmedic.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Calmedic.Data
{
    public class ClinicRepository : Repository<Clinic, MainDatabaseContext>, IClinicRepository
    {
        public ClinicRepository(MainDatabaseContext context) : base(context)
        { }

        public List<SelectModelBinder<int>> GetClinicsToSelect()
        {
            List<SelectModelBinder<int>> result = _dbset.Select(x => new SelectModelBinder<int>()
            {
                Value = x.Id,
                Text = x.Name
            }).OrderBy(x => x.Text).ToList();
            return result;
        }
    }
}
