using Calmedic.Domain;
using Calmedic.EntityFramework;

namespace Calmedic.Data
{
    public class VisitRepository : Repository<Visit, MainDatabaseContext>, IVisitRepository
    {
        public VisitRepository(MainDatabaseContext context) : base(context)
        { }
    }
}