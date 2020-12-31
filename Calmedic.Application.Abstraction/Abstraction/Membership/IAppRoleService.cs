using DevExtreme.AspNet.Data;

namespace Calmedic.Application
{
    public interface IAppRoleService : IService
    {
        object GetRoles(DataSourceLoadOptionsBase loadOptions);
    }
}
