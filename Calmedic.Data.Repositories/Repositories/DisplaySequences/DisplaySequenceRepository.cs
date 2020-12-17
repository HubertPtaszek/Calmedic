using Calmedic.Domain;
using Calmedic.EntityFramework;

namespace Calmedic.Data
{
    public class DisplaySequenceRepository : Repository<DisplaySequence, MainDatabaseContext>, IDisplaySequenceRepository
    {
        public DisplaySequenceRepository(MainDatabaseContext context) : base(context)
        { }
    }
}
