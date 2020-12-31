using Calmedic.Data;
using DevExtreme.AspNet.Data;

namespace Calmedic.Application
{
    public class AppRoleService : ServiceBase, IAppRoleService
    {
        #region Dependencies
        public IAppRoleRepository AppRoleRepository { get; set; }
        public AppRoleConverter AppRoleConverter { get; set; }
        #endregion

        public virtual object GetRoles(DataSourceLoadOptionsBase loadOptions)
        {
            return AppRoleRepository.GetRoles(loadOptions);
        }
    }
}
