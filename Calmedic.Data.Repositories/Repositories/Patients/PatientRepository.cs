using Calmedic.Domain;
using Calmedic.EntityFramework;

namespace Calmedic.Data
{
    public class PatientRepository : Repository<Patient, MainDatabaseContext>, IPatientRepository
    {
        public PatientRepository(MainDatabaseContext context) : base(context)
        { }
    }
}
