using Calmedic.Domain;
using Calmedic.EntityFramework;

namespace Calmedic.Data
{
    public class DoctorRepository : Repository<Doctor, MainDatabaseContext>, IDoctorRepository
    {
        public DoctorRepository(MainDatabaseContext context) : base(context)
        { }
    }
}
