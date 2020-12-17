using Calmedic.Domain;
using Calmedic.EntityFramework;

namespace Calmedic.Data
{
    public class ClinicRepository : Repository<Clinic, MainDatabaseContext>, IClinicRepository
    {
        public ClinicRepository(MainDatabaseContext context) : base(context)
        { }
    }
}
