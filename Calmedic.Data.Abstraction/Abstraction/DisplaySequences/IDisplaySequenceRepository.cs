using Calmedic.Domain;
using DevExtreme.AspNet.Data;

namespace Calmedic.Data
{
    public interface IDisplaySequenceRepository : IRepository<DisplaySequence>
    {
        object GetDisplaySequence(DataSourceLoadOptionsBase loadOptions);
        DisplaySequence GetHigher(int order, int branchId);
        DisplaySequence GetLower(int order, int branchId);
    }
}
