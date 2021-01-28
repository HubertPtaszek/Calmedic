using DevExtreme.AspNet.Data;

namespace Calmedic.Application
{
    public interface IDisplaySequenceService : IService
    {
        object GetDisplaySequence(DataSourceLoadOptionsBase loadOptions);
        void SetElementLower(int id);
        void SetElementHigher(int id);
    }
}
