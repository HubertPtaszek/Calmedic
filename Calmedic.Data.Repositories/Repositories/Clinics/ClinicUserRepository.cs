using Calmedic.Domain;
using Calmedic.EntityFramework;

namespace Calmedic.Data
{
    public class ClinicUserRepository : Repository<ClinicUser, MainDatabaseContext>, IClinicUserRepository
    {
        public ClinicUserRepository(MainDatabaseContext context) : base(context)
        { }
    }
}
