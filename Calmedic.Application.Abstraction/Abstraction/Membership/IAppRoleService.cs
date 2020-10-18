using DevExtreme.AspNet.Data;

namespace Calmedic.Application
{
    public interface IAppRoleService : IService
    {
        object GetRolesToList(DataSourceLoadOptionsBase loadOptions);
    }
}
