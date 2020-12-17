using Calmedic.Domain;
using Calmedic.EntityFramework;

namespace Calmedic.Data
{
    public class SpecializationRepository : Repository<Specialization, MainDatabaseContext>, ISpecializationRepository
    {
        public SpecializationRepository(MainDatabaseContext context) : base(context)
        { }
    }
}
